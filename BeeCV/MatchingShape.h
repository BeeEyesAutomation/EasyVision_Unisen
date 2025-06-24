#pragma once
#include "G.h"
namespace CvPlus {
	public ref class MatchingShape
	{

		//public:void LoadTemp();
		//public:void GetTemp();

	public:float  CheckShape(int threshold, int minArea, int method, bool Invert);
	public:float cycleTool = 0;
	};
}

