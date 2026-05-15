#include "PinPitchCli.h"
#include <cstring>

using namespace BeeCppCli;

static PinCenterMethodCli ToCliMethod(BeeCpp::PinCenterMethod method)
{
    return method == BeeCpp::PinCenterMethod::Geometry
        ? PinCenterMethodCli::Geometry
        : PinCenterMethodCli::WeightedCentroidFallback;
}

PinPitchCli::PinPitchCli() : _core(new BeeCpp::PinPitchCore()) {}
PinPitchCli::~PinPitchCli() { delete _core; _core = nullptr; }

void PinPitchCli::SetImage(IntPtr data, int width, int height, int stride, int channels)
{
    if (data == IntPtr::Zero || width <= 0 || height <= 0 || stride <= 0)
    {
        _core->setImage(cv::Mat());
        return;
    }

    int type = CV_8UC1;
    if (channels == 3)
        type = CV_8UC3;
    else if (channels == 4)
        type = CV_8UC4;

    cv::Mat src(height, width, type, data.ToPointer(), (size_t)stride);
    cv::Mat owned;
    src.copyTo(owned);
    _core->setImage(owned);
}

void PinPitchCli::SetOptions(
    int expectedCount,
    PinArrangeModeCli arrangeMode,
    bool useProjectedPitch,
    bool useAutoThreshold,
    int manualThreshold,
    int morphClose,
    int morphOpen,
    double minAreaPx,
    double maxAreaRatio,
    double minAspect,
    double maxAspect,
    double minFillRatio,
    bool useOutlineCenter,
    int outlineThresholdOffset,
    int outlineClose,
    int outlineDilate,
    int outlinePadding,
    int maxOutlineExpand,
    double mmPerPixel,
    bool useTopHat,
    int topHatKernelPx,
    double minSolidity,
    bool reduceDilateForOutline,
    bool useEdgeBoundary,
    int edgeCannyLow,
    int edgeCannyHigh,
    bool useEdgeGeometryCenter,
    bool useGradientRefinement,
    int gradientPatchMargin,
    int gradientThreshold,
    double claheClipLimit,
    int claheTileSize)
{
    BeeCpp::PinPitchOptions options;
    options.expectedCount = expectedCount;
    switch (arrangeMode)
    {
    case PinArrangeModeCli::Y:
        options.arrangeMode = BeeCpp::PinArrangeMode::Y;
        break;
    case PinArrangeModeCli::RowProjection:
        options.arrangeMode = BeeCpp::PinArrangeMode::RowProjection;
        break;
    default:
        options.arrangeMode = BeeCpp::PinArrangeMode::X;
        break;
    }
    options.useProjectedPitch = useProjectedPitch;
    options.useAutoThreshold = useAutoThreshold;
    options.manualThreshold = manualThreshold;
    options.morphClose = morphClose;
    options.morphOpen = morphOpen;
    options.minAreaPx = minAreaPx;
    options.maxAreaRatio = maxAreaRatio;
    options.minAspect = minAspect;
    options.maxAspect = maxAspect;
    options.minFillRatio = minFillRatio;
    options.useOutlineCenter = useOutlineCenter;
    options.outlineThresholdOffset = outlineThresholdOffset;
    options.outlineClose = outlineClose;
    options.outlineDilate = outlineDilate;
    options.outlinePadding = outlinePadding;
    options.maxOutlineExpand = maxOutlineExpand;
    options.mmPerPixel = mmPerPixel;
    options.useTopHat = useTopHat;
    options.topHatKernelPx = topHatKernelPx;
    options.minSolidity = minSolidity;
    options.reduceDilateForOutline = reduceDilateForOutline;
    options.useEdgeBoundary = useEdgeBoundary;
    options.edgeCannyLow = edgeCannyLow;
    options.edgeCannyHigh = edgeCannyHigh;
    options.useEdgeGeometryCenter = useEdgeGeometryCenter;
    options.useGradientRefinement = useGradientRefinement;
    options.gradientPatchMargin = gradientPatchMargin;
    options.gradientThreshold = gradientThreshold;
    options.claheClipLimit = claheClipLimit;
    options.claheTileSize = claheTileSize;
    _core->setOptions(options);
}

PinPitchCliResult^ PinPitchCli::Measure()
{
    BeeCpp::PinPitchResultCore result = _core->measure();
    auto managed = gcnew PinPitchCliResult();

    managed->Found = result.found;
    managed->Status = result.status;
    managed->Message = gcnew System::String(result.message.c_str());
    managed->ScaleMmPerPx = result.scaleMmPerPx;
    managed->SpanP1P4Mm = result.spanP1P4Mm;
    managed->RowVx = result.rowLine[0];
    managed->RowVy = result.rowLine[1];
    managed->RowX0 = result.rowLine[2];
    managed->RowY0 = result.rowLine[3];
    managed->RowResidualPx = result.rowResidualPx;

    managed->Pins = gcnew array<PinCenterCli^>((int)result.pins.size());
    for (int i = 0; i < (int)result.pins.size(); ++i)
    {
        const auto& pin = result.pins[i];
        auto p = gcnew PinCenterCli();
        p->Id = pin.id;
        p->X = pin.center.x;
        p->Y = pin.center.y;
        p->Score = pin.score;
        p->AreaPx = pin.areaPx;
        p->WidthPx = pin.box.size.width;
        p->HeightPx = pin.box.size.height;
        p->FillRatio = pin.fillRatio;
        p->AngleDeg = pin.box.angle;
        p->Method = ToCliMethod(pin.method);
        managed->Pins[i] = p;
    }

    managed->AdjacentPitchMm = gcnew array<double>((int)result.adjacentPitchMm.size());
    for (int i = 0; i < (int)result.adjacentPitchMm.size(); ++i)
        managed->AdjacentPitchMm[i] = result.adjacentPitchMm[i];

    if (!result.debugBGR.empty())
    {
        const int bytes = (int)(result.debugBGR.step * result.debugBGR.rows);
        unsigned char* buffer = (unsigned char*)::operator new[](bytes);
        std::memcpy(buffer, result.debugBGR.data, bytes);
        managed->DebugPtr = IntPtr((void*)buffer);
        managed->DebugW = result.debugBGR.cols;
        managed->DebugH = result.debugBGR.rows;
        managed->DebugStride = (int)result.debugBGR.step;
        managed->DebugChannels = result.debugBGR.channels();
    }
    else
    {
        managed->DebugPtr = IntPtr::Zero;
        managed->DebugW = 0;
        managed->DebugH = 0;
        managed->DebugStride = 0;
        managed->DebugChannels = 0;
    }

    return managed;
}

void PinPitchCli::FreeBuffer(IntPtr p)
{
    if (p != IntPtr::Zero)
        ::operator delete[](p.ToPointer());
}
