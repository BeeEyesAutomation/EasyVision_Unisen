#pragma once
#include "G.h"
namespace CvPlus {
	
	class NativeData {
	public:
	 Scalar colorTemp;
	vector<Scalar> listColor;
	Scalar lower;
	Scalar upper;
	};

    public ref class ColorArea
	{
	private:
		NativeData* native;  // pointer tới native
	public:	ColorArea() { native = new NativeData(); }
		~ColorArea() { delete native; }
	double H=0, S=0, V=0;
	public: void AddColor();
	public:void LoadTemp(System::String^ listColor);
	public:System::String^ SaveTemp();
	public:void  GetMask(const cv::Mat& mat,int iAreaPixel);
	public: int ScoreRS = 0;
	public:float cycle = 0;
	public:	int pxMathching = 0;
	public:	int StyleColor = 0;
	public:System::String^ GetColor(System::IntPtr buffer, int width, int height, int Step, int image_type, int x,int y);
	 bool GetLimitColor(Scalar color, int iAreaPixel);
	public: int SetColorArea(int iAreaPixel);
	public:bool Undo(int iAreaPixel);
	public:float	CheckColor( int iAreaPixel);
	
	};
}

