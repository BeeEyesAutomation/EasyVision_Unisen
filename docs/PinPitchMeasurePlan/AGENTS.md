# Pin Pitch Measure Plan

This folder tracks the plan for adding a pin pitch measurement mode/tool.

Follow the repository root `AGENTS.md` workflow. Changes in this folder are planning-only unless a map entry explicitly lists code files.

All plan, map, handoff, and implementation notes in this folder must be written in English.

## Agent Handoff

Current decision for implementation:

- Do not create a new `TypeTool` unless explicitly requested later.
- Extend existing `TypeTool.Pitch` with `PitchMeasureMode.PinPitch`.
- Extend existing `TypeTool.Width` with `WidthMeasureMode.PointToLine`.
- `ToolPitch` owns pin center detection and pin pitch values:
  - Find P1..P4.
  - Measure P1-P2, P2-P3, P3-P4, and optional P1-P4 span.
  - Expose P1..P4 through `listP_Center` for other tools.
- `ToolWidth` owns distance from one selected point/pin center to one line `L`:
  - Select P1..P4 from Pitch/PinPitch or another point-producing tool.
  - Detect line `L` with existing `RansacLine.FindBestLine`.
  - Project the point to `L`, draw the perpendicular segment, and store distance in `WidthResult`.
- Native C++ shape:
  - Add independent `PinPitchCore` and thin `PinPitchCli`.
  - Optionally add `VisionGeometryCore` for shared geometry helpers.
  - Do not inherit from existing `PitchCore`; current `PitchCore` is for crest/root profile pitch.
  - Keep `RansacLineCore` focused and reused for line fitting.

Primary planning files:

- `Plan.md`
- `CoreGuiImplementationPlan.md`
- `pin-pitch-map.md`
