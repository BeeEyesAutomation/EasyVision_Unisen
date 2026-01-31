#pragma once
#include <Global.h>

#using <System.dll>
using namespace System;

namespace BeeCpp
{




    public value struct MonoSegCliParams
    {
        int BgBlurK, OpenK, CloseK, Mode;
        bool UseBlackHat;
        int BlackHatK;
    };

    // NOTE: sửa MinSolidity -> MinFillRatio + SizeTol + PaperMinAreaFrac
    public value struct ChipExtractCliParams
    {
        int MinArea, MinW, MinH;
        float MinAspect;
        int VertKW, VertKH, OpenK;

        float MinFillRatio;      // NEW
        float SizeTol;           // NEW
        float PaperMinAreaFrac;  // NEW
    };

    public ref class MonoSegCli sealed
    {
    public:
        static int SegmentMonoLowContrast(
            IntPtr grayPtr, int w, int h, int step,
            [System::Runtime::InteropServices::Out] IntPtr% outMaskPtr,
            [System::Runtime::InteropServices::Out] int% outMaskStep,
            MonoSegCliParams p, bool IsHardNoise
        );

        // NEW: trả về paper + chips dưới dạng RectRotateCli[]
        static cli::array<RectRotateCli>^ ExtractPaperAndChipRects(
            IntPtr maskPtr, int w, int h, int step,
            ChipExtractCliParams p
        );

        // NEW: vẽ RectRotateCli[] lên ảnh color result (BGR)
        static void DrawRectRotateToColorImage(
            IntPtr basePtr, int w, int h, int baseStep,         // CV_8UC1
            cli::array<RectRotateCli>^ rects,
            [System::Runtime::InteropServices::Out] IntPtr% outColorPtr,
            [System::Runtime::InteropServices::Out] int% outColorStep
        );

        static void FreeBuffer(IntPtr ptr);
    };
}
