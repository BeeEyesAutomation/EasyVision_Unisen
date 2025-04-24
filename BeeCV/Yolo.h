#include "G.h"



#pragma once
namespace CvPlus
{
	
	public ref   class Yolo
	{
	public:  float Cycle;
	public: bool InitializeYolo();
	public: bool ClosePython();
	public: bool LoadModel(System::String^ nameModel, int Type);
	public:  System::String^ Check(float Score);
	};
}

