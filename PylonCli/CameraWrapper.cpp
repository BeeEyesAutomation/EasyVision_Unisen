#include "CameraWrapper.h"
#include <msclr/marshal_cppstd.h>
#include <GenApi/GenApi.h>

using namespace PylonCli;
using namespace Pylon;
using namespace GenApi;
using msclr::interop::marshal_as;

// ---- Helpers ----
void Camera::SetIntNode(INodeMap& nm, const char* name, long long v) {
    CIntegerPtr n = nm.GetNode(name);
    if (!n || !IsWritable(n)) throw std::runtime_error("Node not writable");
    long long mn = n->GetMin(), mx = n->GetMax(), inc = n->GetInc();
    if (v < mn) v = mn;
    if (v > mx) v = mx;
    if (inc > 1) v = mn + ((v - mn) / inc) * inc;
    n->SetValue(v);
}
long long Camera::GetIntNode(INodeMap& nm, const char* name) {
    CIntegerPtr n = nm.GetNode(name);
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
double Camera::GetFloatNode(INodeMap& nm, const char* name) {
    CFloatPtr n = nm.GetNode(name);
    if (!n || !IsReadable(n)) throw std::runtime_error("Node not readable");
    return n->GetValue();
}
double Camera::GetFloatAny(INodeMap& nm, std::initializer_list<const char*> names) {
    for (auto s : names) { CFloatPtr n = nm.GetNode(s); if (n && IsReadable(n)) return n->GetValue(); }
    throw std::runtime_error("No float node");
}
void Camera::SetFloatAny(INodeMap& nm, std::initializer_list<const char*> names, double v) {
    for (auto s : names) {
        CFloatPtr n = nm.GetNode(s); if (n && IsWritable(n)) {
            double mn = n->GetMin(), mx = n->GetMax(); if (v < mn)v = mn; if (v > mx)v = mx; n->SetValue(v); return;
        }
    }
    throw std::runtime_error("No float node writable");
}
void Camera::TrySetEnum(INodeMap& nm, const char* enumName, const char* entry) {
    CEnumerationPtr e = nm.GetNode(enumName);
    if (!e || !IsWritable(e)) return;
    CEnumEntryPtr en = e->GetEntryByName(entry);
    if (en && IsReadable(en)) e->SetIntValue(en->GetValue());
}

// ---- Output pixel ----
bool Camera::DetectIsColorSensor() {
    try {
        CEnumerationPtr cf = _cam->GetNodeMap().GetNode("ColorFilter");
        if (cf && IsReadable(cf)) return true;
        CEnumerationPtr pf = _cam->GetNodeMap().GetNode("PixelFormat");
        if (pf && IsReadable(pf)) {
            CEnumEntryPtr cur = pf->GetCurrentEntry();
            if (cur && IsReadable(cur)) {
                std::string sym = cur->GetSymbolic().c_str();

                if (sym.find("Bayer") != std::string::npos || sym.find("RGB") != std::string::npos) return true;
            }
        }
    }
    catch (...) {}
    return false;
}
void Camera::ConfigureConverterForOutput() {
    if (!_conv) return;
    OutputPixel want = _desiredOutput;
    if (want == OutputPixel::Auto) {
        want = DetectIsColorSensor() ? OutputPixel::BGR8 : OutputPixel::Mono8;
    }
    if (want == OutputPixel::Mono8) {
        _conv->OutputPixelFormat = PixelType_Mono8; _activeChannels = 1;
    }
    else {
        _conv->OutputPixelFormat = PixelType_BGR8packed; _activeChannels = 3;
    }
    _conv->OutputBitAlignment = OutputBitAlignment_MsbAligned;
}
void Camera::SetOutputPixel(OutputPixel fmt) { _desiredOutput = fmt; if (_conv) ConfigureConverterForOutput(); }
OutputPixel Camera::GetOutputPixel() { return (_activeChannels == 1) ? OutputPixel::Mono8 : OutputPixel::BGR8; }

// ---- Class ----
Camera::Camera() {}
Camera::~Camera() { this->!Camera(); }
Camera::!Camera() { try { Stop(); Close(); if (_conv) { delete _conv; _conv = nullptr; } } catch (...) {} }

cli::array<String^>^ Camera::List() {
    if (!s_pylonInited) { PylonInitialize(); s_pylonInited = true; }
    CTlFactory& tl = CTlFactory::GetInstance(); DeviceInfoList_t devs; tl.EnumerateDevices(devs);
    auto out = gcnew cli::array<String^>((int)devs.size());
    for (int i = 0; i < (int)devs.size(); ++i) {
        const char* u = devs[i].GetUserDefinedName();
        if (u && *u) out[i] = gcnew String(u);
        else {
            std::string s = std::string(devs[i].GetModelName()) + "_" + std::string(devs[i].GetSerialNumber());
            out[i] = gcnew String(s.c_str());
        }
    }
    return out;
}

void Camera::Open(System::String^ name) {
    if (!s_pylonInited) { PylonInitialize(); s_pylonInited = true; }
    if (_opened) { _lastError = "Already open"; return; }
    std::string want = marshal_as<std::string>(name);
    CTlFactory& tl = CTlFactory::GetInstance(); DeviceInfoList_t devs; tl.EnumerateDevices(devs);
    if (devs.empty()) { _lastError = "No cameras"; return; }
    bool found = false; CDeviceInfo picked;
    for (auto& d : devs) {
        std::string byUser = (d.GetUserDefinedName() && *d.GetUserDefinedName()) ? std::string(d.GetUserDefinedName()) : "";
        std::string byModel = std::string(d.GetModelName()) + "_" + std::string(d.GetSerialNumber());
        if (byUser == want || byModel == want) { picked = d; found = true; break; }
    }
    if (!found) { _lastError = "Not found"; return; }
    _cam = new CInstantCamera(tl.CreateDevice(picked)); _cam->Open();
    _conv = new CImageFormatConverter(); ConfigureConverterForOutput();
    _opened = true; _lastError = nullptr;
}
void Camera::Close() { if (_cam) { if (_cam->IsGrabbing())_cam->StopGrabbing(); if (_cam->IsOpen())_cam->Close(); delete _cam; _cam = nullptr; } _opened = false; }
void Camera::Start() { if (!_cam) { _lastError = "Not open"; return; } if (!_cam->IsGrabbing()) _cam->StartGrabbing(GrabStrategy_LatestImageOnly, GrabLoop_ProvidedByInstantCamera); ConfigureConverterForOutput(); _lastError = nullptr; }
void Camera::Stop() { if (_cam && _cam->IsGrabbing()) _cam->StopGrabbing(); }
bool Camera::IsOpen() { return _opened && _cam && _cam->IsOpen(); }

// ---- ROI ----
int Camera::GetWidth() { try { return (int)GetIntNode(_cam->GetNodeMap(), "Width"); } catch (...) { _lastError = "GetWidth fail"; return 0; } }
void Camera::SetWidth(int v) { try { SetIntNode(_cam->GetNodeMap(), "Width", v); _lastError = nullptr; } catch (...) { _lastError = "SetWidth fail"; } }
int Camera::GetHeight() { try { return (int)GetIntNode(_cam->GetNodeMap(), "Height"); } catch (...) { _lastError = "GetHeight fail"; return 0; } }
void Camera::SetHeight(int v) { try { SetIntNode(_cam->GetNodeMap(), "Height", v); _lastError = nullptr; } catch (...) { _lastError = "SetHeight fail"; } }
int Camera::GetOffsetX() { try { return (int)GetIntNode(_cam->GetNodeMap(), "OffsetX"); } catch (...) { _lastError = "GetOffsetX fail"; return 0; } }
void Camera::SetOffsetX(int v) { try { SetIntNode(_cam->GetNodeMap(), "OffsetX", v); _lastError = nullptr; } catch (...) { _lastError = "SetOffsetX fail"; } }
int Camera::GetOffsetY() { try { return (int)GetIntNode(_cam->GetNodeMap(), "OffsetY"); } catch (...) { _lastError = "GetOffsetY fail"; return 0; } }
void Camera::SetOffsetY(int v) { try { SetIntNode(_cam->GetNodeMap(), "OffsetY", v); _lastError = nullptr; } catch (...) { _lastError = "SetOffsetY fail"; } }

int Camera::GetCenterX() { return GetOffsetX() + GetWidth() / 2; }
void Camera::SetCenterX(int cx) { SetOffsetX(cx - GetWidth() / 2); }
int Camera::GetCenterY() { return GetOffsetY() + GetHeight() / 2; }
void Camera::SetCenterY(int cy) { SetOffsetY(cy - GetHeight() / 2); }
void Camera::CenterX() { try { long long sw = GetIntNode(_cam->GetNodeMap(), "SensorWidth"); int w = GetWidth(); SetOffsetX((int)((sw - w) / 2)); } catch (...) { _lastError = "CenterX fail"; } }
void Camera::CenterY() { try { long long sh = GetIntNode(_cam->GetNodeMap(), "SensorHeight"); int h = GetHeight(); SetOffsetY((int)((sh - h) / 2)); } catch (...) { _lastError = "CenterY fail"; } }

// ---- Params ----
double Camera::GetExposure() { try { return GetFloatAny(_cam->GetNodeMap(), { "ExposureTime","ExposureTimeAbs" }); } catch (...) { _lastError = "GetExposure fail"; return 0; } }
void Camera::SetExposure(double v) { try { TrySetEnum(_cam->GetNodeMap(), "ExposureAuto", "Off"); SetFloatAny(_cam->GetNodeMap(), { "ExposureTime","ExposureTimeAbs" }, v); _lastError = nullptr; } catch (...) { _lastError = "SetExposure fail"; } }
double Camera::GetGain() { try { TrySetEnum(_cam->GetNodeMap(), "GainSelector", "All"); return GetFloatNode(_cam->GetNodeMap(), "Gain"); } catch (...) { _lastError = "GetGain fail"; return 0; } }
void Camera::SetGain(double v) { try { TrySetEnum(_cam->GetNodeMap(), "GainAuto", "Off"); TrySetEnum(_cam->GetNodeMap(), "GainSelector", "All"); SetFloatNode(_cam->GetNodeMap(), "Gain", v); _lastError = nullptr; } catch (...) { _lastError = "SetGain fail"; } }
double Camera::GetBlackLevel() { try { TrySetEnum(_cam->GetNodeMap(), "BlackLevelSelector", "All"); return GetFloatNode(_cam->GetNodeMap(), "BlackLevel"); } catch (...) { try { return GetFloatNode(_cam->GetNodeMap(), "BlackLevelRaw"); } catch (...) { _lastError = "GetBlackLevel fail"; return 0; } } }
void Camera::SetBlackLevel(double v) { try { TrySetEnum(_cam->GetNodeMap(), "BlackLevelSelector", "All"); SetFloatNode(_cam->GetNodeMap(), "BlackLevel", v); _lastError = nullptr; } catch (...) { try { SetFloatNode(_cam->GetNodeMap(), "BlackLevelRaw", v); } catch (...) { _lastError = "SetBlackLevel fail"; } } }

// ---- Grab ----
System::IntPtr Camera::GrabFramePtrEx([Out] int% w, [Out] int% h, [Out] int% stride, [Out] int% channels) {
    if (!_cam || !_cam->IsGrabbing()) { _lastError = "Not grabbing"; w = h = stride = channels = 0; return System::IntPtr::Zero; }
    try {
        CGrabResultPtr res;
        if (_cam->RetrieveResult(500, res, TimeoutHandling_Return) && res && res->GrabSucceeded()) {
            static CPylonImage img;
            _conv->Convert(img, res);
            w = (int)img.GetWidth(); h = (int)img.GetHeight();
            channels = _activeChannels; stride = w * channels;
            _lastError = nullptr;
            return System::IntPtr(const_cast<void*>(img.GetBuffer()));
        }
    }
    catch (const std::exception& e) { _lastError = gcnew System::String(e.what()); }
    w = h = stride = channels = 0; return System::IntPtr::Zero;
}
