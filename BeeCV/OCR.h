#pragma once

#include "G.h"



#pragma once
namespace CvPlus
{

	public ref   class OCR
	{
	public:  float Cycle;
	public: bool Ini();
	public: bool Close();
	public:  System::String^ Find(float Score);
	};
}

