# Pattern2 False Match Map

## ID Rules

- Prefix: `P2`
- Format: `P2-xxx`
- Next ID: `P2-006`
- Reuse existing IDs.
- Do not duplicate IDs.

## Status values: `open` | `wip` | `partial` | `done` | `blocked`
## Risk values: `low` | `medium` | `high`

## Entries

| ID | Status | Risk | Area | Files | Depends on | Notes |
| --- | --- | --- | --- | --- | --- | --- |
| P2-001 | done | low | Debug dataset and replay harness | `Pattern/Pattern2.cpp`, `docs/Pattern2FalseMatchPlan/*` | - | `MatchStable()` debug mode now writes source/template/candidate ROI artifacts beside `DebugLogPath`, adds candidate id/top-left/size to logs, records mat stats, and documents the true-positive vs false-positive replay workflow in `DebugBaseline.md`. Build verified Release x64. |
| P2-002 | open | medium | False-positive shape validator | `Pattern/Pattern2.cpp`, `Pattern/Pattern2.h` | P2-001 | Extend `MatchStable()` validation so diagonal or rectangular background features cannot pass only by NCC. Candidate should be checked against template edge topology using edge mask overlap, contour count/area ratio, aspect/orientation consistency, and optional RANSAC parallel-edge centerline agreement. |
| P2-003 | open | medium | Adaptive threshold tuning | `Pattern/Pattern2.cpp`, `Pattern/Pattern2.h` | P2-001 | Review `BuildAutoGate`, `ApplyDifficultyToScaleTemplate`, and `RelaxedRawScore`. Keep coarse threshold permissive enough to find the true part, but make final threshold and shape gate stricter for low-entropy or edge-sparse templates. |
| P2-004 | open | medium | Candidate generation and NMS stability | `Pattern/Pattern2.cpp` | P2-002 | Verify `GetNextMaxLoc`, `FilterWithRotatedRect`, and `effectiveNmsOverlap` do not keep nearby wrong rectangles over the real template. Prefer stable center/size/angle output and reject ROI-on-border candidates early. |
| P2-005 | open | low | Performance optimization after correctness | `Pattern/Pattern2.cpp`, `Pattern/Pattern2.h` | P2-002, P2-003 | After false-positive rejection is stable, reduce runtime by caching validator mats, reusing ROI buffers, limiting angle/scale candidate count, and measuring CPU multi-thread vs OpenCL path. No speed change should lower OK recall. |
