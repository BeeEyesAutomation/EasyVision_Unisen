#include "CCD.h"
#include <iostream>
#include <string>
#include <locale>
#include <codecvt>
#include <atlstr.h> // Cần thư viện ATL/MFC


using namespace CvPlus; 
using namespace System::Runtime::InteropServices;


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
//Pylon::String_t a="";
//static const size_t c_maxCamerasToUse = 2;
//DeviceInfoList_t devices;
//System::String^ ScanBasler()
//{
//	
//	PylonInitialize();
//	// Get the transport layer factory.
//	CTlFactory& tlFactory = CTlFactory::GetInstance();
//
//	// Get all attached devices and exit application if no device is found.
//
//	if (tlFactory.EnumerateDevices(devices) == 0)
//	{
//		return "";
//	}
//	CInstantCamera camera(CTlFactory::GetInstance().CreateFirstDevice());
//	// Create an array of instant cameras for the found devices and avoid exceeding a maximum number of devices.
//	//CInstantCameraArray cameras(min(devices.size(), c_maxCamerasToUse));
//	std::string list1 = "";
//	// Create and attach all Pylon Devices.
//	for (size_t i = 0; i < devices.size(); ++i)
//	{
//		//cameras[i].Attach(tlFactory.CreateDevice(devices[i]));
//		list1.append(devices[i].GetUserDefinedName()).append("$").append(devices[i].GetModelName()).append("$").append(devices[i].GetSerialNumber()).append("\n");
//		// Print the model name of the camera.
//		//cout << "Using device " << cameras[i].GetDeviceInfo().GetModelName() << endl;
//	}
//
//	return gcnew  System::String(list1.c_str());;
//	
//}
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
// static bool ConnectBasler(int rowCCD, int colCCD, int index)
//{
//	if (index == -1)return false;
//	try
//	{
//		
//		//PylonInitialize();
//	
//		CTlFactory& tlFactory = CTlFactory::GetInstance();
//		if (devices[index].GetFullName() != "")// đã nhận dc tên ccd
//		{
//			try
//			{
//				baslerGigE.Attach(tlFactory.CreateDevice(devices[index]));//kt quyền điều khiển
//				baslerGigE.Open();
//				GenApi::CBooleanPtr ptrAutoPacketSize = baslerGigE.GetStreamGrabberNodeMap().GetNode("AutoPacketSize");
//				if (GenApi::IsWritable(ptrAutoPacketSize))
//				{
//					ptrAutoPacketSize->SetValue(true);
//				}
//				baslerGigE.Width.SetValue(colCCD);
//				baslerGigE.Height.SetValue(rowCCD);
//				int with = (int)baslerGigE.Width.GetMax();
//				int height = (int)baslerGigE.Height.GetMax();
//				baslerGigE.CenterX =true;
//				baslerGigE.CenterX =true;
//				
//				//baslerGigE.ExposureTimeAbs.SetValue(10000);
//				//_minOffsetX = (int)camera.OffsetX.GetMin();
//				//baslerGigE.OffsetX.SetValue(_xCenter);
//				//baslerGigE.OffsetY.SetValue(_yCenter);
//				//baslerGigE.ExposureTime.SetValue(_exporsure);
//				//camera.Gamma.SetValue(3.05);
//				//camera.BlackLevel.SetValue(0);
//				//Lamp(true);
//				//IO(true);
//				//minExplosure = (int)camera.ExposureTimeRaw.GetMin();
//				//maxGain = (double)camera.Gamma.GetMax()*100.0;
//				//maxSetX = (int)camera.OffsetX.GetMax();
//				//maxSetY = (int)camera.OffsetY.GetMax();
//				//WriteParaCCD();//set thong so ccd
//
//				//	_StepExposure = (int)camera.ExposureTime.GetInc();
//
//				//_maxblack = (int)camera.BlackLevel.GetMax();
//				//_minblack = (int)camera.BlackLevel.GetMin();
//
//				//_maxGain = (double)camera.Gain.GetMax();
//				//_minGain = (double)camera.Gain.GetMin();
//
//				//_maxExposure = (double)camera.ExposureTime.GetMax();
//				//_minExposure = (double)camera.ExposureTime.GetMin();
//
//				
//				//_minOffsetY = (int)camera.OffsetY.GetMin();
//				//_maxOffsetY = (int)camera.OffsetY.GetMax();
//				//_maxWidth = (int)baslerGigE.Width.GetMax();
//				//_minWidth = (int)camera.Width.GetMin();
//
//				//_maxHeight = (int)camera.Height.GetMax();
//				//_minHeight = (int)camera.Height.GetMin();
//				////_StepGamma =(int) camera.Gamma.GetInc();
//				//_StepWidth = (int)camera.Width.GetInc();
//				//_StepHeight = (int)camera.Height.GetInc();
//
//				//_StepOffsetX = (int)camera.OffsetX.GetInc();
//				//_StepOffsetY = (int)camera.OffsetY.GetInc();
//				baslerGigE.StartGrabbing();//Tao luong Doc anh
//
//				fc.OutputPixelFormat = PixelType_Mono8;
//
//				baslerGigE.RetrieveResult(-1, ptrGrabResult, TimeoutHandling_ThrowException);//Lay Data Camera SAU KHOẢNG THỜI GIAN SẼ THOÁT RA ,(NẾU GIÁ TRỊ BẰNG -1 KHÔNG THOÁT RA)
//				if (ptrGrabResult->GrabSucceeded())
//				{
//					fc.Convert(image, ptrGrabResult);///Chuyen gia tri ma camera qua anh thu viện Pylon Balser
//					matRaw = cv::Mat(ptrGrabResult->GetHeight(), ptrGrabResult->GetWidth(), CV_8UC1, (uint8_t*)image.GetBuffer(), Mat::AUTO_STEP);///convert anh thu vien pylon thanh Mat			
//					
//					
//
//				}
//				ptrGrabResult.Release();//Xoa data
//				baslerGigE.StopGrabbing();
//				return true;
//				//	
//
//
//			}
//			catch (GenICam::GenericException& e)
//			{
//
//
//				baslerGigE.StopGrabbing();
//				baslerGigE.Close();
//				baslerGigE.DetachDevice();
//
//				//PylonTerminate();
//			}
//		}
//	}
//	catch (GenICam::GenericException& e)
//	{
//
//		baslerGigE.StopGrabbing();
//		baslerGigE.Close();
//		baslerGigE.DetachDevice();
//		PylonTerminate();
//	}
//	return false;
//}
int CCD::GetTypeCCD(int indexCCD)
{
	return listTypeCCD[indexCCD];
}
float	CCD::SetPara( int indexCCD, System::String^ Namepara, float Value)
{
	if (m_pcMyCamera[indexCCD] == nullptr)
		return -1;


	m_pcMyCamera[indexCCD]->StopGrabbing();
	//TypeCCD = listTypeCCD[indexCCD];
	std::string namepara = marshal_as<std::string>(Namepara);
	MVCC_INTVALUE_EX expInfo = {};
	if (m_pcMyCamera[indexCCD]->GetIntValue(namepara.c_str(), &expInfo) != MV_OK)
	{
		std::cerr << "❌ Không đọc được thông tin ExposureTime" << std::endl;
		return -1;
	}

	float minVal = expInfo.nMin;
	float maxVal = expInfo.nMax;
	float step = expInfo.nInc > 0 ? expInfo.nInc : 1.0f;  // tránh step = 0

	// Clamp về khoảng hợp lệ
	Value = std::max(minVal, std::min(maxVal, Value));

	// Làm tròn theo step
	float adjustedExposure = std::round((Value - minVal) / step) * step + minVal;

	// Set giá trị
	if (m_pcMyCamera[indexCCD]->SetIntValue(namepara.c_str(), (int)adjustedExposure) == MV_OK)
	{
		m_pcMyCamera[indexCCD]->StartGrabbing();
		return Value;
	}
	else
	{
		m_pcMyCamera[indexCCD]->StartGrabbing();
		return -1;
		std::cerr << "❌ Set ExposureTime thất bại!" << std::endl;
	}
	m_pcMyCamera[indexCCD]->StartGrabbing();
	return -1;
	/*int nRet = MV_CC_SetFloatValue(m_pcMyCamera[indexCCD], namepara.c_str(), Value);
	if (MV_OK != nRet)
		return false;
	else
		return true;

	return false;*/
}

bool	CCD::GetPara(int indexCCD, System::String^ Namepara, float% min,  float% max,  float% step, float% current)
{
	
		std::string namepara = marshal_as<std::string>(Namepara);
	float value = -1;
	MVCC_INTVALUE_EX expInfo = {};
	if (m_pcMyCamera[indexCCD]->GetIntValue(namepara.c_str(), &expInfo) != MV_OK)
	{
		
		std::cerr << "❌ Không đọc được thông tin ExposureTime" << std::endl;
		return false;
	}
	else
	{
		current =expInfo.nCurValue;
		step = expInfo.nInc;
		min = expInfo.nMin;
		max = expInfo.nMax;
	}	

	/*if (m_pcMyCamera[indexCCD] == nullptr)
		return -1;
	MVCC_ENUMVALUE enumVal = {};
	if (m_pcMyCamera[indexCCD]->GetEnumValue(namepara.c_str(), &enumVal) == MV_OK)
	{
		value = static_cast<double>(enumVal.nCurValue);	
	}*/
	return true;
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
void CCD::AutoFocus()
{
	camUSB.set(CAP_PROP_AUTOFOCUS, 0);
	camUSB.set(CAP_PROP_AUTOFOCUS, 1);

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
bool GetFrame(cv::Mat& image, void* handle) {
	MVCC_ENUMVALUE stPixelFormat;
	memset(&stPixelFormat, 0, sizeof(MVCC_ENUMVALUE));

	// Lấy định dạng ảnh từ camera
	if (MV_OK != MV_CC_GetEnumValue(handle, "PixelFormat", &stPixelFormat)) {
		std::cerr << "Failed to get pixel format." << std::endl;
		return false;
	}

	// Lấy ảnh từ camera
	MV_FRAME_OUT stImageOut = { 0 };
	int nRet = MV_CC_GetImageBuffer(handle, &stImageOut, 1000); // Timeout: 1000ms

	if (nRet != MV_OK) {
		std::cerr << "Failed to grab image, error: " << nRet << std::endl;
		return false;
	}

	int width = stImageOut.stFrameInfo.nWidth;
	int height = stImageOut.stFrameInfo.nHeight;
	void* pBufAddr = stImageOut.pBufAddr;

	// Kiểm tra định dạng ảnh và chuyển đổi về OpenCV Mat
	switch (stPixelFormat.nCurValue) {
	case PixelType_Gvsp_BGR8_Packed:
		image = cv::Mat(height, width, CV_8UC3, pBufAddr).clone();
		break;
	case PixelType_Gvsp_Mono8:
		image = cv::Mat(height, width, CV_8UC1, pBufAddr).clone();
		break;
	case PixelType_Gvsp_BayerRG8:
	case PixelType_Gvsp_BayerBG8:
	case PixelType_Gvsp_BayerGR8:
	case PixelType_Gvsp_BayerGB8:
	{
		cv::Mat rawImage(height, width, CV_8UC3, pBufAddr);
		cv::cvtColor(rawImage, image, cv::COLOR_BayerBG2BGR);  // Chỉnh lại COLOR_BayerXX2BGR theo format camera
	}
	break;
	default:
		std::cerr << "Unsupported pixel format: " << stPixelFormat.nCurValue << std::endl;
		return false;
	}

	return true;
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
	if ((index < 0) | (index >= MV_MAX_DEVICE_NUM))
	{
		
		return false;
	}

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
					//ShowErrorMsg(TEXT("Warning: Set Packet Size fail!"), nRet);
				}
			}
			else
			{
				return false;
				//ShowErrorMsg(TEXT("Warning: Get Packet Size fail!"), nRet);
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
		IsConnect = ConnectHik(index, indexCCD);
		/*try
		{

			StepExposure = (int)baslerGigE.ExposureTimeRaw.GetInc();
			MinExposure = (int)baslerGigE.ExposureTimeRaw.GetMin();
			MaxExposure = (int)baslerGigE.ExposureTimeRaw.GetMax();
			Exposure = (int)baslerGigE.ExposureTimeRaw.GetValue();
		}
		catch (exception ex)
		{

		}*/
		break;
	}

	case 0:
	{
		//*nt it = 0 std::find(listCCCD.begin(), listCCCD.end(), nameCCD);*/

		/*if (it != listCCCD.end()) {
			int index = std::distance(listCCCD.begin(), it);*/
			IsConnect = ConnectUsb(indexCCD);
	/*	}
		else {
			IsConnect = true;
		}*/

		break;
	}
	}

	return IsConnect;
}
auto lastTime = std::chrono::high_resolution_clock::now();
int frameCount = 0;
double fps = 0.0;
std::chrono::duration<double> elapsed;
//bool CaptureFrame(CMvCamera* camera, cv::Mat& image) {
//	MV_FRAME_OUT stImageOut = { 0 };
//	
//	// Lấy buffer ảnh từ camera
//	int nRet = camera->GetImageBuffer(&stImageOut, 1000); // Timeout: 1000ms
//	if (nRet != MV_OK) {
//		std::cerr << "Failed to grab image, error: " << nRet << std::endl;
//		return false;
//	}
//
//	int width = stImageOut.stFrameInfo.nWidth;
//	int height = stImageOut.stFrameInfo.nHeight;
//	int pixelType = stImageOut.stFrameInfo.enPixelType;
//	void* pBufAddr = stImageOut.pBufAddr;
//
//	// Xử lý định dạng ảnh
//	switch (pixelType) {
//	case PixelType_Gvsp_BGR8_Packed:
//		image = cv::Mat(height, width, CV_8UC3, pBufAddr);
//		break;
//	case PixelType_Gvsp_Mono8:
//		image = cv::Mat(height, width, CV_8UC1, pBufAddr);
//		break;
//	case PixelType_Gvsp_Mono12:
//		image = cv::Mat(height, width, CV_8UC1, pBufAddr);
//		break;
//	case PixelType_Gvsp_BayerRG8:
//		break;
//	case PixelType_Gvsp_BayerBG8:
//	{
//		if (pBufAddr == nullptr || width <= 0 || height <= 0)
//			return false;
//
//		cv::Mat rawImage2(height, width, CV_8UC1);
//		memcpy(rawImage2.data, pBufAddr, height * width);  // Sao chép bộ nhớ để tránh lỗi
//
//		if (rawImage2.empty())
//			return false;
//
//		
//		cv::cvtColor(rawImage2, image, cv::COLOR_BayerBG2RGB);  // hoặc COLOR_BayerBG2BGR tùy loại
//		
//		//cv::cvtColor(rawImage2, image, cv::COLOR_BayerBG2BGR); // Chỉnh lại `COLOR_BayerXX2BGR` nếu cần
//		break;
//	}
//	case PixelType_Gvsp_BayerGR8:
//		break;
//	case PixelType_Gvsp_BayerGB8:
//	{
//		//image = cv::Mat(height, width, CV_8UC3, pBufAddr);
//		cv::Mat rawImage(height, width, CV_8UC1, pBufAddr);
//		cv::cvtColor(rawImage, image, cv::COLOR_BayerGB2RGB); // Chỉnh lại `COLOR_BayerXX2BGR` nếu cần
//	}
//	break;
//	default:
//		image =Mat();
//		return false;
//	}
//
//	// Giải phóng buffer
//	camera->FreeImageBuffer(&stImageOut);
//	return true;
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
		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerBG2BGR);
		return true;
	}
	case PixelType_Gvsp_BayerGB8: {
		cv::Mat raw = view8UC1(p);
		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerGB2BGR);
		return true;
	}
	case PixelType_Gvsp_BayerRG8: {
		cv::Mat raw = view8UC1(p);
		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerRG2BGR);
		return true;
	}
	case PixelType_Gvsp_BayerGR8: {
		cv::Mat raw = view8UC1(p);
		cv::cvtColor(raw, imageBGR, cv::COLOR_BayerGR2BGR);
		return true;
	}
	default:
		std::cerr << "Unsupported pixel format: " << pix << std::endl;
		return false;
	}
}

uchar* CCD::ReadCCD(int indexCCD, int* rows, int* cols, int* Type)
{
	auto t0 = std::chrono::steady_clock::now();
	frameCount++;

	cv::Mat rawBGR;

	switch (TypeCamera)
	{
	case 1: { // Camera SDK
		if (!CaptureFrame(m_pcMyCamera[indexCCD], rawBGR)) {
			// Fail: trả nullptr + metadata = 0
			*rows = *cols = *Type = 0;
			return nullptr;
		}
		break;
	}
	default: { // OpenCV VideoCapture
		// grab/retrieve để giảm latency
		if (!camUSB.grab() || !camUSB.retrieve(rawBGR)) {
			*rows = *cols = *Type = 0;
			return nullptr;
		}
		// Đảm bảo ảnh về BGR 8-bit nếu camera trả YUYV/GRAY…
		if (rawBGR.type() == CV_8UC1) {
			cv::cvtColor(rawBGR, rawBGR, cv::COLOR_GRAY2BGR);
		}
		break;
	}
	}

	// Cập nhật FPS mỗi ~1s
	auto now = std::chrono::steady_clock::now();
	elapsed = now - lastTime;
	if (elapsed.count() >= 1.0) {
		FPS = frameCount / elapsed.count();
		frameCount = 0;
		lastTime = now;
	}

	cycle = int(std::chrono::duration_cast<std::chrono::milliseconds>(now - t0).count());

	*rows = rawBGR.rows;
	*cols = rawBGR.cols;
	*Type = rawBGR.type();

	if (rawBGR.empty()) {
		*rows = *cols = *Type = 0;
		return nullptr;
	}

	// Xuất dữ liệu dạng bytes (copy 1 lần để caller sở hữu bộ nhớ)
	const size_t image_size = rawBGR.total() * rawBGR.elemSize();
	uchar* image_uchar = new uchar[image_size];
	std::memcpy(image_uchar, rawBGR.data, image_size);
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
void CCD::DestroyAll(int indexCCD)
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