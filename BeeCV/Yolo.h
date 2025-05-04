#include "G.h"



#pragma once
namespace CvPlus
{
	
	public ref   class Yolo
	{
	public:  float Cycle;

	public: bool LoadModel(System::String^ nameTool, System::String^ nameModel, int Type);
	public:  System::String^ Check(System::String^ nameTool, float Score);
	};
}

