#include "WidthCli.h"

using namespace System;
using namespace System::Drawing;

namespace BeeCpp {

    static BeeCpp::LineOrientation ToNative(LineOrientationCli o) {
        switch (o) {
        case LineOrientationCli::Horizontal: return BeeCpp::LineOrientation::Horizontal;
        case LineOrientationCli::Vertical:   return BeeCpp::LineOrientation::Vertical;
        default:                              return BeeCpp::LineOrientation::Any;
        }
    }
    static BeeCpp::GapExtremum ToNative(GapExtremumCli e) {
        switch (e) {
        case GapExtremumCli::Nearest:   return BeeCpp::GapExtremum::Nearest;
        case GapExtremumCli::Farthest:  return BeeCpp::GapExtremum::Farthest;
        case GapExtremumCli::Outermost: return BeeCpp::GapExtremum::Outermost;
        default:                         return BeeCpp::GapExtremum::Middle;
        }
    }
    static BeeCpp::SegmentStatType ToNative(SegmentStatTypeCli s) {
        switch (s) {
        case SegmentStatTypeCli::Shortest: return BeeCpp::SegmentStatType::Shortest;
        case SegmentStatTypeCli::Average:  return BeeCpp::SegmentStatType::Average;
        default:                           return BeeCpp::SegmentStatType::Longest;
        }
    }

    WidthCli::WidthCli() { core_ = new BeeCpp::WidthCore(); }
    WidthCli::~WidthCli() { this->!WidthCli(); }
    WidthCli::!WidthCli() { if (core_) { delete core_; core_ = nullptr; } }

    void WidthCli::SetMmPerPixel(double mmPerPixel) { core_->SetMmPerPixel(mmPerPixel); }
    void WidthCli::SetRansac(int iterations, double threshold) { core_->SetRansac(iterations, threshold); }

    void WidthCli::SetImgeRaw(IntPtr data, int width, int height, int stride, int channels) {
        core_->SetImageRaw(static_cast<const unsigned char*>(data.ToPointer()), width, height, stride, channels);
    }
    void WidthCli::SetImgeCrop(IntPtr data, int width, int height, int stride, int channels,
        float x, float y, float w, float h, float angleDeg) {
        core_->SetImageCrop(static_cast<const unsigned char*>(data.ToPointer()), width, height, stride, channels,
            x, y, w, h, angleDeg);
    }

    Line2DCli WidthCli::ToCli(const BeeCpp::Line2DCore& s) {
        Line2DCli o; o.Vx = s.vx; o.Vy = s.vy; o.X0 = s.x0; o.Y0 = s.y0; return o;
    }

    GapResultCli WidthCli::MeasureParallelGap(int numLines,
        GapExtremumCli extremum,
        LineOrientationCli orientation,
        SegmentStatTypeCli segStat,
        int minInlierRemain)
    {
        auto R = core_->MeasureParallelGap(numLines, ToNative(extremum), ToNative(orientation), ToNative(segStat), minInlierRemain);

        GapResultCli rc;
        // lines
        rc.Lines = gcnew array<Line2DCli>((int)R.lines.size());
        for (int i = 0; i < (int)R.lines.size(); ++i) rc.Lines[i] = ToCli(R.lines[i]);

        rc.LineA = ToCli(R.lineA);
        rc.LineB = ToCli(R.lineB);

        rc.LineMid = gcnew array<System::Drawing::Point>(2);
        rc.LineMid[0] = System::Drawing::Point(R.lineMid[0].x, R.lineMid[0].y);
        rc.LineMid[1] = System::Drawing::Point(R.lineMid[1].x, R.lineMid[1].y);

        rc.GapMinPx = R.gapMinPx;
        rc.GapMediumPx = R.gapMedPx;
        rc.GapMaxPx = R.gapMaxPx;

        rc.GapMinMm = R.gapMinMm;
        rc.GapMediumMm = R.gapMedMm;
        rc.GapMaxMm = R.gapMaxMm;

        rc.InlierRemain = R.inlierRemain;
        return rc;
    }

} // namespace BeeCppCli
