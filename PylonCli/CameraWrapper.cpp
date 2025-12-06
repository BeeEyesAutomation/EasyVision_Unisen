#include "CameraWrapper.h"
#include <msclr/marshal_cppstd.h>
#include <GenApi/GenApi.h>
#include <vcclr.h>
#include <mutex>

using namespace PylonCli;
using namespace Pylon;
using namespace GenApi;
using msclr::interop::marshal_as;
static long long RoundToStepClamp_I(long long desired, long long vmin, long long vmax, long long inc)
{
    if (inc <= 0) inc = 1;
    if (desired < vmin) desired = vmin;
    if (desired > vmax) desired = vmax;
    const long long n = (desired - vmin + inc / 2) / inc; // round to nearest
    long long val = vmin + n * inc;
    if (val < vmin) val = vmin;
    if (val > vmax) val = vmax;
    return val;
}

static long long GetIncSafe(CIntegerPtr n)
{
    try { return n ? n->GetInc() : 1; }
    catch (...) { return 1; }
}

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
Camera::Camera() { _latestImageMutex = gcnew System::Threading::Mutex();
if (!_latestImage)
_latestImage = new CPylonImage();
}
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
        _bufA = new CPylonImage();
        _bufB = new CPylonImage();
        _bufIndex = 0;
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
void Camera::Start(GrabMode mode) {
    try {
        if (!_cam) { _lastError = "Not open"; return; }
        _mode = mode;

        if (_cam->IsGrabbing())
            _cam->StopGrabbing();

        if (mode == GrabMode::InternalLoop) {
            if (_imgHandlerPtr) {
                _cam->DeregisterImageEventHandler(_imgHandlerPtr);
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
            if (_imgHandlerPtr) {
                _cam->DeregisterImageEventHandler(_imgHandlerPtr);
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

//void Camera::Start(GrabMode mode)
//{
//    try {
//        if (!_cam) { _lastError = "Not open"; return; }
//        _mode = mode;
//
//        if (_cam->IsGrabbing())
//           Stop();
//
//        if (mode == GrabMode::InternalLoop)
//        {
//            // Gỡ handler cũ nếu có
//            if (_imgHandlerPtr) {
//                _cam->DeregisterImageEventHandler(_imgHandlerPtr);
//                delete _imgHandlerPtr;
//                _imgHandlerPtr = nullptr;
//            }
//
//            // Tạo mới handler và register
//            _imgHandlerPtr = new ImageHandler(this);
//            _cam->RegisterImageEventHandler(_imgHandlerPtr,
//                Pylon::RegistrationMode_ReplaceAll,
//                Pylon::Cleanup_None);
//
//            _cam->StartGrabbing(Pylon::GrabStrategy_LatestImageOnly,
//                Pylon::GrabLoop_ProvidedByInstantCamera);
//        }
//        else // UserLoop
//        {
//            if (_imgHandlerPtr) {
//                _cam->DeregisterImageEventHandler(_imgHandlerPtr);
//                delete _imgHandlerPtr;
//                _imgHandlerPtr = nullptr;
//            }
//            _cam->StartGrabbing(Pylon::GrabStrategy_LatestImageOnly,
//                Pylon::GrabLoop_ProvidedByUser);
//        }
//
//        ConfigureConverterForOutput();
//        _lastError = nullptr;
//    }
//    catch (const GenericException& e) {
//        _lastError = gcnew String(e.GetDescription());
//    }
//}
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
            if (_cam->IsGrabbing())_cam->StopGrabbing();
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
    _lastError = System::String::Empty;

    try {
        INodeMap& nm = _cam->GetNodeMap();
        CIntegerPtr node = nm.GetNode("Width");
        // Dừng rồi khôi phục đúng trạng thái ban đầu
        bool wasGrabbing = _cam->IsGrabbing();
        if (wasGrabbing)_cam->StopGrabbing();
        if (!node) { _lastError = "Node Width không tồn tại!"; return 0.0f; }
        if (!GenApi::IsWritable(node)) { _lastError = "Node Width không cho phép ghi!"; return 0.0f; }

      

        long long mn = node->GetMin();
        long long mx = node->GetMax();
        long long inc = GetIncSafe(node);

        long long desired = static_cast<long long>(std::llround(v));
        long long val = RoundToStepClamp_I(desired, mn, mx, inc);
        node->SetValue(val);
        long long applied = node->GetValue();

        if (wasGrabbing)Start();
        return static_cast<float>(applied);
    }
    catch (const GenICam::GenericException& e) {
        _lastError = gcnew System::String((std::string("SetWidth failed: ") + e.GetDescription()).c_str());
        return 0.0f;
    }
    catch (const std::exception& e) {
        _lastError = gcnew System::String((std::string("SetWidth failed: ") + e.what()).c_str());
        return 0.0f;
    }
    catch (...) {
        _lastError = "SetWidth fail: unknown error";
        return 0.0f;
    }
}

float Camera::SetHeight(float v)
{
    _lastError = System::String::Empty;

    try {
        INodeMap& nm = _cam->GetNodeMap();
        CIntegerPtr node = nm.GetNode("Height");
        bool wasGrabbing = _cam->IsGrabbing();
        if (wasGrabbing)_cam->StopGrabbing();
        if (!node) { _lastError = "Node Height không tồn tại!"; return 0.0f; }
        if (!GenApi::IsWritable(node)) { _lastError = "Node Height không cho phép ghi!"; return 0.0f; }

       

        long long mn = node->GetMin();
        long long mx = node->GetMax();
        long long inc = GetIncSafe(node);

        long long desired = static_cast<long long>(std::llround(v));
        long long val = RoundToStepClamp_I(desired, mn, mx, inc);
        node->SetValue(val);
        long long applied = node->GetValue();

        if (wasGrabbing) Start();
        return static_cast<float>(applied);
    }
    catch (const GenICam::GenericException& e) {
        _lastError = gcnew System::String((std::string("SetHeight failed: ") + e.GetDescription()).c_str());
        return 0.0f;
    }
    catch (const std::exception& e) {
        _lastError = gcnew System::String((std::string("SetHeight failed: ") + e.what()).c_str());
        return 0.0f;
    }
    catch (...) {
        _lastError = "SetHeight fail: unknown error";
        return 0.0f;
    }
}

float Camera::SetOffsetX(float v)
{
    _lastError = System::String::Empty;

    try {
        INodeMap& nm = _cam->GetNodeMap();

        // Một số model yêu cầu CenterX = false để set OffsetX
        CBooleanPtr centerX = nm.GetNode("CenterX");
        if (centerX && GenApi::IsWritable(centerX)) {
            try { centerX->SetValue(false); } catch (...) {}
        }

        CIntegerPtr node = nm.GetNode("OffsetX");
        if (!node || !GenApi::IsWritable(node)) { _lastError = "OffsetX not writable"; return 0.0f; }

        bool wasGrabbing = _cam->IsGrabbing();
        if (wasGrabbing)_cam->StopGrabbing();

        long long mn = node->GetMin();
        long long mx = node->GetMax();
        long long inc = GetIncSafe(node);

        long long desired = static_cast<long long>(std::llround(v));
        long long val = RoundToStepClamp_I(desired, mn, mx, inc);
        node->SetValue(val);
        long long applied = node->GetValue();

        if (wasGrabbing) Start();
        return static_cast<float>(applied);
    }
    catch (const GenICam::GenericException& e) {
        _lastError = gcnew System::String((std::string("SetOffsetX failed: ") + e.GetDescription()).c_str());
        return 0.0f;
    }
    catch (const std::exception& e) {
        _lastError = gcnew System::String((std::string("SetOffsetX failed: ") + e.what()).c_str());
        return 0.0f;
    }
    catch (...) {
        _lastError = "SetOffsetX fail";
        return 0.0f;
    }
}

float Camera::SetOffsetY(float v)
{
    try {
        //SetX
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
    _lastError = System::String::Empty;

    try {
        INodeMap& nm = _cam->GetNodeMap();

        // ExposureAuto = Off (nếu có)
        CEnumerationPtr eauto = nm.GetNode("ExposureAuto");
        if (eauto && GenApi::IsWritable(eauto)) {
            try { eauto->FromString("Off"); }
            catch (...) {}
        }

        // ExposureMode = Timed (nếu có)
        CEnumerationPtr emode = nm.GetNode("ExposureMode");
        if (emode && GenApi::IsWritable(emode)) {
            try { emode->FromString("Timed"); }
            catch (...) {}
        }

        // 1) Float: ExposureTime / ExposureTimeAbs
        CFloatPtr f = nm.GetNode("ExposureTime");
        if (!(f && GenApi::IsWritable(f))) {
            f = nm.GetNode("ExposureTimeAbs");
        }
        if (f && GenApi::IsWritable(f)) {
            double mn = f->GetMin();
            double mx = f->GetMax();
            double inc = 0.0;
            try { if (f->HasInc()) inc = f->GetInc(); }
            catch (...) { inc = 0.0; }

            // clamp + (nếu có inc>0) làm tròn theo bước
            double val = us;
            if (val < mn) val = mn;
            if (val > mx) val = mx;
            if (inc > 0.0) {
                double n = std::floor((val - mn) / inc + 0.5);
                val = mn + n * inc;
                if (val < mn) val = mn;
                if (val > mx) val = mx;
            }

            f->SetValue(val);
            return static_cast<float>(f->GetValue());
        }

        // 2) Integer: ExposureTimeRaw (+ factor nếu có)
        CIntegerPtr i = nm.GetNode("ExposureTimeRaw");
        if (i && GenApi::IsWritable(i)) {
            double tickToUs = 0.0;
            CFloatPtr fac = nm.GetNode("ExposureTimeRawAbsFactor");
            if (fac && GenApi::IsReadable(fac)) {
                try { tickToUs = fac->GetValue(); }
                catch (...) { tickToUs = 0.0; }
            }

            int64_t vmin = i->GetMin();
            int64_t vmax = i->GetMax();
            int64_t vinc = 0;
            try { vinc = i->GetInc(); }
            catch (...) { vinc = 0; }
            if (vinc <= 0) vinc = 1;

            int64_t desiredTicks = (tickToUs > 0.0)
                ? static_cast<int64_t>(std::llround(us / tickToUs))
                : static_cast<int64_t>(std::llround(us));

            // clamp + round-to-step
            if (desiredTicks < vmin) desiredTicks = vmin;
            if (desiredTicks > vmax) desiredTicks = vmax;
            int64_t n = (desiredTicks - vmin + vinc / 2) / vinc;
            int64_t valTicks = vmin + n * vinc;
            if (valTicks < vmin) valTicks = vmin;
            if (valTicks > vmax) valTicks = vmax;

            i->SetValue(valTicks);
            int64_t appliedTicks = i->GetValue();
            double appliedUs = (tickToUs > 0.0) ? appliedTicks * tickToUs
                : static_cast<double>(appliedTicks);
            return static_cast<float>(appliedUs);
        }

        _lastError = "Exposure not writable/found";
        return 0.0f;
    }
    catch (const GenICam::GenericException& e) {
        _lastError = gcnew System::String(("SetExposure() failed: " + std::string(e.GetDescription())).c_str());
        return 0.0f;
    }
    catch (const std::exception& e) {
        _lastError = gcnew System::String(("SetExposure() failed: " + std::string(e.what())).c_str());
        return 0.0f;
    }
    catch (...) {
        _lastError = "SetExposure() unknown error";
        return 0.0f;
    }
}


static double RoundToIncClamp_D(double desired, double vmin, double vmax, double inc)
{
    if (desired < vmin) desired = vmin;
    if (desired > vmax) desired = vmax;
    if (inc > 0.0) {
        const double n = std::floor((desired - vmin) / inc + 0.5); // làm tròn về bội số inc
        desired = vmin + n * inc;
        if (desired < vmin) desired = vmin;
        if (desired > vmax) desired = vmax;
    }
    return desired;
}


float Camera::SetGain(float v)
{
    _lastError = System::String::Empty;

    try {
        INodeMap& nm = _cam->GetNodeMap();

        // Tắt auto nếu có
        CEnumerationPtr gauto = nm.GetNode("GainAuto");
        if (gauto && GenApi::IsWritable(gauto)) {
            try { gauto->FromString("Off"); }
            catch (...) {}
        }

        // Chọn selector = All nếu có
        CEnumerationPtr gsel = nm.GetNode("GainSelector");
        if (gsel && GenApi::IsWritable(gsel)) {
            CEnumEntryPtr all = gsel->GetEntryByName("All");
            if (all && GenApi::IsAvailable(all)) {
                try { gsel->SetIntValue(all->GetValue()); }
                catch (...) {}
            }
        }

        // ===== 1) Float: Gain (dB) =====
        {
            CFloatPtr f = nm.GetNode("Gain");
            if (f && GenApi::IsWritable(f)) {
                double mn = f->GetMin();
                double mx = f->GetMax();
                double inc = 0.0;
                try { if (f->HasInc()) inc = f->GetInc(); }
                catch (...) { inc = 0.0; }

                double val = RoundToIncClamp_D(static_cast<double>(v), mn, mx, inc);
                f->SetValue(val);
                return static_cast<float>(f->GetValue()); // trả về dB thực tế
            }
        }

        // ===== 2) Float legacy: GainAbs =====
        {
            CFloatPtr f = nm.GetNode("GainAbs");
            if (f && GenApi::IsWritable(f)) {
                double mn = f->GetMin();
                double mx = f->GetMax();
                double inc = 0.0;
                try { if (f->HasInc()) inc = f->GetInc(); }
                catch (...) { inc = 0.0; }

                double val = RoundToIncClamp_D(static_cast<double>(v), mn, mx, inc);
                f->SetValue(val);
                return static_cast<float>(f->GetValue());
            }
        }

        // ===== 3) Integer: GainRaw (ticks) =====
        {
            CIntegerPtr i = nm.GetNode("GainRaw");
            if (i && GenApi::IsWritable(i)) {
                long long mn = i->GetMin();
                long long mx = i->GetMax();
                long long inc = 0;
                try { inc = i->GetInc(); }
                catch (...) { inc = 0; }
                if (inc <= 0) inc = 1;

                long long desired = static_cast<long long>(std::llround(v));
                long long val = RoundToStepClamp_I(desired, mn, mx, inc);
                i->SetValue(val);

                // Nếu có hệ số đổi raw->dB (tùy hãng), có thể trả về dB; mặc định trả về raw
                // Ví dụ (tùy model): CFloatPtr fac = nm.GetNode("GainRawToDbFactor");
                // if (fac && GenApi::IsReadable(fac)) return static_cast<float>(i->GetValue() * fac->GetValue());
                return static_cast<float>(i->GetValue());
            }
        }

        // ===== 4) Biến thể Basler: BslAnalogGain / BslDigitalGain (float) =====
        {
            CFloatPtr fb = nm.GetNode("BslAnalogGain");
            if (fb && GenApi::IsWritable(fb)) {
                double mn = fb->GetMin(), mx = fb->GetMax();
                double inc = 0.0; try { if (fb->HasInc()) inc = fb->GetInc(); }
                catch (...) { inc = 0.0; }
                double val = RoundToIncClamp_D(v, mn, mx, inc);
                fb->SetValue(val);
                return static_cast<float>(fb->GetValue());
            }
        }
        {
            CFloatPtr fb = nm.GetNode("BslDigitalGain");
            if (fb && GenApi::IsWritable(fb)) {
                double mn = fb->GetMin(), mx = fb->GetMax();
                double inc = 0.0; try { if (fb->HasInc()) inc = fb->GetInc(); }
                catch (...) { inc = 0.0; }
                double val = RoundToIncClamp_D(v, mn, mx, inc);
                fb->SetValue(val);
                return static_cast<float>(fb->GetValue());
            }
        }

        // ===== 5) Biến thể Basler: GainRaw tương tự =====
        {
            CIntegerPtr ib = nm.GetNode("BslGainRaw");
            if (ib && GenApi::IsWritable(ib)) {
                long long mn = ib->GetMin();
                long long mx = ib->GetMax();
                long long inc = 0; try { inc = ib->GetInc(); }
                catch (...) { inc = 0; }
                if (inc <= 0) inc = 1;

                long long desired = static_cast<long long>(std::llround(v));
                long long val = RoundToStepClamp_I(desired, mn, mx, inc);
                ib->SetValue(val);
                return static_cast<float>(ib->GetValue());
            }
        }

        _lastError = "Gain not writable/found";
        return 0.0f;
    }
    catch (const GenICam::GenericException& e) {
        _lastError = gcnew System::String(("SetGain() failed: " + std::string(e.GetDescription())).c_str());
        return 0.0f;
    }
    catch (const std::exception& e) {
        _lastError = gcnew System::String(("SetGain() failed: " + std::string(e.what())).c_str());
        return 0.0f;
    }
    catch (...) {
        _lastError = "SetGain() unknown error";
        return 0.0f;
    }
}

static int64_t RoundToStepClamp(int64_t desired, int64_t vmin, int64_t vmax, int64_t inc)
{
    if (inc <= 0) inc = 1;
    if (desired < vmin) desired = vmin;
    if (desired > vmax) desired = vmax;
    // làm tròn về bội số gần nhất của inc tính từ vmin
    const int64_t n = (desired - vmin + inc / 2) / inc;
    int64_t val = vmin + n * inc;
    if (val < vmin) val = vmin;
    if (val > vmax) val = vmax;
    return val;
}

float Camera::SetDigitalShift(float value)
{
    _lastError = System::String::Empty;
    try {
        INodeMap& nm = _cam->GetNodeMap();

        // 1) Integer: DigitalShift
        {
            CIntegerPtr ishift = nm.GetNode("DigitalShift");
            if (ishift && GenApi::IsWritable(ishift)) {
                int64_t vmin = ishift->GetMin();
                int64_t vmax = ishift->GetMax();
                int64_t vinc = 0;
                try { vinc = ishift->GetInc(); }
                catch (...) { vinc = 0; }
                if (vinc <= 0) vinc = 1;

                int64_t desired = static_cast<int64_t>(llround(value));
                int64_t val = RoundToStepClamp(desired, vmin, vmax, vinc);
                ishift->SetValue(val);

                // đọc lại để biết chắc giá trị thực tế
                int64_t applied = ishift->GetValue();
                return static_cast<float>(applied);
            }
        }

        // 2) Integer: BslDigitalShift (biến thể Basler)
        {
            CIntegerPtr bsl = nm.GetNode("BslDigitalShift");
            if (bsl && GenApi::IsWritable(bsl)) {
                int64_t vmin = bsl->GetMin();
                int64_t vmax = bsl->GetMax();
                int64_t vinc = 0;
                try { vinc = bsl->GetInc(); }
                catch (...) { vinc = 0; }
                if (vinc <= 0) vinc = 1;

                int64_t desired = static_cast<int64_t>(llround(value));
                int64_t val = RoundToStepClamp(desired, vmin, vmax, vinc);
                bsl->SetValue(val);

                int64_t applied = bsl->GetValue();
                return static_cast<float>(applied);
            }
        }

        // 3) Enumeration: DigitalShift (ít gặp)
        {
            CEnumerationPtr eshift = nm.GetNode("DigitalShift");
            if (eshift && GenApi::IsWritable(eshift)) {
                int64_t desired = static_cast<int64_t>(llround(value));

                // thử set trực tiếp theo int (nhiều model map trực tiếp)
                try {
                    eshift->SetIntValue(desired);
                    int64_t applied = eshift->GetIntValue();
                    return static_cast<float>(applied);
                }
                catch (...) {
                    // fallback: tìm entry khả dụng gần nhất
                    GenApi::NodeList_t entries;
                    eshift->GetEntries(entries);
                    bool any = false;
                    CEnumEntryPtr best;
                    int64_t bestDiff = 0;
                    for (size_t k = 0; k < entries.size(); ++k) {
                        CEnumEntryPtr ee = entries[k];
                        if (ee && GenApi::IsAvailable(ee)) {
                            int64_t v = ee->GetValue();
                            int64_t d = llabs(v - desired);
                            if (!any || d < bestDiff) { best = ee; bestDiff = d; any = true; }
                        }
                    }
                    if (any && best) {
                        eshift->SetIntValue(best->GetValue());
                        int64_t applied = eshift->GetIntValue();
                        return static_cast<float>(applied);
                    }
                }
            }
        }

        _lastError = "DigitalShift not writable/found";
        return std::numeric_limits<float>::quiet_NaN();
    }
    catch (const GenICam::GenericException& e) {
        _lastError = gcnew System::String(("SetDigitalShift() failed: " + std::string(e.GetDescription())).c_str());
        return std::numeric_limits<float>::quiet_NaN();
    }
    catch (const std::exception& e) {
        _lastError = gcnew System::String(("SetDigitalShift() failed: " + std::string(e.what())).c_str());
        return std::numeric_limits<float>::quiet_NaN();
    }
    catch (...) {
        _lastError = "SetDigitalShift() unknown error";
        return std::numeric_limits<float>::quiet_NaN();
    }
}
void Camera::GetDigitalShift(float% min, float% max, float% step, float% current)
{
    min = max = step = current = 0.0f;
    _lastError = System::String::Empty;

    try {
        INodeMap& nm = _cam->GetNodeMap();

        // --- 1) Integer: DigitalShift ---
        CIntegerPtr ishift = nm.GetNode("DigitalShift");
        if (ishift && GenApi::IsReadable(ishift)) {
            const int64_t vmin = ishift->GetMin();
            const int64_t vmax = ishift->GetMax();
            const int64_t vcur = ishift->GetValue();
            int64_t vinc = 0;
            try { vinc = ishift->GetInc(); }
            catch (...) { vinc = 0; }
            if (vinc <= 0) vinc = 1;

            min = static_cast<float>(vmin);
            max = static_cast<float>(vmax);
            step = static_cast<float>(vinc);   // integer → bước rời rạc (thường = 1)
            current = static_cast<float>(vcur);
            return;
        }

        // --- 2) Integer: BslDigitalShift (đặt riêng của Basler) ---
        CIntegerPtr bsl = nm.GetNode("BslDigitalShift");
        if (bsl && GenApi::IsReadable(bsl)) {
            const int64_t vmin = bsl->GetMin();
            const int64_t vmax = bsl->GetMax();
            const int64_t vcur = bsl->GetValue();
            int64_t vinc = 0;
            try { vinc = bsl->GetInc(); }
            catch (...) { vinc = 0; }
            if (vinc <= 0) vinc = 1;

            min = static_cast<float>(vmin);
            max = static_cast<float>(vmax);
            step = static_cast<float>(vinc);
            current = static_cast<float>(vcur);
            return;
        }

        // --- 3) Enumeration: DigitalShift (ít gặp, dự phòng) ---
        CEnumerationPtr eshift = nm.GetNode("DigitalShift");
        if (eshift && GenApi::IsReadable(eshift)) {
            // lấy current
            int64_t vcur = 0;
            try { vcur = eshift->GetIntValue(); }
            catch (...) { vcur = 0; }

            // quét entries để suy ra min/max
            GenApi::NodeList_t entries;
            eshift->GetEntries(entries);
            bool any = false;
            int64_t vmin = 0, vmax = 0;
            for (size_t k = 0; k < entries.size(); ++k) {
                CEnumEntryPtr ee = entries[k];
                if (ee && GenApi::IsAvailable(ee)) {
                    const int64_t val = ee->GetValue();
                    if (!any) { vmin = vmax = val; any = true; }
                    else {
                        if (val < vmin) vmin = val;
                        if (val > vmax) vmax = val;
                    }
                }
            }
            if (any) {
                min = static_cast<float>(vmin);
                max = static_cast<float>(vmax);
                step = 1.0f;                 // enum → xem như bước 1
                current = static_cast<float>(vcur);
                return;
            }
        }

        _lastError = "DigitalShift node not found/readable";
    }
    catch (const GenICam::GenericException& e) {
        _lastError = gcnew System::String(("GetDigitalShift() failed: " + std::string(e.GetDescription())).c_str());
        min = max = step = current = 0.0f;
    }
    catch (const std::exception& e) {
        _lastError = gcnew System::String(("GetDigitalShift() failed: " + std::string(e.what())).c_str());
        min = max = step = current = 0.0f;
    }
    catch (...) {
        _lastError = "GetDigitalShift() unknown error";
        min = max = step = current = 0.0f;
    }
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
    min = max = step = current = 0.0f;
    _lastError = "";

    try {
        INodeMap& nm = _cam->GetNodeMap();

        // 1) Tắt auto (nếu có) để đọc/ghi ổn định
        if (CEnumerationPtr expAuto = nm.GetNode("ExposureAuto")) {
            if (IsWritable(expAuto)) expAuto->FromString("Off");
        }

        // 2) Ưu tiên node float chuẩn: ExposureTime (µs)
        CFloatPtr f = nm.GetNode("ExposureTime");
        if (f && IsReadable(f)) {
            min = static_cast<float>(f->GetMin());
            max = static_cast<float>(f->GetMax());
            current = static_cast<float>(f->GetValue());
            step = (f->HasInc() ? static_cast<float>(f->GetInc()) : 0.0f); // 0.0 = liên tục
            return ;
        }
        f = nm.GetNode("ExposureTimeAbs");
        if (f && IsReadable(f)) {
            min = static_cast<float>(f->GetMin());
            max = static_cast<float>(f->GetMax());
            current = static_cast<float>(f->GetValue());
            step = (f->HasInc() ? static_cast<float>(f->GetInc()) : 0.0f);
            return ;
        }

        f = nm.GetNode("ExposureTimeRaw");
        if (f && IsReadable(f)) {
            min = static_cast<float>(f->GetMin());
            max = static_cast<float>(f->GetMax());
            current = static_cast<float>(f->GetValue());
            step = static_cast<float>(f->GetInc()); // integer luôn có inc
            _lastError = "Using ExposureTimeRaw (ticks), convert to µs if needed";
            return ;
        }

        _lastError = "ExposureTime node not found/readable";
        return ;
    }
    catch (const GenICam::GenericException& e) {
        _lastError = gcnew System::String(e.GetDescription()) ;
        return ;
    }
}
void Camera::GetGain(float% min, float% max, float% step, float% current)
{
    // reset output
    min = max = step = current = 0.0f;
    _lastError = System::String::Empty;

    try {
        INodeMap& nm = _cam->GetNodeMap();

        // (tuỳ chọn) tắt auto để đọc/ghi ổn định
        CEnumerationPtr gainAuto = nm.GetNode("GainAuto");
        if (gainAuto && IsWritable(gainAuto)) {
            try { gainAuto->FromString("Off"); }
            catch (...) {}
        }

        CEnumerationPtr sel = nm.GetNode("GainSelector");
        if (sel && GenApi::IsWritable(sel)) {
            // Cách 1: thử bằng tên "All"
            CEnumEntryPtr all = sel->GetEntryByName("All");
            if (all && GenApi::IsAvailable(all)) {
                sel->SetIntValue(all->GetValue());
            }
            else {
                // Cách 2: duyệt entries, chọn entry khả dụng đầu tiên
                GenApi::NodeList_t entries;
                sel->GetEntries(entries);
                for (size_t k = 0; k < entries.size(); ++k) {
                    CEnumEntryPtr ee = entries[k];
                    if (ee && GenApi::IsAvailable(ee)) {
                        sel->SetIntValue(ee->GetValue());
                        break;
                    }
                }
            }
        }

        // 1) Ưu tiên node float chuẩn: Gain (thường là dB)
        CFloatPtr f = nm.GetNode("Gain");
        if (f && IsReadable(f)) {
            min = static_cast<float>(f->GetMin());
            max = static_cast<float>(f->GetMax());
            current = static_cast<float>(f->GetValue());
            step = f->HasInc() ? static_cast<float>(f->GetInc()) : 0.0f; // 0.0 = continuous
            return;
        }

        // 2) Fallback legacy: GainAbs (một số thiết bị dùng tên này)
        CFloatPtr fabs = nm.GetNode("GainAbs");
        if (fabs && IsReadable(fabs)) {
            min = static_cast<float>(fabs->GetMin());
            max = static_cast<float>(fabs->GetMax());
            current = static_cast<float>(fabs->GetValue());
            step = fabs->HasInc() ? static_cast<float>(fabs->GetInc()) : 0.0f;
            return;
        }

        // 3) Fallback integer: GainRaw (đơn vị "ticks")
        CIntegerPtr i = nm.GetNode("GainRaw");
        if (i && IsReadable(i)) {
            min = static_cast<float>(i->GetMin());
            max = static_cast<float>(i->GetMax());
            current = static_cast<float>(i->GetValue());

            int64_t incRaw = 0;
            try { incRaw = i->GetInc(); }
            catch (...) { incRaw = 0; }
            if (incRaw <= 0) incRaw = 1;      // integer nên đảm bảo >= 1
            step = static_cast<float>(incRaw);

            _lastError = "Using GainRaw (integer steps)";
            return;
        }

        // 4) (tuỳ hãng) AnalogGain / DigitalGain
        CFloatPtr analog = nm.GetNode("AnalogGain");
        if (analog && IsReadable(analog)) {
            min = static_cast<float>(analog->GetMin());
            max = static_cast<float>(analog->GetMax());
            current = static_cast<float>(analog->GetValue());
            step = analog->HasInc() ? static_cast<float>(analog->GetInc()) : 0.0f;
            return;
        }

        _lastError = "Gain node not found/readable";
    }
    catch (const GenICam::GenericException& e) {
        _lastError = gcnew System::String(("GetGain() failed: " + std::string(e.GetDescription())).c_str());
        min = max = step = current = 0.0f;
    }
    catch (const std::exception& e) {
        _lastError = gcnew System::String(("GetGain() failed: " + std::string(e.what())).c_str());
        min = max = step = current = 0.0f;
    }
    catch (...) {
        _lastError = "GetGain() unknown error";
        min = max = step = current = 0.0f;
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
        if (!n || !IsReadable(n))  _lastError = "Height node not readable";

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
    // reset output
    min = max = step = current = 0.0f;
    _lastError = System::String::Empty;

    try {
        INodeMap& nm = _cam->GetNodeMap();

        // 0) (tuỳ chọn) tắt auto nếu có
        CEnumerationPtr blAuto = nm.GetNode("BlackLevelAuto");
        if (blAuto && GenApi::IsWritable(blAuto)) {
            try { blAuto->FromString("Off"); }
            catch (...) {}
        }

        // 1) (tuỳ chọn) chọn selector = All nếu có
        CEnumerationPtr sel = nm.GetNode("BlackLevelSelector");
        if (sel && GenApi::IsWritable(sel)) {
            // ưu tiên entry "All" nếu có
            CEnumEntryPtr all = sel->GetEntryByName("All");
            if (all && GenApi::IsAvailable(all)) {
                try { sel->SetIntValue(all->GetValue()); }
                catch (...) {}
            }
            // nếu không có "All", để nguyên selector hiện tại
        }

        // 2) Ưu tiên node float chuẩn: BlackLevel (đơn vị DN)
        {
            CFloatPtr f = nm.GetNode("BlackLevel");
            if (f && GenApi::IsReadable(f)) {
                min = static_cast<float>(f->GetMin());
                max = static_cast<float>(f->GetMax());
                current = static_cast<float>(f->GetValue());
                step = f->HasInc() ? static_cast<float>(f->GetInc()) : 0.0f; // 0.0 = continuous
                return;
            }
        }

        // 3) Fallback integer: BlackLevelRaw (đếm thô)
        {
            CIntegerPtr iraw = nm.GetNode("BlackLevelRaw");
            if (iraw && GenApi::IsReadable(iraw)) {
                const int64_t vmin = iraw->GetMin();
                const int64_t vmax = iraw->GetMax();
                const int64_t vcur = iraw->GetValue();
                int64_t vinc = 0;
                try { vinc = iraw->GetInc(); }
                catch (...) { vinc = 0; }
                if (vinc <= 0) vinc = 1;

                min = static_cast<float>(vmin);
                max = static_cast<float>(vmax);
                step = static_cast<float>(vinc); // integer → bước rời rạc
                current = static_cast<float>(vcur);
                return;
            }
        }

        // 4) Biến thể Basler: BslBlackLevel (float)
        {
            CFloatPtr fb = nm.GetNode("BslBlackLevel");
            if (fb && GenApi::IsReadable(fb)) {
                min = static_cast<float>(fb->GetMin());
                max = static_cast<float>(fb->GetMax());
                current = static_cast<float>(fb->GetValue());
                step = fb->HasInc() ? static_cast<float>(fb->GetInc()) : 0.0f;
                return;
            }
        }

        // 5) Biến thể Basler: BslBlackLevelRaw (integer)
        {
            CIntegerPtr ibr = nm.GetNode("BslBlackLevelRaw");
            if (ibr && GenApi::IsReadable(ibr)) {
                const int64_t vmin = ibr->GetMin();
                const int64_t vmax = ibr->GetMax();
                const int64_t vcur = ibr->GetValue();
                int64_t vinc = 0;
                try { vinc = ibr->GetInc(); }
                catch (...) { vinc = 0; }
                if (vinc <= 0) vinc = 1;

                min = static_cast<float>(vmin);
                max = static_cast<float>(vmax);
                step = static_cast<float>(vinc);
                current = static_cast<float>(vcur);
                return;
            }
        }

        _lastError = "BlackLevel node not found/readable";
        // outputs đã là 0; return không throw
    }
    catch (const GenICam::GenericException& e) {
        _lastError = gcnew System::String(("GetBlackLevel() failed: " + std::string(e.GetDescription())).c_str());
        min = max = step = current = 0.0f;
    }
    catch (const std::exception& e) {
        _lastError = gcnew System::String(("GetBlackLevel() failed: " + std::string(e.what())).c_str());
        min = max = step = current = 0.0f;
    }
    catch (...) {
        _lastError = "GetBlackLevel() unknown error";
        min = max = step = current = 0.0f;
    }
}
void Camera::GetOffsetX(float% min, float% max, float% step, float% current)
{
    try {
        CIntegerPtr n = _cam->GetNodeMap().GetNode("OffsetX");
        if (!n || !IsReadable(n))  _lastError = "OffsetX node not readable";

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
        if (!n || !IsReadable(n)) _lastError = "OffsetY node not readable";

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
        int next = (_bufIndex == 0) ? 1 : 0;
        CPylonImage* target = (next == 0) ? _bufA : _bufB;
        _conv->Convert(*target, ptr);
        _bufIndex = next;  // Atomic nếu cần thread-safe tuyệt đối
        _frameCount++;
        auto now = std::chrono::steady_clock::now();
                std::chrono::duration<double> elapsed = now - _lastFrameTime;
                if (elapsed.count() >= 1.0) {
                    _emaFps = _frameCount / elapsed.count();
                    _frameCount = 0;
                    _lastFrameTime = now;
                }
    }
    catch (...) {
        _lastError = "ProcessGrabbed error";
    }
   
   
    //_latestImageMutex->WaitOne();
    //try {
    //    CPylonImage* dst = NextBuffer();
    //    _conv->Convert(*dst, ptr);

    //    int w = (int)dst->GetWidth();
    //    int h = (int)dst->GetHeight();
    //    int ch = _activeChannels;
    //    int stride = w * ch; // packed Mono8/BGR8

    //    if (_frameReadyHandlers != nullptr) {
    //        _frameCount++;
    //        System::IntPtr buffer((unsigned char*)dst->GetBuffer()); // uchar*
    //        FrameReady(buffer, w, h, stride, ch);
    //        auto now = std::chrono::steady_clock::now();
    //        std::chrono::duration<double> elapsed = now - _lastFrameTime;
    //        if (elapsed.count() >= 1.0) {
    //            _emaFps = _frameCount / elapsed.count();
    //            _frameCount = 0;
    //            _lastFrameTime = now;
    //        }
    //    }
    //    _lastError = nullptr;
    //}
    //catch (...) { _lastError = "ProcessGrabbed error"; }
}

// ===== UserLoop API (uchar* qua IntPtr) =====
System::IntPtr Camera::GrabOneUcharPtr(int timeoutMs, int% w, int% h, int% stride, int% channels)
{
    _frameCount++;
    w = h = stride = channels = 0;
    _lastError = System::String::Empty;

    if (!_cam || !_cam->IsGrabbing()) { _lastError = "Not grabbing"; return System::IntPtr::Zero; }
    if (_mode != GrabMode::UserLoop) { _lastError = "GrabOneUcharPtr only in UserLoop"; return System::IntPtr::Zero; }

    std::lock_guard<std::mutex> lock(g_grabMutex);

    try {
        const auto t_start = std::chrono::steady_clock::now();
        CGrabResultPtr res;

        // Vòng lặp “chờ trong ngân sách timeout”
        while (true) {
            const auto now = std::chrono::steady_clock::now();
            const int waited = (int)std::chrono::duration_cast<std::chrono::milliseconds>(now - t_start).count();
            if (waited >= timeoutMs) { _lastError = "Timeout"; return System::IntPtr::Zero; }

            // dùng phần timeout còn lại cho lần gọi này
            const int remain = timeoutMs - waited;
            if (!_cam->RetrieveResult(remain, res, TimeoutHandling_Return)) {
                // lần này chưa có frame → loop tiếp cho đến hết timeout
                continue;
            }

            if (!res || !res->GrabSucceeded()) {
                // Có kết quả nhưng lỗi – ghi mã lỗi rồi loop tiếp cho đến hết timeout
                if (res) {
                    auto code = res->GetErrorCode();
                    GenICam::gcstring gdesc = res->GetErrorDescription();
                    _lastError = System::String::Format("Grab failed: {0} - {1}",
                        (int)code,
                        gcnew System::String(gdesc.c_str()));
                   
                }
                else {
                    _lastError = "Grab failed: null result";
                }
                continue;
            }

            CPylonImage* dst = NextBuffer();
            if (!dst) { _lastError = "NextBuffer() returned null"; return System::IntPtr::Zero; }

            // Convert theo cấu hình OutputPixelFormat/OutputPaddingX đã set
            _conv->Convert(*dst, res);
            if (!dst->IsValid() || dst->GetBuffer() == nullptr) {
                _lastError = "Convert() produced empty buffer";
                return System::IntPtr::Zero;
            }

            const size_t ww = dst->GetWidth();
            const size_t hh = dst->GetHeight();

            // Vì OutputPaddingX=0 nên stride = w * channels
            w = static_cast<int>(ww);
            h = static_cast<int>(hh);
            channels = _activeChannels;
            stride = w * channels;

            // --- Update FPS ---
            auto now2 = std::chrono::steady_clock::now();
            std::chrono::duration<double> elapsed = now2 - _lastFrameTime;
            if (elapsed.count() >= 1.0) {
                _emaFps = _frameCount / elapsed.count();
                _frameCount = 0;
                _lastFrameTime = now2;
            }

            _lastError = System::String::Empty;
            return System::IntPtr((void*)dst->GetBuffer()); // buffer nội bộ
        }
    }
    catch (const GenericException& e) { _lastError = gcnew System::String(e.GetDescription()); }
    catch (const std::exception& e) { _lastError = gcnew System::String(e.what()); }
    catch (...) { _lastError = "GrabOneUcharPtr unknown error"; }

    return System::IntPtr::Zero;
}System::IntPtr Camera::CopyLatestImage(int% w, int% h, int% stride, int% channels)
{
    w = h = stride = channels = 0;
    CPylonImage* src = (_bufIndex == 0) ? _bufB : _bufA;
    if (!src || !src->IsValid()) {
        _lastError = "No valid image";
        return System::IntPtr::Zero;
    }

    size_t imgSize = src->GetImageSize();
    if (imgSize == 0 || !src->GetBuffer()) {
        _lastError = "Invalid buffer";
        return System::IntPtr::Zero;
    }

    // === CHỈNH ĐÚNG NGUYÊN TẮC: cấp phát vùng nhớ mới ===
    unsigned char* clone = new unsigned char[imgSize];
    memcpy(clone, src->GetBuffer(), imgSize);

    w = (int)src->GetWidth();
    h = (int)src->GetHeight();
    channels = _activeChannels;
    stride = w * channels;

    return System::IntPtr(clone);
}
void Camera::FreeImage(System::IntPtr p)
{
    if (p != System::IntPtr::Zero)
    {
        unsigned char* buf = (unsigned char*)p.ToPointer();
        delete[] buf;
    }
}

//System::IntPtr Camera::CopyLatestImage(int% w, int% h, int% stride, int% channels)
//{
//    w = h = stride = channels = 0;
//    CPylonImage* src = (_bufIndex == 0) ? _bufB : _bufA; // lấy ảnh vừa hoàn tất
//    if (!src || !src->IsValid()) {
//        _lastError = "No valid image";
//        return System::IntPtr::Zero;
//    }
//
//    size_t imgSize = src->GetImageSize();
//    unsigned char* clone = new unsigned char[imgSize];
//    memcpy(clone, src->GetBuffer(), imgSize);
//
//    w = (int)src->GetWidth();
//    h = (int)src->GetHeight();
//    channels = _activeChannels;
//    stride = w * channels;
//
//    return System::IntPtr(clone);
//    //_latestImageMutex->WaitOne();
//    //if (!_latestImage || !_latestImage->IsValid()) {
//    //    _lastError = "No latest image";
//    //    return System::IntPtr::Zero;
//    //}
//
//    //try {
//    //    size_t imgSize = _latestImage->GetImageSize();
//    //    if (imgSize == 0 || !_latestImage->GetBuffer()) {
//    //        _lastError = "Empty or null image buffer";
//    //        return System::IntPtr::Zero;
//    //    }
//
//    //    // Allocate memory for copy
//    //    unsigned char* clone = new unsigned char[imgSize];
//    //    memcpy(clone, _latestImage->GetBuffer(), imgSize);
//
//    //    w = (int)_latestImage->GetWidth();
//    //    h = (int)_latestImage->GetHeight();
//    //    channels = _activeChannels;
//    //    stride = w * channels;
//
//    //    _lastError = nullptr;
//    //    return System::IntPtr(clone); // Caller must manage memory if needed
//    //}
//    //catch (...) {
//    //    _lastError = "CopyLatestImage failed";
//    //    return System::IntPtr::Zero;
//    //}
//    //finally {
//    //    _latestImageMutex->ReleaseMutex();
//    //}
//}

//System::IntPtr Camera::GrabOneUcharPtr(int timeoutMs, int% w, int% h, int% stride, int% channels) {
//  //  auto t0 = std::chrono::steady_clock::now();
//    _frameCount++;
//    w = h = stride = channels = 0;
//    if (!_cam || !_cam->IsGrabbing()) { _lastError = "Not grabbing"; return System::IntPtr::Zero; }
//    if (_mode != GrabMode::UserLoop) { _lastError = "GrabOneUcharPtr only in UserLoop"; return System::IntPtr::Zero; }
//
//    std::lock_guard<std::mutex> lock(g_grabMutex);
//    try {
//        CGrabResultPtr res;
//        if (_cam->RetrieveResult(timeoutMs, res, TimeoutHandling_Return)) {
//            if (res && res->GrabSucceeded()) {
//                CPylonImage* dst = NextBuffer();
//                _conv->Convert(*dst, res);
//
//                w = (int)dst->GetWidth();
//                h = (int)dst->GetHeight();
//                channels = _activeChannels;
//                stride = w * channels; // packed
//
//                _lastError = nullptr;
//                // --- Update FPS ---
//                // Cập nhật FPS mỗi ~1s
//                auto now = std::chrono::steady_clock::now();
//                std::chrono::duration<double> elapsed = now - _lastFrameTime;
//                if (elapsed.count() >= 1.0) {
//                    _emaFps = _frameCount / elapsed.count();
//                    _frameCount = 0;
//                    _lastFrameTime = now;
//                }
//               
//                return System::IntPtr((unsigned char*)dst->GetBuffer()); // uchar*
//            }
//            else {
//                _lastError = "Grab failed";
//            }
//        }
//        else {
//            _lastError = "Timeout";
//        }
//    }
//    catch (const GenericException& e) { _lastError = gcnew System::String(e.GetDescription()); }
//    catch (const std::exception& e) { _lastError = gcnew System::String(e.what()); }
//    catch (...) { _lastError = "GrabOneUcharPtr unknown error"; }
//
//    return System::IntPtr::Zero;
//}

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