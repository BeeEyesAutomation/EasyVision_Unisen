#include "G.h"
#include <Windows.h>
#include <dshow.h>
#pragma comment(lib, "strmiids")
#include <msclr/marshal_cppstd.h>
// Bổ sung định nghĩa RGB8 nếu header SDK của bạn chưa có:
#ifndef PixelType_Gvsp_RGB8_Packed
#define PixelType_Gvsp_RGB8_Packed 0x02180021
#endif
#ifndef PixelType_Gvsp_BGR8_Packed
#define PixelType_Gvsp_BGR8_Packed 0x02180014
#endif
#ifndef PixelType_Gvsp_Mono8
#define PixelType_Gvsp_Mono8       0x01080001
#endif
#ifndef PixelType_Gvsp_BayerBG8
#define PixelType_Gvsp_BayerBG8    0x0318000B
#endif
#ifndef PixelType_Gvsp_BayerGB8
#define PixelType_Gvsp_BayerGB8    0x0318000A
#endif
#ifndef PixelType_Gvsp_BayerRG8
#define PixelType_Gvsp_BayerRG8    0x03180009
#endif
#ifndef PixelType_Gvsp_BayerGR8
#define PixelType_Gvsp_BayerGR8    0x03180008
#endif

using namespace System;
using namespace msclr::interop;
#include <map>
#include <string>
namespace CvPlus {
	struct Device {
		int id; // This can be used to open the device in OpenCV
		std::string devicePath;
		std::string deviceName; // This can be used to show the devices to the user
	};

	class DeviceEnumerator {

	public:

		DeviceEnumerator() = default;
		std::map<int, Device> getDevicesMap(const GUID deviceClass);
		std::map<int, Device> getVideoDevicesMap();
		std::map<int, Device> getAudioDevicesMap();

	private:

		std::string ConvertBSTRToMBS(BSTR bstr);
		std::string ConvertWCSToMBS(const wchar_t* pstr, long wslen);

	};
	public ref class CCD
	{
	
	public: System::String^ Ex = "";
	public: float Exposure = 0;
	public: double StepExposure = 0,MinExposure=1,MaxExposure=1000;
	public: 	int TypeCCD = 0;
	public: 	int FPS = 0;
	public: float cycle = 0;
	public: int numERR = 0;
	public: int TypeCamera = 0;
	public: bool  IsErrCCD = false;
	//public:int  colCCD = 1280, rowCCD = 720; //  colCCD = 240, rowCCD = 120; //
	public:int colCrop, rowCrop;
	public:uchar* ReadCCD(int indexCCD,  int* rows, int* cols, int* Type);
	//public:void  ReadRaw(bool IsHist);
	public:System::String^ ScanCCD();
	public:bool	Connect( int indeCCD, System::String^ NameCCD);
	public:float SetPara(int indexCCD, System::String^ Namepara, float Value);
	public:float SetParaFloat(int indexCCD, System::String^ Namepara, float Value);
	public:bool GetPara(int indexCCD, System::String^ Namepara, float% min,  float% max,  float% step,  float% current);
	public:bool GetParaFloat(int indexCCD, System::String^ Namepara, float% min, float% max, float% step, float% current);
	public:int GetTypeCCD(int indexCCD);
	public:void	DestroyAll(int indexCCD);
	public:void	ShowSetting();
	public:void CalHist();
	public:  void SetFocus(int Focus);
	public:void SetZoom(int Zoom);
	public:	  int GetZoom();
	public:	  int GetFocus();
	public:	  void AutoFocus(bool Auto);
	public:void SetWidth(int Value);
	public:void SetHeight(int Value);
	public:int GetWidth();
	public:int GetHeight();
	public:void SetFormatImage(int Format);
	
	};
}
