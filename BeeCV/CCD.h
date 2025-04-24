#include "G.h"
#include <Windows.h>
#include <dshow.h>
#pragma comment(lib, "strmiids")

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
	public: float Exposure = 0;
	public: double StepExposure = 0,MinExposure=1,MaxExposure=1000;
	public: 	int typeCCD = 0;
	public: 	int FPS = 0;
	public: float cycle = 0;
	public: int numERR = 0;
	public: bool  IsErrCCD = false;
	public:int  colCCD = 1280, rowCCD = 720; //  colCCD = 240, rowCCD = 120; //
	public:int colCrop, rowCrop;
	public:void  ReadCCD();
	//public:void  ReadRaw(bool IsHist);
	public:System::String^ ScanCCD();
	public:bool	Connect( int rowCCD, int colCCD, int index);
	public:bool	SetPara();
	public:void	DestroyAll();
	public:void	ShowSetting();
	public:void CalHist();
	
	};
}
