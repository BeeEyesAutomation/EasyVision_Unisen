#pragma once
#include <opencv2/imgproc/types_c.h>
#include <opencv2/core/utility.hpp>
#include <opencv2/core.hpp>
#include <opencv2/imgproc.hpp>
#include <opencv2/imgcodecs.hpp>
#include <vector>
#include <cmath>
#include <limits>


#include <thread>
#include <limits>
#include <algorithm>
// C++/CLI
using namespace cv;
using namespace std;
using namespace System;
using namespace System::Collections::Generic;

#include <Pattern.h>
namespace BeeCpp
{



  


	
    // ===== Wrapper managed =====
    public ref class Pattern2
    {
    public:
        Pattern2();
        ~Pattern2();
        !Pattern2();
		void SetRawNoCrop(IntPtr data, int w, int h, int stride, int ch);
        void SetImgeRaw(IntPtr data, int w, int h, int stride, int ch, RectRotateCli rr, Nullable<RectRotateCli> rrMask);
		void SetImgeSampleNoCrop(IntPtr data, int w, int h, int stride, int ch);
		System::IntPtr SetImgeSample(IntPtr data, int w, int h, int stride, int ch,  RectRotateCli rr, Nullable<RectRotateCli> rrMask, bool NoCrop,
			[System::Runtime::InteropServices::Out] int% outW,
			[System::Runtime::InteropServices::Out] int% outH,
			[System::Runtime::InteropServices::Out] int% outStride,
			[System::Runtime::InteropServices::Out] int% outChannels );
        void LearnPattern();
		void FreeBuffer(System::IntPtr p);
		// set validation / anti-false-positive options
		void SetMatchOptions(PatternMatchOptions opt);
 		PatternMatchOptions GetMatchOptions();
        List<Rotaterectangle>^ Match(
            bool m_bStopLayer1,
            double m_dToleranceAngle,
            double m_dTolerance1,
            double m_dTolerance2,
            double m_dScore,
            bool m_ckSIMD,
            bool m_ckBitwiseNot,
            bool m_bSubPixel,
            int m_iMaxPos,
            double m_dMaxOverlap,
            bool useMultiThread,
            int numThreads
        );
	
    private:
		CommonPlus* com;
        // === AUTO score stability (CÁCH 1) ===
        bool ValidateScoreStability(
            Img* img,
            const s_MatchParameter& mp,
            bool m_ckSIMD,
            bool m_ckBitwiseNot
        );

        Img* img;
		PatternMatchOptions m_opt;
    };
}
