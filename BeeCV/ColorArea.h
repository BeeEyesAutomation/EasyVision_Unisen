#pragma once
#include "G.h"
namespace CvPlus {
	 Scalar  colorTemp;
	 vector<Scalar> listColor;

	 Scalar lower, upper;
	
    public ref class ColorArea
	{
		double H=0, S=0, V=0;
	public: void AddColor();
	public:void LoadTemp(System::String^ listColor);
	public:System::String^ SaveTemp();
	public:void  GetMask(Mat mat,int iAreaPixel);
	public: int ScoreRS = 0;
	public:float cycle = 0;
	public:	int pxMathching = 0;
	public:	int StyleColor = 0;
	public:System::String^ GetColor(int x,int y);
	 bool GetLimitColor(Scalar color, int iAreaPixel);
	public: int SetColorArea(int iAreaPixel);
	public:bool Undo(int iAreaPixel);
	public:float	CheckColor( int iAreaPixel);
	
	};
}

