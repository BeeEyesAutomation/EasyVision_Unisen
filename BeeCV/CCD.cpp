#include "CCD.h"
#include <iostream>
#include <string>
#include <locale>
#include <codecvt>
#include <atlstr.h> // Cần thư viện ATL/MFC


using namespace CvPlus; 
using namespace System::Runtime::InteropServices;
namespace {
	struct GrabPause {
		CMvCamera* cam;
		bool stopped = false;
		explicit GrabPause(CMvCamera* c) : cam(c) {
			if (cam) { cam->StopGrabbing(); stopped = true; }
		}
		~GrabPause() {
			if (cam) cam->StartGrabbing();
		}
	};

	// Làm tròn theo step (step <= 0 thì bỏ qua)
	inline double round_to_step(double v, double minV, double step) {
		if (step <= 0.0) return v;
		return std::round((v - minV) / step) * step + minV;
	}

	// Thử tắt Auto nếu tham số liên quan đến Exposure/Gain (không fail toàn bộ nếu không hỗ trợ)
	inline void try_disable_auto(CMvCamera* cam, const std::string& key) {
		if (!cam) return;
		auto contains = [&](const char* s) {
			return key.find(s) != std::string::npos || key.find(std::string(s) + "Time") != std::string::npos;
			};
		// Exposure
		if (contains("Exposure") || contains("ExposureTime")) {
			// 0 = Off đối với enum ExposureAuto của HIK/MVS
			cam->SetEnumValue("ExposureAuto", 0);
		}
		// Gain
		if (contains("Gain")) {
			cam->SetEnumValue("GainAuto", 0);
		}
	}
	bool probe_float_increment(CMvCamera* cam, const string& key, double& incOut)
	{
		if (!cam) return false;

		static const char* SUFFIXES[] = { "Increment", "Inc", "Step", "MinStep" };

		// Thử đọc FLOAT node phụ
		for (auto suf : SUFFIXES) {
			string nid = key + suf;
			MVCC_FLOATVALUE fv{};
			if (cam->GetFloatValue(nid.c_str(), &fv) == MV_OK) {
				// Tuỳ model: dùng fCurValue hoặc fMin tuỳ cách SDK công bố
				double inc = (fv.fCurValue > 0) ? fv.fCurValue : fv.fMin;
				if (inc > 0) { incOut = inc; return true; }
			}
		}
		// Thử đọc INT node phụ (một số SDK công bố step dạng INT)
		for (auto suf : SUFFIXES) {
			string nid = key + suf;
			MVCC_INTVALUE_EX iv{};
			if (cam->GetIntValue(nid.c_str(), &iv) == MV_OK) {
				long long inc = iv.nCurValue ? iv.nCurValue : iv.nInc;
				if (inc > 0) { incOut = static_cast<double>(inc); return true; }
			}
		}
		return false;
	}
	static int ClampI(int v, int lo, int hi)
	{
		return v < lo ? lo : (v > hi ? hi : v);
	}
}

float CCD::SetPara(int indexCCD, System::String^ nameParaManaged, float valueIn)
{
	if (indexCCD < 0 || indexCCD >= (int)m_pcMyCamera.size() || m_pcMyCamera[indexCCD] == nullptr)
		return -1.0f;
	IsWaiting = true;

	CMvCamera* cam = m_pcMyCamera[indexCCD];

	std::string key = marshal_as<std::string>(nameParaManaged);

	// Tự tắt chế độ Auto nếu có (không coi là lỗi nếu không hỗ trợ)
	try_disable_auto(cam, key);

	// Đảm bảo luôn StartGrabbing lại nhờ RAII
	GrabPause guard(cam);

	// 1) Thử như FLOAT trước
	{
		MVCC_FLOATVALUE fv{}; // min/max/inc dạng double
		if (cam->GetFloatValue(key.c_str(), &fv) == MV_OK) {
			double v = (double)valueIn;
			// clamp min–max

			v =ClampI(v, fv.fMin, fv.fMax);
			// làm tròn theo step nếu có
		//	v = round_to_step(v, fv.fMin, fv.fInc);

			if (cam->SetFloatValue(key.c_str(), v) == MV_OK) {
				// Đọc lại giá trị thực sự áp dụng (nếu muốn chính xác)
				MVCC_FLOATVALUE fv2{};
				if (cam->GetFloatValue(key.c_str(), &fv2) == MV_OK) {
					IsWaiting = false;
					return (float)fv2.fCurValue;
				}
				IsWaiting = false;
				return (float)v;
			}
			// Nếu set float thất bại, đừng vội trả về — thử sang INT bên dưới
		}
	}

	// 2) Thử như INT (một số node là integer như Width/Height/OffsetX/OffsetY/Exposure lines trên vài model)
	{
		MVCC_INTVALUE_EX iv{};
		if (cam->GetIntValue(key.c_str(), &iv) == MV_OK) {
			long long minV = (long long)iv.nMin;
			long long maxV = (long long)iv.nMax;
			long long inc = (long long)(iv.nInc > 0 ? iv.nInc : 1);

			double v = ClampI((double)valueIn, (double)minV, (double)maxV);
			// làm tròn theo step integer
			long long steps = (long long)std::llround((v - minV) / inc);
			long long vAdj = minV + steps * inc;
			vAdj = ClampI(vAdj, minV, maxV);

			if (cam->SetIntValue(key.c_str(), vAdj) == MV_OK) {
				MVCC_INTVALUE_EX iv2{};
				if (cam->GetIntValue(key.c_str(), &iv2) == MV_OK) {
					IsWaiting = false;
					return (float)iv2.nCurValue;
				}
				IsWaiting = false;
				return (float)vAdj;
			}
		}
	}

	// 3) Không hỗ trợ hoặc set thất bại
	std::cerr << "❌ Set parameter failed: " << key << std::endl;
	IsWaiting = false;
	return -1.0f;
}


void ReadImage()
{

	ucRaw = MatToBytes(matRaw);//
}
void 	SetImage(int ixThread, int indexTool, uchar* uc, int image_rows, int image_cols, int image_type)
{
	
	m_matDst[ixThread][indexTool] = Mat();
	m_matDst[ixThread][indexTool] = BytesToMat(uc, image_rows, image_cols, image_type).clone();
	if (m_matDst[ixThread][indexTool].type() == CV_8UC3)
		cvtColor(m_matDst[ixThread][indexTool], m_matDst[ixThread][indexTool], COLOR_BGR2GRAY);
}
extern "C" __declspec(dllexport) uchar * GetImage(int* rows, int* cols, int* Type)
{
	int rows_ = matRaw.rows;
	int cols_ = matRaw.cols;
	int Type_ = matRaw.type();
	*rows = rows_;
	*cols = cols_;
	*Type = Type_;
	
	return  MatToBytes(matRaw);
}
extern "C" __declspec(dllexport)
void FreeBuffer(uchar* buffer)
{
	delete[] buffer;
}
extern "C" __declspec(dllexport) uchar * GetImageCrop(  int* rows, int* cols,int* Type)
{
	
	
	int rows_ = matCrop.rows;
	int cols_ = matCrop.cols;
	int Type_ = matCrop.type();
	*rows = rows_;
	*cols = cols_;
	*Type = Type_;
	return  MatToBytes(matCrop);
}
extern "C" __declspec(dllexport) uchar * GetImageResult(int* rows, int* cols, int* Type)
{


	int rows_ = matResult.rows;
	int cols_ = matResult.cols;
	int Type_ = matResult.type();
	*rows = rows_;
	*cols = cols_;
	*Type = Type_;

	return  MatToBytes(matResult);
}
extern "C" __declspec(dllexport) void SetDst(int ixThread,int indexTool, uchar * uc ,int image_rows, int image_cols,int image_type)
{
	
	SetImage(ixThread, indexTool,  uc,  image_rows,  image_cols,  image_type);
	
}
extern "C" __declspec(dllexport) void SetSrc(uchar * uc, int image_rows, int image_cols, int image_type)
{
	matRaw = BytesToMat(uc,image_rows, image_cols, image_type);

}
extern "C" __declspec(dllexport) void SetRaw(uchar* uc, int image_rows, int image_cols, int image_type)
{
	if (!uc || image_rows <= 0 || image_cols <= 0)
	{
		std::cerr << "Invalid input to SetRaw\n";
		return;
	}

	try
	{
		
		matRaw = BytesToMat(uc, image_rows, image_cols, image_type);
	}
	catch (cv::Exception& e)
	{
		std::cerr << "OpenCV error: " << e.what() << std::endl;
	}
	//// PHẢI clone nếu uc là vùng nhớ cấp từ C#
	//cv::Mat temp(image_rows, image_cols, image_type, uc);
	//if (temp.empty())return;
	//matRaw = temp.clone();  // ✅ an toàn
	////matRaw = BytesToMat(uc, image_rows, image_cols, image_type);

}
extern "C" __declspec(dllexport) void SetImgTemp(uchar* uc, int image_rows, int image_cols, int image_type)
{
	matSetTemp = BytesToMat(uc, image_rows, image_cols, image_type);

}
extern "C" __declspec(dllexport) uchar* GetRaw(int* rows, int* cols, int* Type)
{
	int rows_ = matRaw.rows;
	int cols_ = matRaw.cols;
	int Type_ = matRaw.type();
	*rows = rows_;
	*cols = cols_;
	*Type = Type_;

	return  MatToBytes(matRaw);
}
extern "C" __declspec(dllexport) uchar* GetCrop(int* rows, int* cols, int* Type)
{


	int rows_ = matCrop.rows;
	int cols_ = matCrop.cols;
	int Type_ = matCrop.type();
	*rows = rows_;
	*cols = cols_;
	*Type = Type_;
	return  MatToBytes(matCrop);
}
extern "C" __declspec(dllexport) uchar* GetResult(int* rows, int* cols, int* Type)
{


	int rows_ = matResult.rows;
	int cols_ = matResult.cols;
	int Type_ = matResult.type();
	*rows = rows_;
	*cols = cols_;
	*Type = Type_;

	return  MatToBytes(matResult);
}
#pragma region ScanCCD



std::map<int, Device> DeviceEnumerator::getVideoDevicesMap() {
	return getDevicesMap(CLSID_VideoInputDeviceCategory);
}

std::map<int, Device> DeviceEnumerator::getAudioDevicesMap() {
	return getDevicesMap(CLSID_AudioInputDeviceCategory);
}

// Returns a map of id and devices that can be used
std::map<int, Device> DeviceEnumerator::getDevicesMap(const GUID deviceClass)
{
	std::map<int, Device> deviceMap;

	HRESULT hr = CoInitialize(nullptr);
	if (FAILED(hr)) {
		return deviceMap; // Empty deviceMap as an error
	}

	// Create the System Device Enumerator
	ICreateDevEnum* pDevEnum;
	hr = CoCreateInstance(CLSID_SystemDeviceEnum, NULL, CLSCTX_INPROC_SERVER, IID_PPV_ARGS(&pDevEnum));

	// If succeeded, create an enumerator for the category
	IEnumMoniker* pEnum = NULL;
	if (SUCCEEDED(hr)) {
		hr = pDevEnum->CreateClassEnumerator(deviceClass, &pEnum, 0);
		if (hr == S_FALSE) {
			hr = VFW_E_NOT_FOUND;
		}
		pDevEnum->Release();
	}

	// Now we check if the enumerator creation succeeded
	int deviceId = -1;
	if (SUCCEEDED(hr)) {
		// Fill the map with id and friendly device name
		IMoniker* pMoniker = NULL;
		while (pEnum->Next(1, &pMoniker, NULL) == S_OK) {

			IPropertyBag* pPropBag;
			HRESULT hr = pMoniker->BindToStorage(0, 0, IID_PPV_ARGS(&pPropBag));
			if (FAILED(hr)) {
				pMoniker->Release();
				continue;
			}

			// Create variant to hold data
			VARIANT var;
			VariantInit(&var);

			std::string deviceName;
			std::string devicePath;

			// Read FriendlyName or Description
			hr = pPropBag->Read(L"Description", &var, 0); // Read description
			if (FAILED(hr)) {
				// If description fails, try with the friendly name
				hr = pPropBag->Read(L"FriendlyName", &var, 0);
			}
			// If still fails, continue with next device
			if (FAILED(hr)) {
				VariantClear(&var);
				continue;
			}
			// Convert to string
			else {
				deviceName = ConvertBSTRToMBS(var.bstrVal);
			}

			VariantClear(&var); // We clean the variable in order to read the next value

								// We try to read the DevicePath
			hr = pPropBag->Read(L"DevicePath", &var, 0);
			if (FAILED(hr)) {
				VariantClear(&var);
				continue; // If it fails we continue with next device
			}
			else {
				devicePath = ConvertBSTRToMBS(var.bstrVal);
			}

			// We populate the map
			deviceId++;
			Device currentDevice;
			currentDevice.id = deviceId;
			currentDevice.deviceName = deviceName;
			currentDevice.devicePath = devicePath;
			deviceMap[deviceId] = currentDevice;

		}
		pEnum->Release();
	}
	CoUninitialize();
	return deviceMap;
}

/*
This two methods were taken from
https://stackoverflow.com/questions/6284524/bstr-to-stdstring-stdwstring-and-vice-versa
*/

std::string DeviceEnumerator::ConvertBSTRToMBS(BSTR bstr)
{
	int wslen = ::SysStringLen(bstr);
	return ConvertWCSToMBS((wchar_t*)bstr, wslen);
}

std::string DeviceEnumerator::ConvertWCSToMBS(const wchar_t* pstr, long wslen)
{
	int len = ::WideCharToMultiByte(CP_ACP, 0, pstr, wslen, NULL, 0, NULL, NULL);

	std::string dblstr(len, '\0');
	len = ::WideCharToMultiByte(CP_ACP, 0 /* no flags */,
		pstr, wslen /* not necessary NULL-terminated */,
		&dblstr[0], len,
		NULL, NULL /* no default char */);

	return dblstr;
}
#pragma endregion

int TypeCCDPlus = 0;
std::string wchar_to_string(const std::wstring& wstr) {
	std::wstring_convert<std::codecvt_utf8<wchar_t>> converter;
	return converter.to_bytes(wstr);
}
System::String^ ScanHik()
{
	CString strMsg;

	
	memset(&m_stDevList, 0, sizeof(MV_CC_DEVICE_INFO_LIST));

	// ch:枚举子网内所有设备 | en:Enumerate all devices within subnet
	int nRet = CMvCamera::EnumDevices(MV_GIGE_DEVICE | MV_USB_DEVICE | MV_GENTL_GIGE_DEVICE | MV_GENTL_CAMERALINK_DEVICE |
		MV_GENTL_CXP_DEVICE | MV_GENTL_XOF_DEVICE, &m_stDevList);
	std::string list1 = "";
	if (MV_OK != nRet)
	{
		return gcnew  System::String("Unknown");;
	
	}
	// ch:将值加入到信息列表框中并显示出来 | en:Add value to the information list box and display
	for (unsigned int i = 0; i < m_stDevList.nDeviceNum; i++)
	{
		MV_CC_DEVICE_INFO* pDeviceInfo = m_stDevList.pDeviceInfo[i];
		if (NULL == pDeviceInfo)
		{
			continue;
		}
		wchar_t* sManufacturerName = NULL;
		char strUserName[256] = { 0 };
		wchar_t* pUserName = NULL;
		sprintf_s(strUserName, 256, "%s (%s)", pDeviceInfo->SpecialInfo.stGigEInfo.chManufacturerName,
			pDeviceInfo->SpecialInfo.stGigEInfo.chSerialNumber);
		DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
		sManufacturerName = new wchar_t[dwLenUserName];
		const wchar_t* needle = L"Basler";
		const wchar_t* needle2 = L"Hik";
		if (wcsstr(sManufacturerName, needle)) {
			TypeCCDPlus = 0;
		}
		else if (wcsstr(sManufacturerName, needle2)) {
			TypeCCDPlus = 1;
		}
		else {
			TypeCCDPlus = -1;
		}
		if (pDeviceInfo->nTLayerType == MV_GIGE_DEVICE)
		{
			/*int nIp1 = ((m_stDevList.pDeviceInfo[i]->SpecialInfo.stGigEInfo.nCurrentIp & 0xff000000) >> 24);
			int nIp2 = ((m_stDevList.pDeviceInfo[i]->SpecialInfo.stGigEInfo.nCurrentIp & 0x00ff0000) >> 16);
			int nIp3 = ((m_stDevList.pDeviceInfo[i]->SpecialInfo.stGigEInfo.nCurrentIp & 0x0000ff00) >> 8);
			int nIp4 = (m_stDevList.pDeviceInfo[i]->SpecialInfo.stGigEInfo.nCurrentIp & 0x000000ff);
			*/
			if (strcmp("", (LPCSTR)(pDeviceInfo->SpecialInfo.stGigEInfo.chUserDefinedName)) != 0)
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stGigEInfo.chUserDefinedName,
					pDeviceInfo->SpecialInfo.stGigEInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stGigEInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			else
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stGigEInfo.chModelName,
					pDeviceInfo->SpecialInfo.stGigEInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stGigEInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			memset(strUserName, 0, 256);
		
			MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, sManufacturerName, dwLenUserName);
	
			strMsg.Format(_T("%s"),pUserName );
		}
		else if (pDeviceInfo->nTLayerType == MV_USB_DEVICE)
		{
			if (strcmp("", (LPCSTR)(pDeviceInfo->SpecialInfo.stUsb3VInfo.chUserDefinedName)) != 0)
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stUsb3VInfo.chUserDefinedName,
					pDeviceInfo->SpecialInfo.stUsb3VInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stUsb3VInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			else
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stUsb3VInfo.chModelName,
					pDeviceInfo->SpecialInfo.stUsb3VInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stUsb3VInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			memset(strUserName, 0, 256);

			MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, sManufacturerName, dwLenUserName);

			strMsg.Format(_T("%s"), pUserName);
		
		}
		else if (pDeviceInfo->nTLayerType == MV_GENTL_CAMERALINK_DEVICE)
		{
			if (strcmp("", (char*)pDeviceInfo->SpecialInfo.stCMLInfo.chUserDefinedName) != 0)
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)", pDeviceInfo->SpecialInfo.stCMLInfo.chUserDefinedName,
					pDeviceInfo->SpecialInfo.stCMLInfo.chSerialNumber);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			else
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)", pDeviceInfo->SpecialInfo.stCMLInfo.chModelName,
					pDeviceInfo->SpecialInfo.stCMLInfo.chSerialNumber);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			strMsg.Format(_T("[%d]CML:  %s"), i, pUserName);
		}
		else if (pDeviceInfo->nTLayerType == MV_GENTL_CXP_DEVICE)
		{
			if (strcmp("", (char*)pDeviceInfo->SpecialInfo.stCXPInfo.chUserDefinedName) != 0)
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)", pDeviceInfo->SpecialInfo.stCXPInfo.chUserDefinedName,
					pDeviceInfo->SpecialInfo.stCXPInfo.chSerialNumber);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			else
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)", pDeviceInfo->SpecialInfo.stCXPInfo.chModelName,
					pDeviceInfo->SpecialInfo.stCXPInfo.chSerialNumber);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			strMsg.Format(_T("[%d]CXP:  %s"), i, pUserName);
		}
		else if (pDeviceInfo->nTLayerType == MV_GENTL_XOF_DEVICE)
		{
			if (strcmp("", (char*)pDeviceInfo->SpecialInfo.stXoFInfo.chUserDefinedName) != 0)
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)", pDeviceInfo->SpecialInfo.stXoFInfo.chUserDefinedName,
					pDeviceInfo->SpecialInfo.stXoFInfo.chSerialNumber);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			else
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)", pDeviceInfo->SpecialInfo.stXoFInfo.chModelName,
					pDeviceInfo->SpecialInfo.stXoFInfo.chSerialNumber);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			strMsg.Format(_T("[%d]XoF:  %s"), i, pUserName);
		}
		else
		{
			return gcnew  System::String("Unknown device enumerated");;
			//ShowErrorMsg(TEXT("Unknown device enumerated"), 0);
		}
		
	//	m_ctrlDeviceCombo.AddString(strMsg);

		if (pUserName)
		{
			delete[] pUserName;
			pUserName = NULL;
		}
	
		list1.append(wchar_to_string(strMsg.GetString())).append("\n");
	}
	
	if (0 == m_stDevList.nDeviceNum)
	{
		return gcnew  System::String("No Device");;
		//ShowErrorMsg(TEXT("No device"), 0);
		//return;
	}
	return gcnew  System::String(list1.c_str());;
	//m_ctrlDeviceCombo.SetCurSel(0);
}
vector<string> listCCCD;
System::String^ ScanUsb()
{
	listCCCD = vector<string>();
	System::String^ ListCCD = "";
	DeviceEnumerator de;
	// Audio Devices
	std::map<int, Device> devices;
	std::string list1 = "";
	devices = de.getVideoDevicesMap();
	for (auto const& device : devices) {
		
		list1.append(device.second.deviceName).append("$").append(device.second.devicePath).append("\n");
		listCCCD.push_back(list1);
		//.append(std::to_string(device.first)).append("$").
	}
	//ListCCD=System::String(list1.c_str()); //Marshal::PtrToStringAnsi(&list1, NULL);
	//Console::WriteLine(ListCCD);
	return gcnew  System::String(list1.c_str());;
}
System::String^ CCD::ScanCCD()
{
	System::String^ nameCCD="";
	switch (TypeCamera)
	{
	case 1:
	nameCCD= ScanHik();
		break;
	default: 
		nameCCD =ScanUsb();
		break;
	}
	return nameCCD;
}

int CCD::GetTypeCCD(int indexCCD)
{
	return listTypeCCD[indexCCD];
}

bool CCD::GetPara(
	int indexCCD,
	System::String^ namePara,
	float% minVal,
	float% maxVal,
	float% stepVal,
	float% currentVal
	
)
{
	// Mặc định
	minVal = maxVal = stepVal = currentVal = 0.0f;
	

	if (indexCCD < 0 || indexCCD >= (int)m_pcMyCamera.size() || m_pcMyCamera[indexCCD] == nullptr) {
		std::cerr << "❌ Camera index invalid/null" << std::endl;
		return false;
	}
	IsWaiting = true;
	using msclr::interop::marshal_as;
	string key = marshal_as<string>(namePara);
	CMvCamera* cam = m_pcMyCamera[indexCCD];

	// 1) Thử FLOAT trước
	{
		MVCC_FLOATVALUE fv{};
		if (cam->GetFloatValue(key.c_str(), &fv) == MV_OK) {
			minVal = static_cast<float>(fv.fMin);
			maxVal = static_cast<float>(fv.fMax);
			currentVal = static_cast<float>(fv.fCurValue);

			// Cố gắng dò step; nếu không có → để 0 (UI tuỳ chọn xử lý: free slider / nhập tự do)
			double inc = 0.0;
			if (probe_float_increment(cam, key, inc)) {
				stepVal = static_cast<float>(inc);
			}
			else {
				stepVal = 0.0f; // không công bố step cho FLOAT
			}
			IsWaiting = false;
			//typeName = "Float";
			return true;
		}
	}

	// 2) Nếu không phải FLOAT, thử INT
	{
		MVCC_INTVALUE_EX iv{};
		if (cam->GetIntValue(key.c_str(), &iv) == MV_OK) {
			minVal = static_cast<float>(iv.nMin);
			maxVal = static_cast<float>(iv.nMax);
			currentVal = static_cast<float>(iv.nCurValue);

			// nInc có thể =0 ở vài node; chuẩn hoá về >=1
			long long inc = (iv.nInc > 0 ? iv.nInc : 1);
			stepVal = static_cast<float>(inc);
			IsWaiting = false;
			//typeName = "Int";
			return true;
		}
	}
	IsWaiting = false;
	// 3) Không đọc được
	std::cerr << "❌ Không đọc được thông tin tham số: " << key << std::endl;
	return false;
}
void CCD::SetFocus(int Focus)
{
	camUSB.set(CAP_PROP_AUTOFOCUS, 0);
	camUSB.set(CAP_PROP_FOCUS, Focus);
}
void CCD::SetZoom(int Zoom)
{
	camUSB.set(CAP_PROP_ZOOM, Zoom);
	
}
int CCD::GetZoom()
{
return	camUSB.get(CAP_PROP_ZOOM);

}
int CCD::GetFocus()
{
	return	camUSB.get(CAP_PROP_FOCUS);

}
int CCD::GetExposure()
{
	return	camUSB.get(CAP_PROP_EXPOSURE);

}
void CCD::SetExposure(int Value)
{
	camUSB.set(CAP_PROP_AUTO_EXPOSURE, 0);
	camUSB.set(CAP_PROP_EXPOSURE, Value);
}
void CCD::AutoFocus(bool Auto)
{
	
	camUSB.set(CAP_PROP_AUTOFOCUS, Auto);

}
void CCD::SetWidth(int Value)
{
	camUSB.set(CAP_PROP_FRAME_WIDTH, Value);
}
void CCD::SetHeight(int Value)
{
	camUSB.set(CAP_PROP_FRAME_HEIGHT, Value);
}
int CCD::GetWidth()
{
	return camUSB.get(CAP_PROP_FRAME_WIDTH);
}
int CCD::GetHeight()
{
	return camUSB.get(CAP_PROP_FRAME_HEIGHT);
}
bool ConnectUsb( int index)
{
	camUSB.open(index);
	//camUSB.set(CAP_PROP_FRAME_WIDTH, colCCD);
	///camUSB.set(CAP_PROP_FRAME_HEIGHT, rowCCD);
	camUSB.set(CAP_PROP_AUTOFOCUS, 0);
	//camUSB.set(CAP_PROP_FOCUS, 12);
	camUSB.set(CAP_PROP_AUTO_EXPOSURE, 0);
	//camUSB.set(CAP_PROP_EXPOSURE, -11);
	camUSB.set(cv::CAP_PROP_FPS, 60);          // Tăng số khung hình trên giây

	if (camUSB.isOpened())
	{
		camUSB.read(matRaw);
		camUSB.read(matRaw);
		camUSB.read(matRaw);
		camUSB.read(matRaw);

		return true;
	}
	return false;
}

// ---- Soft reset qua GenICam command "DeviceReset" ----
static bool DeviceReset(CMvCamera* camera, int waitMsAfter = 3000)
{
	if (!camera) return false;
	camera->StopGrabbing(); // an toàn: dừng stream nếu đang chạy

	int r = camera->CommandExecute("DeviceReset"); // gọi GenICam command
	if (r != MV_OK) {
		std::cerr << "DeviceReset unsupported/failed: " << r << "\n";
		return false; // hoặc fallback: close/reopen
	}

	std::this_thread::sleep_for(std::chrono::milliseconds(waitMsAfter));
}

int FindCameraIndexByUserName(MV_CC_DEVICE_INFO_LIST& devList, const std::string& targetUserName)
{
	for (unsigned int i = 0; i < devList.nDeviceNum; ++i)
	{
		MV_CC_DEVICE_INFO* pDeviceInfo = devList.pDeviceInfo[i];
		if (pDeviceInfo == nullptr)
			continue;

		if (pDeviceInfo->nTLayerType == MV_GIGE_DEVICE)
		{
		//	const char* userName = (const char*)pDeviceInfo->SpecialInfo.stGigEInfo.chUserDefinedName;
			char strUserName[256] = { 0 };
			wchar_t* pUserName = NULL;
			if (strcmp("", (LPCSTR)(pDeviceInfo->SpecialInfo.stGigEInfo.chUserDefinedName)) != 0)
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stGigEInfo.chUserDefinedName,
					pDeviceInfo->SpecialInfo.stGigEInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stGigEInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			else
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stGigEInfo.chModelName,
					pDeviceInfo->SpecialInfo.stGigEInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stGigEInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			/*if (strcmp("", (LPCSTR)(pDeviceInfo->SpecialInfo.stGigEInfo.chUserDefinedName)) != 0)
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stGigEInfo.chUserDefinedName,
					pDeviceInfo->SpecialInfo.stGigEInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stGigEInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			else
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stGigEInfo.chUserDefinedName,
					pDeviceInfo->SpecialInfo.stGigEInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stGigEInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}*/
			std::wstring targetWStr = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(targetUserName);

			if (pUserName && wcscmp(pUserName, targetWStr.c_str()) == 0)
			{
				return i;
			}
			
		}
		else if (pDeviceInfo->nTLayerType == MV_USB_DEVICE)
		{
			char strUserName[256] = { 0 };
			wchar_t* pUserName = NULL;
			if (strcmp("", (LPCSTR)(pDeviceInfo->SpecialInfo.stUsb3VInfo.chUserDefinedName)) != 0)
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stUsb3VInfo.chUserDefinedName,
					pDeviceInfo->SpecialInfo.stUsb3VInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stUsb3VInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			else
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stUsb3VInfo.chModelName,
					pDeviceInfo->SpecialInfo.stUsb3VInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stUsb3VInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			//memset(strUserName, 0, 256);

			//MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, sManufacturerName, dwLenUserName);

		/*	strMsg.Format(_T("%s"), pUserName);
			if (strcmp("", (LPCSTR)(pDeviceInfo->SpecialInfo.stUsb3VInfo.chUserDefinedName)) != 0)
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stUsb3VInfo.chUserDefinedName,
					pDeviceInfo->SpecialInfo.stUsb3VInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stUsb3VInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}
			else
			{
				memset(strUserName, 0, 256);
				sprintf_s(strUserName, 256, "%s (%s)$%s$", pDeviceInfo->SpecialInfo.stUsb3VInfo.chUserDefinedName,
					pDeviceInfo->SpecialInfo.stUsb3VInfo.chSerialNumber, pDeviceInfo->SpecialInfo.stUsb3VInfo.chManufacturerName);
				DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
				pUserName = new wchar_t[dwLenUserName];
				MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			}*/
			std::wstring targetWStr = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(targetUserName);

			if (pUserName && wcscmp(pUserName, targetWStr.c_str()) == 0)
			{
				return i;
			}
			////const char* userName = (const char*)pDeviceInfo->SpecialInfo.stUsb3VInfo.chUserDefinedName;
			////if (userName && strcmp(userName, targetUserName.c_str()) == 0)
			////{
			////	return i; // tìm thấy index
			////}
			//char strUserName[256] = { 0 };
			//wchar_t* pUserName = NULL;
			//if (strcmp("", (LPCSTR)(pDeviceInfo->SpecialInfo.stUsb3VInfo.chUserDefinedName)) != 0)
			//{
			//	memset(strUserName, 0, 256);
			//	sprintf_s(strUserName, 256, "%s (%s)", pDeviceInfo->SpecialInfo.stUsb3VInfo.chUserDefinedName,
			//		pDeviceInfo->SpecialInfo.stUsb3VInfo.chSerialNumber);
			//	DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
			//	pUserName = new wchar_t[dwLenUserName];
			//	MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			//}
			//else
			//{
			//	memset(strUserName, 0, 256);
			//	sprintf_s(strUserName, 256, "%s (%s)", pDeviceInfo->SpecialInfo.stUsb3VInfo.chModelName,
			//		pDeviceInfo->SpecialInfo.stUsb3VInfo.chSerialNumber);
			//	DWORD dwLenUserName = MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, NULL, 0);
			//	pUserName = new wchar_t[dwLenUserName];
			//	MultiByteToWideChar(CP_ACP, 0, (LPCSTR)(strUserName), -1, pUserName, dwLenUserName);
			//}std::wstring targetWStr = std::wstring_convert<std::codecvt_utf8<wchar_t>>().from_bytes(targetUserName);

			//if (pUserName && wcscmp(pUserName, targetWStr.c_str()) == 0)
			//{
			//	return i;
			//}
		}
	}

	return -1; // Không tìm thấy
}
bool ConnectHik( int index,int indexCCD)
{

	int nRet = 0;
	

	// ch:由设备信息创建设备实例 | en:Device instance created by device information
	if (NULL == m_stDevList.pDeviceInfo[index])
	{
		//ShowErrorMsg(TEXT("Device does not exist"), 0);
		return false;
	}
	listTypeCCD[indexCCD] = TypeCCDPlus;
		m_pcMyCamera[indexCCD] = new CMvCamera;
		if (NULL == m_pcMyCamera[indexCCD])
		{
			return false;
		}
		 nRet = m_pcMyCamera[indexCCD]->Open(m_stDevList.pDeviceInfo[index]);
		if (MV_OK != nRet)
		{
			//DeviceReset(m_pcMyCamera[indexCCD]);
			delete m_pcMyCamera[indexCCD];
			m_pcMyCamera[indexCCD] = NULL;
			//ShowErrorMsg(TEXT("Open Fail"), nRet);
			return false;
		}

		// ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
		if (m_stDevList.pDeviceInfo[index]->nTLayerType == MV_GIGE_DEVICE)
		{
			unsigned int nPacketSize = 0;
			nRet = m_pcMyCamera[indexCCD]->GetOptimalPacketSize(&nPacketSize);
			if (nRet == MV_OK)
			{
				nRet = m_pcMyCamera[indexCCD]->SetIntValue("GevSCPSPacketSize", nPacketSize);
				if (nRet != MV_OK)
				{
					return false;
					
				}
			}
			else
			{
				return false;
				
			}
		}

		m_pcMyCamera[indexCCD]->StartGrabbing();

	return true;
}
bool CCD::Connect(int indexCCD, System::String^ NameCamera)
{
	std::string nameCCD = marshal_as<std::string>(NameCamera);
	numERR = 0;
	IsErrCCD = false;
	bool IsConnect = false;
	switch (TypeCamera)
	{
	case 1:
	{
		int index = FindCameraIndexByUserName(m_stDevList, nameCCD);
		if (index <= -1)
			return false;
		IsConnect = ConnectHik(index, indexCCD);
	
		break;
	}

	case 0:
	{
		
			IsConnect = ConnectUsb(indexCCD);

		break;
	}
	}

	return IsConnect;
}

std::chrono::duration<double> elapsed;

int FormatCCD=0;
// ====== timing (tuỳ chọn) ======
static auto lastTime = std::chrono::high_resolution_clock::now();
static int frameCount = 0;
static double fps = 0.0;
//
//static inline void EnsureSize(cv::Mat& img, int w, int h, int type) {
//	if (img.empty() || img.cols != w || img.rows != h || img.type() != type)
//		img.create(h, w, type);
//}
//
//static inline bool ConvertBySDKEx(CMvCamera* camera,
//	const MV_FRAME_OUT& out,
//	cv::Mat& dstBGR)
//{
//	const unsigned w = out.stFrameInfo.nWidth;
//	const unsigned h = out.stFrameInfo.nHeight;
//
//	EnsureSize(dstBGR, (int)w, (int)h, CV_8UC3);
//
//	MV_CC_PIXEL_CONVERT_PARAM_EX prm{};
//	prm.nWidth = w;
//	prm.nHeight = h;
//	prm.enSrcPixelType = (MvGvspPixelType)out.stFrameInfo.enPixelType;
//	prm.pSrcData = (unsigned char*)out.pBufAddr;
//	prm.nSrcDataLen = out.stFrameInfo.nFrameLen;
//	prm.enDstPixelType = PixelType_Gvsp_BGR8_Packed;
//	prm.pDstBuffer = dstBGR.data;
//	prm.nDstBufferSize = (unsigned)dstBGR.total() * (unsigned)dstBGR.elemSize();
//	prm.nDstLen = 0;
//
//	int ret = camera->ConvertPixelType(&prm);
//	if (ret != MV_OK) {
//		std::cerr << "ConvertPixelTypeEx failed: " << ret
//			<< " (srcPix=0x" << std::hex << out.stFrameInfo.enPixelType << std::dec << ")\n";
//		return false;
//	}
//	return true;
//}
//
//// (Tuỳ chọn) zero-copy cho BGR8: chỉ dùng trong scope hiện tại
//struct BorrowedFrame {
//	cv::Mat view; // KHÔNG sở hữu bộ nhớ
//	struct Guard {
//		CMvCamera* cam{ nullptr };
//		MV_FRAME_OUT* out{ nullptr };
//		~Guard() { if (cam && out) cam->FreeImageBuffer(out); }
//	} guard;
//};
//
//static inline bool CaptureFrameView_Borrowed(CMvCamera* camera,
//	BorrowedFrame& bf,
//	int timeoutMs = 1000)
//{
//	MV_FRAME_OUT out{};
//	int ret = camera->GetImageBuffer(&out, timeoutMs);
//	if (ret != MV_OK) return false;
//
//	const auto& info = out.stFrameInfo;
//	if (!out.pBufAddr || info.nWidth == 0 || info.nHeight == 0) {
//		camera->FreeImageBuffer(&out);
//		return false;
//	}
//	if (info.enPixelType != PixelType_Gvsp_BGR8_Packed) {
//		camera->FreeImageBuffer(&out);
//		return false;
//	}
//
//	size_t step = (size_t)info.nWidth * 3;
//#ifdef MV_FRAME_OUT_INFO_EX_HAS_STRIDE
//	if (info.nStride) step = info.nStride;
//#endif
//
//	bf.guard.cam = camera;
//	bf.guard.out = &out;
//	bf.view = cv::Mat((int)info.nHeight, (int)info.nWidth, CV_8UC3, out.pBufAddr, step);
//	return true;
//}

// Chính: lấy frame ra Mat BGR (an toàn, sở hữu bộ nhớ)
//bool CaptureFrameMat(CMvCamera* camera, cv::Mat& imageBGR, int timeoutMs = 1000)
//{
//	
//	MV_FRAME_OUT out{};
//	int ret = camera->GetImageBuffer(&out, timeoutMs);
//	if (ret != MV_OK) return false;
//
//	struct Guard {
//		CMvCamera* cam; MV_FRAME_OUT* o;
//		~Guard() { if (cam && o) cam->FreeImageBuffer(o); }
//	} guard{ camera, &out };
//
//	const auto& info = out.stFrameInfo;
//	const unsigned w = info.nWidth;
//	const unsigned h = info.nHeight;
//	const unsigned px = info.enPixelType;
//	unsigned char* p = static_cast<unsigned char*>(out.pBufAddr);
//	if (!p || w == 0 || h == 0) return false;
//
//	size_t step = 0;
//#ifdef MV_FRAME_OUT_INFO_EX_HAS_STRIDE
//	step = info.nStride; // nếu SDK có nStride thì dùng
//#endif
//
//	switch (px)
//	{
//	case PixelType_Gvsp_BGR8_Packed:
//	{
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		if (step && step != (size_t)w * 3) {
//			cv::Mat view((int)h, (int)w, CV_8UC3, p, step);
//			view.copyTo(imageBGR);
//		}
//		else {
//			cv::Mat view((int)h, (int)w, CV_8UC3, p);
//			view.copyTo(imageBGR);
//		}
//		break;
//	}
//
//	case PixelType_Gvsp_RGB8_Packed:
//	{
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat rgb((int)h, (int)w, CV_8UC3, p, step ? step : (size_t)w * 3);
//		const int fromTo[6] = { 2,0, 1,1, 0,2 }; // R,G,B -> B,G,R
//		cv::mixChannels(&rgb, 1, &imageBGR, 1, fromTo, 3);
//		break;
//	}
//
//	case PixelType_Gvsp_Mono8:
//	{
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat mono((int)h, (int)w, CV_8UC1, p, step ? step : (size_t)w);
//		cv::cvtColor(mono, imageBGR, cv::COLOR_GRAY2BGR);
//		break;
//	}
//
//	// ===== Bayer 8-bit (OpenCV demosaic ra BGR) =====
//	case PixelType_Gvsp_BayerBG8:
//	{
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat raw((int)h, (int)w, CV_8UC1, p, step ? step : (size_t)w);
//		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerBG2BGR); // chuẩn BGR
//		break;
//	}
//	case PixelType_Gvsp_BayerGB8:
//	{
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat raw((int)h, (int)w, CV_8UC1, p, step ? step : (size_t)w);
//		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerGB2BGR);
//		break;
//	}
//	case PixelType_Gvsp_BayerRG8:
//	{
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat raw((int)h, (int)w, CV_8UC1, p, step ? step : (size_t)w);
//		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerRG2BGR);
//		break;
//	}
//	case PixelType_Gvsp_BayerGR8:
//	{
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat raw((int)h, (int)w, CV_8UC1, p, step ? step : (size_t)w);
//		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerGR2BGR);
//		break;
//	}
//
//	default:
//		if (!ConvertBySDKEx(camera, out, imageBGR)) return false;
//		break;
//	}
//
//	// ===== đếm FPS (tuỳ chọn) =====
//	if (++frameCount >= 30) {
//		auto now = std::chrono::high_resolution_clock::now();
//		std::chrono::duration<double> dt = now - lastTime;
//		fps = frameCount / dt.count();
//		frameCount = 0;
//		lastTime = now;
//		// std::cout << "FPS: " << fps << "\n";
//	}
//	return true;
//}
static inline void EnsureSize(cv::Mat& img, int w, int h, int type) {
	if (img.empty() || img.cols != w || img.rows != h || img.type() != type)
		img.create(h, w, type);
}

static inline void RGBtoBGR(const cv::Mat& srcRGB, cv::Mat& dstBGR) {
	const int fromTo[6] = { 2,0, 1,1, 0,2 };
	cv::mixChannels(&srcRGB, 1, &dstBGR, 1, fromTo, 3);
}
static inline bool ConvertBySDKEx(CMvCamera* camera, const MV_FRAME_OUT& out, cv::Mat& dstBGR) {
	const unsigned w = out.stFrameInfo.nWidth;
	const unsigned h = out.stFrameInfo.nHeight;
	EnsureSize(dstBGR, (int)w, (int)h, CV_8UC3);

	MV_CC_PIXEL_CONVERT_PARAM_EX prm{};
	prm.nWidth = w;
	prm.nHeight = h;
	prm.enSrcPixelType = (MvGvspPixelType)out.stFrameInfo.enPixelType;
	prm.pSrcData = (unsigned char*)out.pBufAddr;      // cast đúng kiểu
	prm.nSrcDataLen = out.stFrameInfo.nFrameLen;         // dùng frameLen từ SDK
	prm.enDstPixelType = (MvGvspPixelType)PixelType_Gvsp_BGR8_Packed;
	prm.pDstBuffer = dstBGR.data;
	prm.nDstBufferSize = w * h * 3;
	prm.nDstLen = 0;

	int ret = camera->ConvertPixelType(&prm);
	if (ret != MV_OK) {
		std::cerr << "ConvertPixelTypeEx failed: " << ret
			<< " (srcPix=0x" << std::hex << out.stFrameInfo.enPixelType << std::dec << ")\n";
		return false;
	}
	return true;
}
bool CaptureFrameMat(CMvCamera* camera, cv::Mat& imageBGR, int timeoutMs = 1000)
{
	MV_FRAME_OUT out{};
	if (camera->GetImageBuffer(&out, timeoutMs) != MV_OK)
		return false;

	struct Guard {
		CMvCamera* cam; MV_FRAME_OUT* o;
		~Guard() { cam->FreeImageBuffer(o); }
	} guard{ camera, &out };

	auto& info = out.stFrameInfo;
	if (!out.pBufAddr || info.nWidth == 0 || info.nHeight == 0)
		return false;

	int w = info.nWidth, h = info.nHeight;
	void* p = out.pBufAddr;
	return ConvertBySDKEx(camera, out, imageBGR);
	switch (info.enPixelType)
	{
	case PixelType_Gvsp_BGR8_Packed:
		EnsureSize(imageBGR, w, h, CV_8UC3);
		cv::Mat(h, w, CV_8UC3, p).copyTo(imageBGR);
		return true;

	case PixelType_Gvsp_RGB8_Packed:
		EnsureSize(imageBGR, w, h, CV_8UC3);
		cv::cvtColor(cv::Mat(h, w, CV_8UC3, p), imageBGR, cv::COLOR_RGB2BGR);
		return true;

	case PixelType_Gvsp_Mono8:
		EnsureSize(imageBGR, w, h, CV_8UC3);
		//cv::cvtColor(cv::Mat(h, w, CV_8UC1, p), imageBGR, cv::COLOR_GRAY2BGR);
		return true;

	case PixelType_Gvsp_BayerBG8:
		EnsureSize(imageBGR, w, h, CV_8UC3);
		cv::cvtColor(cv::Mat(h, w, CV_8UC1, p), imageBGR, cv::COLOR_BayerBG2RGB);
		return true;
	case PixelType_Gvsp_BayerRG8:
		EnsureSize(imageBGR, w, h, CV_8UC3);
		cv::Mat(h, w, CV_8UC1, p).copyTo(imageBGR);
		//cv::cvtColor(cv::Mat(h, w, CV_8UC1, p), imageBGR, cv::COLOR_BayerBG2RGB);
		return true;


	default:
		return ConvertBySDKEx(camera, out, imageBGR);
	}
}

//bool CaptureFrameMat(CMvCamera* camera, cv::Mat& imageBGR, int timeoutMs = 1000)
//{
//	MV_FRAME_OUT out{};
//	int ret = camera->GetImageBuffer(&out, timeoutMs);
//	if (ret != MV_OK) {
//		std::cerr << "GetImageBuffer failed: " << ret << "\n";
//		return false;
//	}
//
//	struct Guard {
//		CMvCamera* cam; MV_FRAME_OUT* o;
//		~Guard() { if (cam && o) cam->FreeImageBuffer(o); }
//	} guard{ camera, &out };
//
//	const unsigned int w = out.stFrameInfo.nWidth;
//	const unsigned int h = out.stFrameInfo.nHeight;
//	const unsigned int px = out.stFrameInfo.enPixelType;
//	void* p = out.pBufAddr;
//	if (!p || w == 0 || h == 0) return false;
//
//	// --- 1) BGR8: copy thẳng (1 lần) vào buffer tái sử dụng
//	if (px == PixelType_Gvsp_BGR8_Packed) {
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat view(h, w, CV_8UC3, p);
//		view.copyTo(imageBGR);
//		return true;
//	}
//
//	// --- 2) RGB8: đảo kênh → BGR (chuẩn OpenCV)
//	if (px == PixelType_Gvsp_RGB8_Packed) {
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat rgb(h, w, CV_8UC3, p);
//		RGBtoBGR(rgb, imageBGR);
//		return true;
//	}
//
//	// --- 3) MONO8: nâng lên BGR
//	if (px == PixelType_Gvsp_Mono8) {
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat mono(h, w, CV_8UC1, p);
//		cv::cvtColor(mono, imageBGR, cv::COLOR_GRAY2BGR);
//		return true;
//	}
//	// --- 2) RGB8: đảo kênh → BGR (chuẩn OpenCV)
//	if (px == PixelType_Gvsp_BayerBG8) {
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat raw(h, w, CV_8UC1, p);
//		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerBG2RGB);
//		return true;
//	}
//	// --- 4) Bayer8: demosaic bằng OpenCV (nhanh). Có 4 pattern thường gặp.
//	// Nếu bạn muốn màu đẹp hơn (HQ), có thể bỏ 4 nhánh này để dùng SDK ConvertBySDKEx.
//	/*if (px == PixelType_Gvsp_BayerBG8) {
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat raw(h, w, CV_8UC1, p);
//		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerBG2BGR);
//		return true;
//	}
//	if (px == PixelType_Gvsp_BayerGB8) {
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat raw(h, w, CV_8UC1, p);
//		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerGB2BGR);
//		return true;
//	}
//	if (px == PixelType_Gvsp_BayerRG8) {
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat raw(h, w, CV_8UC1, p);
//		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerRG2BGR);
//		return true;
//	}
//	if (px == PixelType_Gvsp_BayerGR8) {
//		EnsureSize(imageBGR, (int)w, (int)h, CV_8UC3);
//		cv::Mat raw(h, w, CV_8UC1, p);
//		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerGR2BGR);
//		return true;
//	}*/
//
//	// --- 5) Các định dạng còn lại (Bayer10/12/16, Mono10/12/16, Packed, v.v.) → dùng SDK EX
//	return ConvertBySDKEx(camera, out, imageBGR);
//}
bool CaptureFrame(CMvCamera* camera, cv::Mat& imageBGR) {
	MV_FRAME_OUT out = { 0 };

	// Lấy buffer ảnh từ camera (timeout 1000 ms)
	int nRet = camera->GetImageBuffer(&out, 1000);
	if (nRet != MV_OK) {
		std::cerr << "Failed to grab image, error: " << nRet << std::endl;
		return false;
	}

	// RAII: luôn trả buffer về SDK khi ra khỏi scope
	struct BufferGuard {
		CMvCamera* cam; MV_FRAME_OUT* o;
		~BufferGuard() { if (cam && o) cam->FreeImageBuffer(o); }
	} guard{ camera, &out };

	const int  w = out.stFrameInfo.nWidth;
	const int  h = out.stFrameInfo.nHeight;
	const auto pix = out.stFrameInfo.enPixelType;
	void* p = out.pBufAddr;

	if (!p || w <= 0 || h <= 0) return false;

	// Tạo Mat "view" trỏ trực tiếp vào buffer của SDK (không copy)
	auto view8UC1 = [&](void* buf) { return cv::Mat(h, w, CV_8UC1, buf); };
	auto view8UC3 = [&](void* buf) { return cv::Mat(h, w, CV_8UC3, buf); };

	switch (pix) {
	case PixelType_Gvsp_BGR8_Packed: {
		// Đã là BGR8: copy ra imageBGR để dùng ngoài phạm vi buffer SDK
		cv::Mat bgrView = view8UC3(p);
		imageBGR = bgrView.clone();
		return true;
	}
	case PixelType_Gvsp_Mono8: {
		cv::Mat mono = view8UC1(p);
		cv::cvtColor(mono, imageBGR, cv::COLOR_GRAY2BGR);
		return true;
	}
	case PixelType_Gvsp_Mono12: {
		// Nếu SDK có hàm convert, ưu tiên dùng (ví dụ MV_CC_ConvertPixelType…)
		// Ở đây minh hoạ nén 12-bit về 8-bit bằng dịch phải 4 bit.
		// LƯU Ý: Nếu là "Mono12Packed", bố trí bit khác -> cần giải pack đúng theo SDK!
		const int bytes = w * h * 2; // giả định mỗi pixel 16-bit chứa 12-bit hữu dụng
		cv::Mat mono16(h, w, CV_16UC1, p);
		cv::Mat mono8;
		mono16.convertTo(mono8, CV_8U, 1.0 / 16.0); // >>4
		cv::cvtColor(mono8, imageBGR, cv::COLOR_GRAY2BGR);
		return true;
	}
	case PixelType_Gvsp_BayerBG8: {
		cv::Mat raw = view8UC1(p);
		cv::cvtColor(raw, imageBGR,cv::COLOR_BayerBG2RGB);//hik
		return true;
	}
	case PixelType_Gvsp_BayerGB8: {
		cv::Mat raw = view8UC1(p);
		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerGB2BGR);
		return true;
	}
	case PixelType_Gvsp_BayerRG8: {
		cv::Mat raw = view8UC1(p);
		cv::cvtColor(raw, imageBGR,cv::COLOR_BayerRG2BGR);
		return true;
	}
	case PixelType_Gvsp_BayerGR8: {
		cv::Mat raw = view8UC1(p);
		cv::cvtColor(raw, imageBGR,  cv::COLOR_BayerGR2BGR);
		return true;
	}
	default:
		std::cerr << "Unsupported pixel format: " << pix << std::endl;
		return false;
	}
}
void  CCD::SetFormatImage(int Format)
{
	FormatCCD = Format;
}
struct MvFrameGuard
{
	CMvCamera* cam;
	MV_FRAME_OUT* frame;

	MvFrameGuard(CMvCamera* c, MV_FRAME_OUT* f) : cam(c), frame(f) {}
	~MvFrameGuard()
	{
		if (cam && frame) cam->FreeImageBuffer(frame);
	}
};
//uchar* CCD::ReadCCD(int indexCCD, int* rows, int* cols, int* Type)
//{
//	auto t0 = std::chrono::steady_clock::now();
//	frameCount++;
//
//	cv::Mat rawBGR;
//
//	switch (TypeCamera)
//	{
//	case 1: { // Camera SDK
//		if (IsWaiting)
//		{
//			return nullptr;
//		}
//		//auto t0 = std::chrono::steady_clock::now();
//		frameCount++;
//
//		*rows = *cols = *Type = 0;
//
//		if (IsWaiting || !m_pcMyCamera[indexCCD])
//			return nullptr;
//
//		MV_FRAME_OUT frameOut{};
//		if (m_pcMyCamera[indexCCD]->GetImageBuffer(&frameOut, 1000) != MV_OK)
//			return nullptr;
//
//		// auto FreeImageBuffer
//		MvFrameGuard guard(m_pcMyCamera[indexCCD], &frameOut);
//
//		auto& info = frameOut.stFrameInfo;
//		if (!frameOut.pBufAddr || info.nWidth == 0 || info.nHeight == 0)
//			return nullptr;
//
//		// ===== CHỈ CHẤP NHẬN MONO8 =====
//		if (info.enPixelType != PixelType_Gvsp_Mono8)
//			
//			return nullptr;
//
//		const int w = info.nWidth;
//		const int h = info.nHeight;
//		const size_t imageSize = (size_t)w * h;
//
//		uchar* image_uchar = new uchar[imageSize];
//
//		// Copy 1 lần duy nhất
//		std::memcpy(image_uchar, frameOut.pBufAddr, imageSize);
//
//		// ===== FPS =====
//		auto now = std::chrono::steady_clock::now();
//		elapsed = now - lastTime;
//		if (elapsed.count() >= 1.0)
//		{
//			FPS = frameCount / elapsed.count();
//			frameCount = 0;
//			lastTime = now;
//		}
//		cycle = int(std::chrono::duration_cast<std::chrono::milliseconds>(now - t0).count());
//
//		*rows = h;
//		*cols = w;
//		*Type = CV_8UC1;
//
//		return image_uchar;
//		//return ConvertBySDKEx(camera, out, imageBGR);
//		//if (!CaptureFrameMat(m_pcMyCamera[indexCCD], rawBGR)) {
//		//	//DeviceReset(m_pcMyCamera[indexCCD]);
//		//	*rows = *cols = *Type = 0;
//		//	return nullptr;
//		//}
//		break;
//	}
//	default: { // OpenCV VideoCapture
//		// grab/retrieve để giảm latency
//		if (!camUSB.grab() || !camUSB.retrieve(rawBGR)) {
//			*rows = *cols = *Type = 0;
//			return nullptr;
//		}
//		// Đảm bảo ảnh về BGR 8-bit nếu camera trả YUYV/GRAY…
//		if (rawBGR.type() == CV_8UC1) {
//			cv::cvtColor(rawBGR, rawBGR, cv::COLOR_GRAY2BGR);
//		}
//		break;
//	}
//	}
//
//	// Cập nhật FPS mỗi ~1s
//	auto now = std::chrono::steady_clock::now();
//	elapsed = now - lastTime;
//	if (elapsed.count() >= 1.0) {
//		FPS = frameCount / elapsed.count();
//		frameCount = 0;
//		lastTime = now;
//	}
//
//	cycle = int(std::chrono::duration_cast<std::chrono::milliseconds>(now - t0).count());
//
//	*rows = rawBGR.rows;
//	*cols = rawBGR.cols;
//	*Type = rawBGR.type();
//
//	if (rawBGR.empty()) {
//		*rows = *cols = *Type = 0;
//		return nullptr;
//	}
//
//	// Xuất dữ liệu dạng bytes (copy 1 lần để caller sở hữu bộ nhớ)
//	const size_t image_size = rawBGR.total() * rawBGR.elemSize();
//	uchar* image_uchar = new uchar[image_size];
//	std::memcpy(image_uchar, rawBGR.data, image_size);
//	return image_uchar;
//}
uchar* CCD::ReadCCD(int indexCCD, int* rows, int* cols, int* Type)
{
	auto t0 = std::chrono::steady_clock::now();
	*rows = *cols = *Type = 0;

	if (IsWaiting || !m_pcMyCamera[indexCCD])
		return nullptr;

	MV_FRAME_OUT frameOut{};
	if (m_pcMyCamera[indexCCD]->GetImageBuffer(&frameOut, 1000) != MV_OK)
		return nullptr;

	MvFrameGuard guard(m_pcMyCamera[indexCCD], &frameOut);

	auto& info = frameOut.stFrameInfo;
	if (!frameOut.pBufAddr || info.nWidth == 0 || info.nHeight == 0)
		return nullptr;

	const int w = info.nWidth;
	const int h = info.nHeight;

	uchar* image_uchar = nullptr;

	// =====================================================
	// 1️⃣ Nếu là Mono8 → copy trực tiếp
	// =====================================================
	if (info.enPixelType == PixelType_Gvsp_Mono8)
	{
		size_t imageSize = (size_t)w * h;
		image_uchar = new uchar[imageSize];

		std::memcpy(image_uchar, frameOut.pBufAddr, imageSize);

		*rows = h;
		*cols = w;
		*Type = CV_8UC1;
	}
	// =====================================================
	// 2️⃣ Nếu KHÔNG phải Mono8 → convert sang BGR
	// =====================================================
	else
	{
		cv::Mat dstBGR;

		if (!ConvertBySDKEx(m_pcMyCamera[indexCCD], frameOut, dstBGR))
			return nullptr;

		size_t imageSize = (size_t)w * h * 3;
		image_uchar = new uchar[imageSize];

		std::memcpy(image_uchar, dstBGR.data, imageSize);

		*rows = h;
		*cols = w;
		*Type = CV_8UC3;
	}

	// ================= FPS =================
	frameCount++;
	auto now = std::chrono::steady_clock::now();
	elapsed = now - lastTime;

	if (elapsed.count() >= 1.0)
	{
		FPS = frameCount / elapsed.count();
		frameCount = 0;
		lastTime = now;
	}

	cycle = int(std::chrono::duration_cast<std::chrono::milliseconds>(now - t0).count());

	return image_uchar;
}
Mat equalizeBGRA(const Mat& img)
{
	Mat res(img.size(), img.type());
	Mat imgB(img.size(), CV_8UC1);
	Mat imgG(img.size(), CV_8UC1);
	Mat imgR(img.size(), CV_8UC1);
	Vec3b pixel;



	for (int r = 0; r < img.rows; r++)
	{
		for (int c = 0; c < img.cols; c++)
		{
			pixel = img.at<Vec3b>(r, c);
			imgB.at<uchar>(r, c) = pixel[0];
			imgG.at<uchar>(r, c) = pixel[1];
			imgR.at<uchar>(r, c) = pixel[2];
		}
	}

	equalizeHist(imgB, imgB);
	equalizeHist(imgG, imgG);
	equalizeHist(imgR, imgR);

	for (int r = 0; r < img.rows; r++)
	{
		for (int c = 0; c < img.cols; c++)
		{
			pixel = Vec3b(imgB.at<uchar>(r, c), imgG.at<uchar>(r, c), imgR.at<uchar>(r, c));
			res.at<Vec3b>(r, c) = pixel;
		}
	}

	return res;
}
//void CCD::ReadRaw(bool IsHist)
//{
//	double d1 = clock();
//	Mat raw = Mat();
//	camUSB.read(raw);
//
//	camUSB >> raw;
//	if (IsHist)
//		matRaw = equalizeBGRA(raw);
//	else
//		matRaw = raw;
//	if (matRaw.empty() || matRaw.cols == 0 || matRaw.rows == 0)
//		numERR++;
//
//	if (numERR > 5)
//	{
//		numERR = 0;
//		IsErrCCD = true;
//	}
//	cycle = int(clock() - d1);
//}


void CCD::CalHist()
{
	Mat src = matRaw.clone();
	
	//! [Load image]

	//! [Separate the image in 3 places ( B, G and R )]
	vector<Mat> bgr_planes;
	split(src, bgr_planes);
	//! [Separate the image in 3 places ( B, G and R )]

	//! [Establish the number of bins]
	int histSize = 256;
	//! [Establish the number of bins]

	//! [Set the ranges ( for B,G,R) )]
	float range[] = { 0, 256 }; //the upper boundary is exclusive
	const float* histRange[] = { range };
	//! [Set the ranges ( for B,G,R) )]

	//! [Set histogram param]
	bool uniform = true, accumulate = false;
	//! [Set histogram param]

	//! [Compute the histograms]
	Mat b_hist, g_hist, r_hist;
	calcHist(&bgr_planes[0], 1, 0, Mat(), b_hist, 1, &histSize, histRange, uniform, accumulate);
	calcHist(&bgr_planes[1], 1, 0, Mat(), g_hist, 1, &histSize, histRange, uniform, accumulate);
	calcHist(&bgr_planes[2], 1, 0, Mat(), r_hist, 1, &histSize, histRange, uniform, accumulate);
	//! [Compute the histograms]

	//! [Draw the histograms for B, G and R]
	int hist_w = 512, hist_h = 400;
	int bin_w = cvRound((double)hist_w / histSize);

	Mat histImage(hist_h, hist_w, CV_8UC3, Scalar(0, 0, 0));
	//! [Draw the histograms for B, G and R]

	//! [Normalize the result to ( 0, histImage.rows )]
	normalize(b_hist, b_hist, 0, histImage.rows, NORM_MINMAX, -1, Mat());
	normalize(g_hist, g_hist, 0, histImage.rows, NORM_MINMAX, -1, Mat());
	normalize(r_hist, r_hist, 0, histImage.rows, NORM_MINMAX, -1, Mat());
	//! [Normalize the result to ( 0, histImage.rows )]

	//! [Draw for each channel]
	for (int i = 1; i < histSize; i++)
	{
		line(histImage, cv::Point(bin_w * (i - 1), hist_h - cvRound(b_hist.at<float>(i - 1))),
			cv::Point(bin_w * (i), hist_h - cvRound(b_hist.at<float>(i))),
			Scalar(255, 0, 0), 2, 8, 0);
		line(histImage, cv::Point(bin_w * (i - 1), hist_h - cvRound(g_hist.at<float>(i - 1))),
			cv::Point(bin_w * (i), hist_h - cvRound(g_hist.at<float>(i))),
			Scalar(0, 255, 0), 2, 8, 0);
		line(histImage, cv::Point(bin_w * (i - 1), hist_h - cvRound(r_hist.at<float>(i - 1))),
			cv::Point(bin_w * (i), hist_h - cvRound(r_hist.at<float>(i))),
			Scalar(0, 0, 255), 2, 8, 0);
	}
	//! [Draw for each channel]

	//! [Display]
	imshow("Source image", equalizeBGRA(matRaw.clone()));
	imshow("calcHist Demo", histImage);


}
bool CCD::ReconnectHik(int indexCCD,System::String^ NameCamera, int waitAfterResetMs )
{
	

	
	// ===== 1) Stop grabbing nếu còn sống =====
	m_pcMyCamera[indexCCD]->StopGrabbing();

	// ===== 2) Thử DeviceReset (soft reset) =====
	int r = m_pcMyCamera[indexCCD]->CommandExecute("DeviceReset");
	if (r == MV_OK)
	{
		std::cout << "[Reconnect] DeviceReset OK, wait reboot..." << std::endl;
		std::this_thread::sleep_for(std::chrono::milliseconds(waitAfterResetMs));
	}
	else
	{
		std::cout << "[Reconnect] DeviceReset not supported / failed = " << r << std::endl;
	}

	// ===== 3) Close & Destroy handle cũ =====
	m_pcMyCamera[indexCCD]->Close();
	delete m_pcMyCamera[indexCCD];
	m_pcMyCamera[indexCCD] = nullptr;

	// ===== 4) Enum lại device =====
	memset(&m_stDevList, 0, sizeof(MV_CC_DEVICE_INFO_LIST));
	int nRet = CMvCamera::EnumDevices(
		MV_GIGE_DEVICE | MV_USB_DEVICE | MV_GENTL_GIGE_DEVICE,
		&m_stDevList
	);
	if (nRet != MV_OK || m_stDevList.nDeviceNum == 0)
	{
		std::cerr << "[Reconnect] EnumDevices failed or no device\n";
		return false;
	}
	std::string nameCCD = marshal_as<std::string>(NameCamera);
	int index = FindCameraIndexByUserName(m_stDevList, nameCCD);
	if (index <= -1)
		return false;
	bool IsConnect = ConnectHik(index, indexCCD);



	if (!IsConnect)
	{
		std::cerr << "[Reconnect] Open failed: " << nRet << std::endl;
		delete m_pcMyCamera[indexCCD];
		m_pcMyCamera[indexCCD] = nullptr;
		return false;
	}

	// ===== 7) GigE: set lại packet size =====
	if (m_stDevList.pDeviceInfo[index]->nTLayerType == MV_GIGE_DEVICE)
	{
		unsigned int packetSize = 0;
		if (m_pcMyCamera[indexCCD]->GetOptimalPacketSize(&packetSize) == MV_OK)
		{
			m_pcMyCamera[indexCCD]->SetIntValue("GevSCPSPacketSize", packetSize);
		}
	}

	// ===== 8) Start grabbing =====
	m_pcMyCamera[indexCCD]->StartGrabbing();

	std::cout << "[Reconnect] Reconnect SUCCESS\n";
	return true;
}

void CCD::DestroyAll(int indexCCD,int TypeCamera )
{
	
	switch (TypeCamera)
	{
	case 0 :
		camUSB.release();
		destroyAllWindows();
		break;
	case 1:
		if (m_pcMyCamera[indexCCD] != NULL)
		{
			m_pcMyCamera[indexCCD]->StopGrabbing();
			m_pcMyCamera[indexCCD]->Close();
			m_pcMyCamera[indexCCD]->FinalizeSDK();
			/*baslerGigE.StopGrabbing();
			baslerGigE.Close();
			baslerGigE.DetachDevice();*/

			cvDestroyAllWindows();
		}
		break;
	default:
		break;
	}
	
}
bool IsShow = false;
void CCD::ShowSetting()
{
	IsShow = !IsShow;
	
	if(IsShow)
camUSB.set(CAP_PROP_SETTINGS, 1);
	else
		destroyAllWindows();
}