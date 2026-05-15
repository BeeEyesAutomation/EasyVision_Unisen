# Pattern2 Debug Baseline

## Purpose

This baseline is for comparing one true-positive image and one false-positive image before changing Pattern2 matching thresholds or keep/reject logic.

Use the existing `Pattern2StableConfig.DebugLog` path. P2-001 only adds observability:

- Match logic is unchanged.
- Thresholds are unchanged.
- Keep/reject decisions are unchanged.
- Debug logs now include candidate id, top-left point, size, scale, all existing score metrics, and keep reason.
- Debug mode writes source/template/candidate ROI images beside the log file.

## How To Enable

Configure the stable match call with:

```csharp
var cfg = new BeeCpp.Pattern2StableConfig(true);
cfg.DebugLog = true;
cfg.DebugLogPath = @"E:\Code\EasyVision_Unisen\pattern2_debug.txt";
```

Then run the same trained template against:

1. an OK frame where Pattern2 should match the real white part, and
2. a false-positive frame like the supplied screenshot where a diagonal/background rectangle is selected.

## Output

The text log is appended to `DebugLogPath`.

Images are saved to:

```text
<DebugLogPath without extension>_artifacts\
```

Expected files include:

- `source_raw.png`
- `source_feature.png`
- `source_gray_pre.png`
- `source_edge_mag.png`
- `source_edge_bin.png`
- `scale_000_tpl_feature.png`
- `scale_000_tpl_norm.png`
- `scale_000_tpl_grad.png`
- `scale_000_tpl_edge.png`
- `candidate_0000_scale_000_roi_gray.png`
- `candidate_0000_scale_000_roi_norm.png`
- `candidate_0000_scale_000_roi_grad.png`
- `candidate_0000_scale_000_roi_edge.png`

Candidate ROI images are capped at 64 candidates per `MatchStable()` call to keep debug runs bounded.

## What To Compare

For the true-positive and false-positive cases, compare these log fields:

- `candidate`
- `cx`, `cy`, `ptLT`, `size`, `ang`, `scale`
- `coarse`
- `raw`
- `rawEdge`
- `grad`
- `edgeIoU`
- `edgeRatio`
- `final`
- `keepStrong`, `keepNormal`, `keepRescue`, `keep`
- `reason`

The next implementation entry should use this baseline to identify which metric separates the real part from the diagonal/background false positive.

