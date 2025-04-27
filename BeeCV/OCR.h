#pragma once

#include "G.h"



#pragma once
namespace CvPlus
{

	public ref   class OCR
	{
	public:  float Cycle;
	public:bool SetModel();
	public:  System::String^ Find(float Score);
	};
}

