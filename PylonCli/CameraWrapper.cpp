#include "CameraWrapper.h"
#include <msclr/marshal_cppstd.h>
#include <GenApi/GenApi.h>
#include <vcclr.h>
#include <mutex>

using namespace PylonCli;
using namespace Pylon;
using namespace GenApi;
using msclr::interop::marshal_as;


// Mutex cho RetrieveResult (UserLoop)
static std::mutex g_grabMutex;

// Native ImageHandler (không nested)
class ImageHandler : public Pylon::CImageEventHandler {
    gcroot<PylonCli::Camera^> _parent;
public:
    explicit ImageHandler(PylonCli::Camera^ parent) : _parent(parent) {}
    void OnImageGrabbed(Pylon::CInstantCamera& cam, const Pylon::CGrabResultPtr& ptr) override {
        if (ptr && ptr->GrabSucceeded()) _parent->ProcessGrabbed(ptr);
    }
};

// ===== GenICam helpers =====
long long Camera::GetIntNode(INodeMap& nm, const char* name) {
    CIntegerPtr n = nm.GetNode(name);
    if (!n || !IsReadable(n)) throw std::runtime_error("Node not readable");
    return n->GetValue();
}
void Camera::SetIntNode(INodeMap& nm, const char* name, long long v) {
    CIntegerPtr n = nm.GetNode(name);
    if (!n || !IsWritable(n)) throw std::runtime_error("Node not writable");
    long long mn = n->GetMin(), mx = n->GetMax(), inc = n->GetInc();
    if (v < mn) v = mn; if (v > mx) v = mx;
    if (inc > 1) v = mn + ((v - mn) / inc) * inc;
    n->SetValue(v);
}
double Camera::GetFloatNode(INodeMap& nm, const char* name) {
    CFloatPtr n = nm.GetNode(name);
    if (!n || !IsReadable(n)) throw std::runtime_error("Node not readable");
    return n->GetValue();
}
void Camera::SetFloatNode(INodeMap& nm, const char* name, double v) {
    CFloatPtr n = nm.GetNode(name);
    if (!n || !IsWritable(n)) throw std::runtime_error("Node not writable");
    double mn = n->GetMin(), mx = n->GetMax();
    if (v < mn) v = mn; if (v > mx) v = mx;
    n->SetValue(v);
}
double Camera::GetFloatAny(INodeMap& nm, std::initializer_list<const char*> names) {
    for (auto s : names) { CFloatPtr n = nm.GetNode(s); if (n && IsReadable(n)) return n->GetValue(); }
    throw std::runtime_error("No float node readable");
}
void Camera::SetFloatAny(INodeMap& nm, std::initializer_list<const char*> names, double v) {
    for (auto s : names) {
        CFloatPtr n = nm.GetNode(s); if (n && IsWritable(n)) {
            double mn = n->GetMin(), mx = n->GetMax(); if (v < mn) v = mn; if (v > mx) v = mx; n->SetValue(v); return;
        }
    }
    throw std::runtime_error("No float node writable");
}
void Camera::TrySetEnum(INodeMap& nm, const char* en, const char* entry) {
    CEnumerationPtr e = nm.GetNode(en); if (!e || !IsWritable(e)) return;
    CEnumEntryPtr ent = e->GetEntryByName(entry);
    if (ent && IsReadable(ent)) e->SetIntValue(ent->GetValue());
}

// ===== Pixel helpers =====
bool Camera::DetectIsColorSensor() {
    try {
        CEnumerationPtr cf = _cam->GetNodeMap().GetNode("ColorFilter");
        if (cf && IsReadable(cf)) return true;
        CEnumerationPtr pf = _cam->GetNodeMap().GetNode("PixelFormat");
        if (pf && IsReadable(pf)) {
            CEnumEntryPtr cur = pf->GetCurrentEntry();
            if (cur && IsReadable(cur)) {
                std::string sym = cur->GetSymbolic().c_str();
                if (sym.find("Bayer") != std::string::npos ||
                    sym.find("RGB") != std::string::npos ||
                    sym.find("YUV") != std::string::npos) return true;
            }
        }
    }
    catch (...) {}
    return false;
}
void Camera::ConfigureConverterForOutput() {
    if (!_conv) return;
    OutputPixel want = _desiredOutput;
    if (want == OutputPixel::Auto)
        want = DetectIsColorSensor() ? OutputPixel::BGR8 : OutputPixel::Mono8;

    if (want == OutputPixel::Mono8) { _conv->OutputPixelFormat = PixelType_Mono8; _activeChannels = 1; }
    else { _conv->OutputPixelFormat = PixelType_BGR8packed; _activeChannels = 3; }
    _conv->OutputBitAlignment = OutputBitAlignment_MsbAligned;
}

// ===== Double-buffer =====
CPylonImage* Camera::NextBuffer() { _bufIndex = (_bufIndex + 1) & 1; return _bufIndex ? _bufB : _bufA; }
CPylonImage* Camera::CurrentBuffer() { return _bufIndex ? _bufB : _bufA; }

// ===== Lifecycle =====
Camera::Camera() {}
Camera::~Camera() { this->!Camera(); }


cli::array<String^>^ Camera::List() {
    if (!s_pylonInited) { PylonInitialize(); s_pylonInited = true; }
    CTlFactory& tl = CTlFactory::GetInstance();
    DeviceInfoList_t devs; tl.EnumerateDevices(devs);
    auto out = gcnew cli::array<String^>((int)devs.size());
    for (int i = 0; i < (int)devs.size(); ++i) {
        const char* u = devs[i].GetUserDefinedName();
        if (u && *u) out[i] = gcnew String(u);
        else {
            std::string s = std::string(devs[i].GetModelName().c_str()) + "_" +
                std::string(devs[i].GetSerialNumber().c_str());
            out[i] = gcnew String(s.c_str());
        }
    }
    return out;
}

void Camera::Open(System::String^ name) {
    try {
        if (!s_pylonInited) { PylonInitialize(); s_pylonInited = true; }
        if (_opened) { _lastError = "Already open"; return; }

        std::string want = marshal_as<std::string>(name);
        CTlFactory& tl = CTlFactory::GetInstance();
        DeviceInfoList_t devs; tl.EnumerateDevices(devs);
        if (devs.empty()) { _lastError = "No cameras"; return; }

        bool found = false; CDeviceInfo picked;
        for (auto& d : devs) {
            const char* u = d.GetUserDefinedName();
            std::string byUser = (u && *u) ? std::string(u) : "";
            std::string byModel = std::string(d.GetModelName().c_str()) + "_" +
                std::string(d.GetSerialNumber().c_str());
            if (byUser == want || byModel == want) { picked = d; found = true; break; }
        }
        if (!found) { _lastError = "Not found"; return; }

        _cam = new CInstantCamera(tl.CreateDevice(picked));
        _cam->Open();

        _conv = new CImageFormatConverter();
        ConfigureConverterForOutput();

        if (!_bufA) _bufA = new CPylonImage();
        if (!_bufB) _bufB = new CPylonImage();
        _bufIndex = 0;

        _opened = true; _lastError = nullptr;
    }
    catch (const GenericException& e) { _lastError = gcnew String(e.GetDescription()); }
    catch (const std::exception& e) { _lastError = gcnew String(e.what()); }
}

void Camera::Start() { Start(GrabMode::InternalLoop); }
void Camera::Start(GrabMode mode)
{
    try {
        if (!_cam) { _lastError = "Not open"; return; }
        _mode = mode;

        if (_cam->IsGrabbing())
            _cam->StopGrabbing();

        if (mode == GrabMode::InternalLoop)
        {
            // Gỡ handler cũ nếu có
            if (_imgHandlerPtr) {
                _cam->DeregisterImageEventHandler(_imgHandlerPtr);
                delete _imgHandlerPtr;
                _imgHandlerPtr = nullptr;
            }

            // Tạo mới handler và register
            _imgHandlerPtr = new ImageHandler(this);
            _cam->RegisterImageEventHandler(_imgHandlerPtr,
                Pylon::RegistrationMode_ReplaceAll,
                Pylon::Cleanup_None);

            _cam->StartGrabbing(Pylon::GrabStrategy_LatestImageOnly,
                Pylon::GrabLoop_ProvidedByInstantCamera);
        }
        else // UserLoop
        {
            if (_imgHandlerPtr) {
                _cam->DeregisterImageEventHandler(_imgHandlerPtr);
                delete _imgHandlerPtr;
                _imgHandlerPtr = nullptr;
            }
            _cam->StartGrabbing(Pylon::GrabStrategy_LatestImageOnly,
                Pylon::GrabLoop_ProvidedByUser);
        }

        ConfigureConverterForOutput();
        _lastError = nullptr;
    }
    catch (const GenericException& e) {
        _lastError = gcnew String(e.GetDescription());
    }
}
void Camera::ChangeGrabLoop(bool useInternal)
{
    try {
        if (!_cam) { _lastError = "Not open"; return; }

        if (_cam->IsGrabbing())
            _cam->StopGrabbing();

        if (useInternal) {
            // Internal loop
            if (_imgHandlerPtr) {
                // gỡ handler cũ
                delete _imgHandlerPtr;
                _imgHandlerPtr = nullptr;
            }
            _imgHandlerPtr = new ImageHandler(this);
            _cam->RegisterImageEventHandler(_imgHandlerPtr,
                Pylon::RegistrationMode_ReplaceAll,
                Pylon::Cleanup_None);
            _cam->StartGrabbing(Pylon::GrabStrategy_LatestImageOnly,
                Pylon::GrabLoop_ProvidedByInstantCamera);
        }
        else {
            // User loop
            if (_imgHandlerPtr) {
                delete _imgHandlerPtr;
                _imgHandlerPtr = nullptr;
            }
            _cam->StartGrabbing(Pylon::GrabStrategy_LatestImageOnly,
                Pylon::GrabLoop_ProvidedByUser);
        }

        _lastError = nullptr;
    }
    catch (const GenericException& e) {
        _lastError = gcnew String(e.GetDescription());
    }
}

void Camera::Stop()
{
    try {
        if (_cam && _cam->IsGrabbing())
            _cam->StopGrabbing();
    }
    catch (...) {
        _lastError = "Stop error"; }
}

void Camera::Close()
{
    try {
        if (_cam) {
            // Gỡ & delete handler
            if (_imgHandlerPtr) {
                _cam->DeregisterImageEventHandler(_imgHandlerPtr);
                delete _imgHandlerPtr;
                _imgHandlerPtr = nullptr;
            }
            if (_cam->IsGrabbing()) _cam->StopGrabbing();
            if (_cam->IsOpen())     _cam->Close();
            delete _cam;
            _cam = nullptr;
        }
        _opened = false; _lastError = nullptr;
    }
    catch (...) { _lastError = "Close error"; }
}

Camera::!Camera()
{
    try {
        Stop();
        Close();
        if (_conv) { delete _conv; _conv = nullptr; }
        if (_bufA) { delete _bufA; _bufA = nullptr; }
        if (_bufB) { delete _bufB; _bufB = nullptr; }
    }
    catch (...) {}
}

bool Camera::IsOpen() { return _opened && _cam && _cam->IsOpen(); }

// ===== Output pixel API =====
void Camera::SetOutputPixel(OutputPixel fmt) { _desiredOutput = fmt; if (_conv) ConfigureConverterForOutput(); }
OutputPixel Camera::GetOutputPixel() { return (_activeChannels == 1) ? OutputPixel::Mono8 : OutputPixel::BGR8; }

float Camera::SetWidth(float v)
{
    try {
        CIntegerPtr n = _cam->GetNodeMap().GetNode("Width");
        if (!n || !IsWritable(n)) throw std::runtime_error("Width not writable");
        long long mn = n->GetMin(), mx = n->GetMax(), inc = n->GetInc();
        if (v < mn) v = (int)mn;
        if (v > mx) v = (int)mx;
        if (inc > 1) v = (int)(mn + ((v - mn) / inc) * inc);
        n->SetValue(v);
        _lastError = nullptr;
        return (int)n->GetValue();
    }
    catch (...) { _lastError = "SetWidth fail"; return 0; }
}

float Camera::SetHeight(float v)
{
    try {
        CIntegerPtr n = _cam->GetNodeMap().GetNode("Height");
        if (!n || !IsWritable(n)) throw std::runtime_error("Height not writable");
        long long mn = n->GetMin(), mx = n->GetMax(), inc = n->GetInc();
        if (v < mn) v = (int)mn;
        if (v > mx) v = (int)mx;
        if (inc > 1) v = (int)(mn + ((v - mn) / inc) * inc);
        n->SetValue(v);
        _lastError = nullptr;
        return (int)n->GetValue();
    }
    catch (...) { _lastError = "SetHeight fail"; return 0; }
}

float Camera::SetOffsetX(float v)
{
    try {
        CIntegerPtr n = _cam->GetNodeMap().GetNode("OffsetX");
        if (!n || !IsWritable(n)) throw std::runtime_error("OffsetX not writable");
        long long mn = n->GetMin(), mx = n->GetMax(), inc = n->GetInc();
        if (v < mn) v = (int)mn;
        if (v > mx) v = (int)mx;
        if (inc > 1) v = (int)(mn + ((v - mn) / inc) * inc);
        n->SetValue(v);
        _lastError = nullptr;
        return (int)n->GetValue();
    }
    catch (...) { _lastError = "SetOffsetX fail"; return 0; }
}

float Camera::SetOffsetY(float v)
{
    try {
        CIntegerPtr n = _cam->GetNodeMap().GetNode("OffsetY");
        if (!n || !IsWritable(n)) throw std::runtime_error("OffsetY not writable");
        long long mn = n->GetMin(), mx = n->GetMax(), inc = n->GetInc();
        if (v < mn) v = (int)mn;
        if (v > mx) v = (int)mx;
        if (inc > 1) v = (int)(mn + ((v - mn) / inc) * inc);
        n->SetValue(v);
        _lastError = nullptr;
        return (int)n->GetValue();
    }
    catch (...) { _lastError = "SetOffsetY fail"; return 0; }
}

float Camera::SetExposure(float us)
{
    try {
        TrySetEnum(_cam->GetNodeMap(), "ExposureAuto", "Off");
        CFloatPtr n = _cam->GetNodeMap().GetNode("ExposureTime");
        if (!n || !IsWritable(n)) n = _cam->GetNodeMap().GetNode("ExposureTimeAbs");
        if (!n || !IsWritable(n)) throw std::runtime_error("Exposure not writable");

        double mn = n->GetMin(), mx = n->GetMax();
        if (us < mn) us = mn;
        if (us > mx) us = mx;
        n->SetValue(us);
        _lastError = nullptr;
        return n->GetValue();
    }
    catch (...) { _lastError = "SetExposure fail"; return 0; }
}

float Camera::SetGain(float v)
{
    try {
        TrySetEnum(_cam->GetNodeMap(), "GainAuto", "Off");
        TrySetEnum(_cam->GetNodeMap(), "GainSelector", "All");
        CFloatPtr n = _cam->GetNodeMap().GetNode("Gain");
        if (!n || !IsWritable(n)) throw std::runtime_error("Gain not writable");

        float mn = n->GetMin(), mx = n->GetMax();
        if (v < mn) v = mn;
        if (v > mx) v = mx;
        n->SetValue(v);
        _lastError = nullptr;
        return n->GetValue();
    }
    catch (...) { _lastError = "SetGain fail"; return 0; }
}

float Camera::SetBlackLevel(float v)
{
    try {
        TrySetEnum(_cam->GetNodeMap(), "BlackLevelSelector", "All");
        CFloatPtr n = _cam->GetNodeMap().GetNode("BlackLevel");
        if (!n || !IsWritable(n)) n = _cam->GetNodeMap().GetNode("BlackLevelRaw");
        if (!n || !IsWritable(n)) throw std::runtime_error("BlackLevel not writable");

        float mn = n->GetMin(), mx = n->GetMax();
        if (v < mn) v = mn;
        if (v > mx) v = mx;
        n->SetValue(v);
        _lastError = nullptr;
        return n->GetValue();
    }
    catch (...) { _lastError = "SetBlackLevel fail"; return 0; }
}
void Camera::GetExposure(float% min, float% max, float% step, float% current)
{
    try {
        CFloatPtr n = _cam->GetNodeMap().GetNode("ExposureTime");
        if (!n || !IsReadable(n)) n = _cam->GetNodeMap().GetNode("ExposureTimeAbs");
        if (!n || !IsReadable(n)) throw std::runtime_error("Exposure node not readable");

        min = (float)n->GetMin();
        max = (float)n->GetMax();
        step = (float)n->GetInc();  // increment, có thể 0.0 nếu liên tục
        current = (float)n->GetValue();
        _lastError = nullptr;
    }
    catch (...) {
        min = max = step = current = 0.0f;
        _lastError = "GetExposure(min/max) fail";
    }
}

void Camera::GetGain(float% min, float% max, float% step, float% current)
{
    try {
        TrySetEnum(_cam->GetNodeMap(), "GainSelector", "All");
        CFloatPtr n = _cam->GetNodeMap().GetNode("Gain");
        if (!n || !IsReadable(n)) throw std::runtime_error("Gain node not readable");

        min = (float)n->GetMin();
        max = (float)n->GetMax();
        step = (float)n->GetInc();
        current = (float)n->GetValue();
        _lastError = nullptr;
    }
    catch (...) {
        min = max = step = current = 0.0f;
        _lastError = "GetGain(min/max) fail";
    }
}

void Camera::GetWidth(float% min, float% max, float% step, float% current)
{
    try {
        CIntegerPtr n = _cam->GetNodeMap().GetNode("Width");
        if (!n || !IsReadable(n)) throw std::runtime_error("Width node not readable");

        min = (int)n->GetMin();
        max = (int)n->GetMax();
        step = (int)n->GetInc();
        current = (int)n->GetValue();
        _lastError = nullptr;
    }
    catch (...) {
        min = max = step = current = 0;
        _lastError = "GetWidth(min/max) fail";
    }
}

void Camera::GetHeight(float% min, float% max, float% step, float% current)
{
    try {
        CIntegerPtr n = _cam->GetNodeMap().GetNode("Height");
        if (!n || !IsReadable(n)) throw std::runtime_error("Height node not readable");

        min = (int)n->GetMin();
        max = (int)n->GetMax();
        step = (int)n->GetInc();
        current = (int)n->GetValue();
        _lastError = nullptr;
    }
    catch (...) {
        min = max = step = current = 0;
        _lastError = "GetHeight(min/max) fail";
    }
}



void Camera::CenterX() {
    try {
        long long sw = GetIntNode(_cam->GetNodeMap(), "SensorWidth");
        int w = (int)GetIntNode(_cam->GetNodeMap(), "Width");
        SetOffsetX((int)((sw - w) / 2)); _lastError = nullptr;
    }
    catch (...) { _lastError = "CenterX fail"; }
}
void Camera::CenterY() {
    try {
        long long sh = GetIntNode(_cam->GetNodeMap(), "SensorHeight");
        int h = (int)GetIntNode(_cam->GetNodeMap(), "Height"); SetOffsetY((int)((sh - h) / 2)); _lastError = nullptr;
    }
    catch (...) { _lastError = "CenterY fail"; }
}

// ===== Params =====

void Camera::GetBlackLevel(float% min, float% max, float% step, float% current)
{
    try {
        TrySetEnum(_cam->GetNodeMap(), "BlackLevelSelector", "All");

        CFloatPtr n = _cam->GetNodeMap().GetNode("BlackLevel");
        if (!n || !IsReadable(n))
            n = _cam->GetNodeMap().GetNode("BlackLevelRaw");
        if (!n || !IsReadable(n))
            throw std::runtime_error("BlackLevel node not readable");

        min = (float)n->GetMin();
        max = (float)n->GetMax();
        step = (float)n->GetInc();     // thường 0.0 nếu liên tục
        current = (float)n->GetValue();

        _lastError = nullptr;
    }
    catch (...) {
        min = max = step = current = 0.0f;
        _lastError = "GetBlackLevel(min/max) fail";
    }
}


void Camera::GetOffsetX(float% min, float% max, float% step, float% current)
{
    try {
        CIntegerPtr n = _cam->GetNodeMap().GetNode("OffsetX");
        if (!n || !IsReadable(n)) throw std::runtime_error("OffsetX node not readable");

        min = (int)n->GetMin();
        max = (int)n->GetMax();
        step = (int)n->GetInc();
        current = (int)n->GetValue();
        _lastError = nullptr;
    }
    catch (...) {
        min = max = step = current = 0;
        _lastError = "GetOffsetX(min/max) fail";
    }
}

void Camera::GetOffsetY(float% min, float% max, float% step, float% current)
{
    try {
        CIntegerPtr n = _cam->GetNodeMap().GetNode("OffsetY");
        if (!n || !IsReadable(n)) throw std::runtime_error("OffsetY node not readable");

        min = (int)n->GetMin();
        max = (int)n->GetMax();
        step = (int)n->GetInc();
        current = (int)n->GetValue();
        _lastError = nullptr;
    }
    catch (...) {
        min = max = step = current = 0;
        _lastError = "GetOffsetY(min/max) fail";
    }
}

// ===== InternalLoop bridge =====
void Camera::ProcessGrabbed(const CGrabResultPtr& ptr) {
    try {
        CPylonImage* dst = NextBuffer();
        _conv->Convert(*dst, ptr);

        int w = (int)dst->GetWidth();
        int h = (int)dst->GetHeight();
        int ch = _activeChannels;
        int stride = w * ch; // packed Mono8/BGR8

        if (_frameReadyHandlers != nullptr) {
            _frameCount++;
            System::IntPtr buffer((unsigned char*)dst->GetBuffer()); // uchar*
            FrameReady(buffer, w, h, stride, ch);
            auto now = std::chrono::steady_clock::now();
            std::chrono::duration<double> elapsed = now - _lastFrameTime;
            if (elapsed.count() >= 1.0) {
                _emaFps = _frameCount / elapsed.count();
                _frameCount = 0;
                _lastFrameTime = now;
            }
        }
        _lastError = nullptr;
    }
    catch (...) { _lastError = "ProcessGrabbed error"; }
}

// ===== UserLoop API (uchar* qua IntPtr) =====
System::IntPtr Camera::GrabOneUcharPtr(int timeoutMs, int% w, int% h, int% stride, int% channels) {
  //  auto t0 = std::chrono::steady_clock::now();
    _frameCount++;
    w = h = stride = channels = 0;
    if (!_cam || !_cam->IsGrabbing()) { _lastError = "Not grabbing"; return System::IntPtr::Zero; }
    if (_mode != GrabMode::UserLoop) { _lastError = "GrabOneUcharPtr only in UserLoop"; return System::IntPtr::Zero; }

    std::lock_guard<std::mutex> lock(g_grabMutex);
    try {
        CGrabResultPtr res;
        if (_cam->RetrieveResult(timeoutMs, res, TimeoutHandling_Return)) {
            if (res && res->GrabSucceeded()) {
                CPylonImage* dst = NextBuffer();
                _conv->Convert(*dst, res);

                w = (int)dst->GetWidth();
                h = (int)dst->GetHeight();
                channels = _activeChannels;
                stride = w * channels; // packed

                _lastError = nullptr;
                // --- Update FPS ---
                // Cập nhật FPS mỗi ~1s
                auto now = std::chrono::steady_clock::now();
                std::chrono::duration<double> elapsed = now - _lastFrameTime;
                if (elapsed.count() >= 1.0) {
                    _emaFps = _frameCount / elapsed.count();
                    _frameCount = 0;
                    _lastFrameTime = now;
                }
               
                return System::IntPtr((unsigned char*)dst->GetBuffer()); // uchar*
            }
            else {
                _lastError = "Grab failed";
            }
        }
        else {
            _lastError = "Timeout";
        }
    }
    catch (const GenericException& e) { _lastError = gcnew System::String(e.GetDescription()); }
    catch (const std::exception& e) { _lastError = gcnew System::String(e.what()); }
    catch (...) { _lastError = "GrabOneUcharPtr unknown error"; }

    return System::IntPtr::Zero;
}

System::IntPtr Camera::GrabLatestUcharPtr(int% w, int% h, int% stride, int% channels) {
    w = h = stride = channels = 0;
    CPylonImage* cur = CurrentBuffer();
    if (!cur || !cur->IsValid() || !cur->GetBuffer()) { _lastError = "No latest frame"; return System::IntPtr::Zero; }
    w = (int)cur->GetWidth();
    h = (int)cur->GetHeight();
    channels = _activeChannels;
    stride = w * channels; // packed
    _lastError = nullptr;
    return System::IntPtr((unsigned char*)cur->GetBuffer()); // uchar*
}
double Camera::GetMeasuredFps()
{
    return _emaFps; // sẽ >0 sau khi có vài frame
}

double Camera::GetDeviceFps()
{
    try {
        return GetFloatAny(_cam->GetNodeMap(),
            { "ResultingFrameRate", "AcquisitionResultingFrameRate", "AcquisitionFrameRate" });
    }
    catch (...) {
        _lastError = "GetDeviceFps fail";
        return 0.0;
    }
}