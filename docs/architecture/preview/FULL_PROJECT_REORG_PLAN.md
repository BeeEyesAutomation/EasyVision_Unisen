# Full Project Reorganization Plan — EasyVision_Unisen

> Master plan from current state (2026-05-03) to target architecture in `CLAUDE.md` section 3. Read order: (1) `CLAUDE.md` § 0 Hard Rules, (2) last 3 entries of `CODEX_HISTORY.md`, (3) this file. One Task Card = one commit + one history entry (template 11.2 in `CLAUDE.md`).

---

## 0. Hard Rules for Codex / Claude (READ FIRST — NON-NEGOTIABLE)

### 0.1 Mandatory pre-flight before ANY task

```
[1] READ CODEX_HISTORY.md — full file, or at minimum the LAST 5 entries.
    └─ If you skip this step, you WILL repeat completed work or break invariants.
[2] READ CLAUDE.md § 0 (Hard Rules) and § 11 (Task Card template).
[3] READ this file (FULL_PROJECT_REORG_PLAN.md) — section 1 (current state)
    + the section for the block you are working in.
[4] Identify Task Card ID (e.g. P3L.13, P2.1) from section 2-11.
[5] git status → if dirty outside your scope, STOP and ask the user.
[6] Confirm Task Card has not already been done (search CODEX_HISTORY.md
    for the Task Card ID — if found, STOP).
```

**If steps [1]–[6] are not all satisfied, do not write any code.** Stop and report the gap to the user.

### 0.2 Execute-once rule

Every Task Card listed in this plan is **executed exactly once**. After it appears as a completed entry in `CODEX_HISTORY.md`, it MUST NOT be re-run, re-rewritten, or re-implemented.

- Re-doing a completed task = silent regression risk.
- If a completed task needs revision, that becomes a NEW Task Card with a new ID (e.g. `P3L.7-fix1`) and its own history entry — do not edit the original entry.
- If a Task Card is partially done (e.g. P3L.5 was scaffolded only), the next agent extends it under a clearly different sub-ID, never overwriting.

### 0.3 Per-step history append (not just per-task)

`CODEX_HISTORY.md` is the canonical execution log. Append rules:

1. **At task start**: append a header `## YYYY-MM-DD — <id> <slug> [STARTED]` with a one-line scope statement before touching any file.
2. **After each substantive step** (file move, namespace change, build verify, smoke test): append a sub-bullet under the current task header noting the step + result.
3. **At task end**: convert the `[STARTED]` header to the final form using template 11.2 in `CLAUDE.md` (Scope / Files touched / Verify / Notes / Blockers).
4. **On stop-and-ask**: append the question and the user's answer verbatim, then continue.
5. **On rollback**: append a `[ROLLED BACK]` entry with the reason.

This per-step granularity is what lets the next agent resume work safely if a session is interrupted mid-task.

### 0.4 Original user goals (must all be satisfied at the end of the plan)

1. Reorganize code scientifically — clean separation, no class interleaving.
2. Customizable tool UI — flexible tabs, easy to add features.
3. Refine tab sections of each tool.
4. Detailed class breakdown — single responsibility per file.
5. **HARD CONSTRAINT**: do not change the structure of classes already saved in the current storage mechanism (`BinaryFormatter + Base64`, `.prog` files).

Mapping to blocks:
- Goal 1, 4 → Block D (split BeeCore) + Block F (UI shell) + Block H (renames).
- Goal 2, 3 → Block 0 (Phase 3 Lite finish) + Block A (Group B host) + Block B (slimming).
- Goal 5 → enforced by Block 0 / P3L.9 persistence test, then by every later block.

### 0.5 Forbidden actions (zero-tolerance)

1. Re-running a Task Card that is already completed in `CODEX_HISTORY.md`.
2. Editing or deleting any past entry in `CODEX_HISTORY.md` — only append.
3. Skipping the per-step append in 0.3 because "it's a small change".
4. Working on multiple Task Cards in one commit.
5. Changing field layout of any `BeeCore/Unit/*.cs` payload class.
6. Creating new `Common.PropetyTools[...]` call sites outside `BeeCore/Common.cs`.
7. Touching files outside the In-scope list of the active Task Card (the worktree is dirty in many places — leave unrelated edits alone).
8. Subscribing to events without `X.Event -= Handler;` immediately above `X.Event += Handler;`.

### 0.6 Stop-and-ask triggers

| Situation | Question to ask |
|---|---|
| Task Card already completed in CODEX_HISTORY | "P<x>.<y> already done on <date>. Should I treat this as a follow-up fix or skip?" |
| Worktree dirty outside scope | "Files <list> are dirty. Stash, commit, or leave?" |
| A step would break the persistence test | "Doing this would break `.prog` round-trip. Stop, change plan, or accept break?" |
| New native/NuGet dependency required | "Add dependency <name>?" |
| Pilot effort > 2 days | "Switch tool / abort pilot / extend timebox?" |
| Public type rename would touch serialized data | "Provide a sample `.prog` to validate before rename?" |
| Diff > 100 files in one move | "Split into smaller commits?" |

---

## 1. Current state snapshot (2026-05-03)

**Completed**:
- P3L.0 — `BeeInterface/Tool/_Base/` framework (`IToolView`, `ToolTabContext`, `ToolTabRegistry`, `ToolViewBase` with 4 fixed tabs: General/ROI/Params/Result).
- P3L.1 — Three shared UCs: `RoiToolbar`, `ScoreThresholdBar`, `ResultMiniGrid` in `BeeInterface/Custom/`.
- P3L.2/3 — Runner facade pilots for `ToolCircle`, `ToolWidth` (`*EngineRunner.cs` in `BeeCore/Func/Engines/`).
- P3L.4 — Runner facades for 8 more Group A tools: Measure, Edge, Crop, EdgePixel, Corner, Intersect, ColorArea, CheckMissing, PositionAdjustment.
- P3L.5 — Group B tab registrar (`GroupBToolTabRegistrar.cs`) wired into `ToolViewBase.BuildDynamicTabs()`. **Scaffold only** at the time, but P3L.7 + P3L.11 now host real Group B tools on the base.
- P3L.6 — Investigated `CustomGui` / `Converts` / `ShapeEditing` duplicates. Not 1:1; safe migration recipes captured in P3L.10.
- P3L.7 — `ToolBarcode` migrated to `ToolViewBase`; `BarcodeEngineRunner.cs` added; dynamic Preset tab now visible.
- P3L.8 — `ToolCounter` decision = **bypass/deprecate**. Marked `[Obsolete]`; no `TypeTool.Counter` enum value, no palette entry to remove.
- P3L.9 — Persistence safety net: `tests/BeeCore.Persistence.Tests/` console harness with 3 tests (load count, frozen-key round-trip, score round-trip). Synthetic program used (no real `.prog` fixture committed yet).
- P3L.10 — Doc-only usage maps for `CustomGui` / `Converts` / `ShapeEditing` written to `docs/architecture/preview/CUSTOMGUI_USAGE_MAP.md` and `CONVERTS_USAGE_MAP.md`.
- P3L.11 — `ToolOCR` migrated to `ToolViewBase`; `OCREngineRunner.cs` added; dynamic Preset + Preprocess tabs now visible. (User picked OCR first over CraftOCR.)
- P3L.12 — `TOOL_SLIMMING_PROTOCOL.md` written; `ToolCircle` split into `ToolCircle.cs` + `ToolCircle.Roi.cs` + `ToolCircle.Parameters.cs`; main file now 136 LOC, under 250 budget.
- Build baseline: BeeInterface Debug\|x64 green; Pattern Debug\|x64 fixed (ZXing path, C++17, OpenCV linked); persistence harness Debug\|x64 builds + runs 3/3 green.

**Open / blocked**:
- Group B tools still on `UserControl` (need migration via Block A): Pattern, Patterns, MultiPattern, MatchingShape, Pitch, CraftOCR — registered tabs still dead UI for these.
- Pattern2 work in progress in worktree → blocks `ToolPattern` migration (P3L.18, last in Block A).
- Release\|x64 full-solution build still not verified — tracked as P3L.13.
- Runner facades did not actually reduce LOC for most Group A tools (only Circle has been slimmed via P3L.12); Block B will address the remaining 11.
- No real `.prog` fixture committed to `tests/testdata/`; persistence test relies on a synthesized program built in code.
- `BeeCore/Unit/Edge.cs`, `Width.cs` carry pre-existing dirty edits unrelated to runner work.
- `BeeInterface/Group/View.cs` is non-UTF8 (handled with byte-exact edits when touched).

**Hard invariants** (NEVER change without explicit user approval):
- `BeeCore/Unit/*.cs` payload field layout (Circle, Width, Edge, Pattern, Patterns, Yolo, …).
- 20 frozen keys in `PropetyTool.GetObjectData` (see `code_map.json` → `save_load_pipeline.frozen_keys`).
- Public API of `BeeCore.Common`: `TryGetTool`, `TryGetToolList`, `EnsureToolList`, `EnsureCurrentToolList`, `SetToolList`.
- Zero `Common.PropetyTools[...]` outside `BeeCore/Common.cs`.
- Event subscribe pattern: `X.Event -= Handler;` immediately above `X.Event += Handler;`.

---

## 2. Block 0 — Finish Phase 3 Lite (P3L.7 → P3L.13)

Goal: prove the dynamic tab pattern end-to-end, build a safety net, restore Release baseline. After this block, every later block is safer.

| # | Task | Status | Output | Stop-and-ask |
|---|---|---|---|---|
| P3L.7 | Group B pilot — Barcode | ✅ Done | `ToolBarcode` inherits `ToolViewBase`; Preset tab visible. `BarcodeEngineRunner.cs` added. | — |
| P3L.8 | Resolve ToolCounter | ✅ Done — bypass | Marked `[Obsolete("Bypassed; use the Learning/Yolo workflow instead.")]`. | Decided. |
| P3L.9 | Persistence safety net | ✅ Done | `tests/BeeCore.Persistence.Tests/` console harness, 3/3 green. Synthetic program; real `.prog` fixture still optional. | — |
| P3L.10 | Dedup reference map | ✅ Done | `CUSTOMGUI_USAGE_MAP.md`, `CONVERTS_USAGE_MAP.md` written. | — |
| P3L.11 | Group B pilot 2 — OCR | ✅ Done | `ToolOCR` on `ToolViewBase` with Preset + Preprocess tabs. `OCREngineRunner.cs` added. | OCR chosen first. |
| P3L.12 | Slimming protocol + Circle | ✅ Done | `TOOL_SLIMMING_PROTOCOL.md` written; `ToolCircle.cs` split to 136 LOC main + 2 partials. | Budget = 250 LOC accepted. |
| P3L.13 | Release\|x64 baseline | ⏳ Pending | New `baseline_build_release.log` + section in `baseline_build.md`. Minimal csproj patches if needed. | — |

**Exit criteria**: at least 2 Group B tools host dynamic tabs ✅ · persistence test green ✅ · Release\|x64 errors = 0 ⏳ (P3L.13) · ToolCounter has documented status ✅ · LOC protocol exists ✅.

**Block 0 status**: 6 of 7 tasks complete. Only P3L.13 remains before moving to Block A.

---

## 3. Block A — Finish Group B migration (P3L.14 → P3L.18)

Goal: all 8 Group B tools inherit `ToolViewBase` and show their registered dynamic tabs.

| # | Tool | Notes / dependencies |
|---|---|---|
| P3L.14 | `ToolMatchingShape` | Brings ShapeEditing host into base layout. Persistence test must include shapes. |
| P3L.15 | `ToolPitch` | Convert `PitchGdiPainter` into a UserControl hosted in Params tab. |
| P3L.16 | `ToolMultiPattern` | Adds DataGrid for sub-patterns; reuses Pattern's Preset tab. |
| P3L.17 | `ToolCraftOCR` | Mirror P3L.11 (OCR) with model-path UI in Params. |
| P3L.18 | `ToolPattern` | **LAST** — only after Pattern2 committed/stable. Highest risk. Refresh persistence fixtures with Pattern templates. |

**Exit criteria**: 8/8 Group B tools host dynamic tabs · persistence test still green · all `ToolXxx.cs` within LOC budget OR documented per-tool exception.

---

## 4. Block B — Slimming pass for all migrated tools (P3L.19 → P3L.30)

Goal: every migrated tool meets the LOC budget defined in P3L.12.

For each tool, in this order: Circle (already done in P3L.12) → Width → Measure → Edge → Crop → EdgePixel → Corner → Intersect → ColorArea → CheckMissing → PositionAdjustment → Barcode → OCR → MatchingShape → Pitch → MultiPattern → CraftOCR → Pattern.

**Per-tool steps**:
1. Move leftover engine plumbing into matching `*EngineRunner.cs`.
2. Move shared UI helpers into `BeeInterface/Custom/` if reused by ≥ 3 tools.
3. Designer untouched unless removing now-duplicate panels.
4. Persistence test + smoke + LOC delta in history entry.

**Stop-and-ask trigger**: if budget unrealistic for Pattern/MultiPattern, define per-tool budget and document in protocol file.

---

## 5. Block C — Group C decisions (P3L.31 → P3L.35)

Goal: every "tạm bỏ qua" tool from CLAUDE.md § 10.5 has a documented status.

| # | Tool | Decision required |
|---|---|---|
| P3L.31 | `ToolYolo` | Keep custom (label editor + training UI) / refactor / deprecate |
| P3L.32 | `ToolMultiOnnx` | Same options, depends on Yolo |
| P3L.33 | `ToolOKNG` | Wait for `OKNG.h` standardization or migrate now |
| P3L.34 | `ToolAutoTrig` | Tool or "Trigger module" pattern? |
| P3L.35 | `ToolVisualMatch` | Confirm legacy vs active |

**Per-tool**: STOP and ask user. Tag with `[ToolCustomLayout("reason")]` or `[Obsolete("...")]`. No silent migrations.

---

## 6. Block D — Split BeeCore into 7 sub-projects (P2.0 → P2.8)

Goal: turn `BeeCore` into a pure shim assembly. Each sub-project owns one concern. Backward compat via `[assembly: TypeForwardedTo]`.

**Order is fixed by ref count (low → high)**:

| # | Sub-project | Contents | Risk gate |
|---|---|---|---|
| P2.0 | Shim infra | `Properties/AssemblyAttributes.cs`, `tools/gen_typeforward.ps1` | — |
| P2.1 | `BeeCore.Domain` | Items, `Propety/Propety2/PropetyTool/ToolState`, `Common.cs`, `ResultItem`, `LabelItem`, `ValueRobot` | Persistence test must pass |
| P2.2 | `BeeCore.Algorithms` | `Algorithm/*` (folder lift) | Build + smoke |
| P2.3 | `BeeCore.Vision` | `Vision.cs`, `dataMat.cs` (move LAST in this card), `MatHelper`, `Crop`, `Draws`, `ImageUtils`, `BitmapExtensions`, `MatrixExtension`, `ShapeEditing/*` | Build + smoke |
| P2.4 | `BeeCore.Camera` | `Camera/*`, `HEROJE`, `USB`, `KEY_Send` | Keep namespace if serialized |
| P2.5 | `BeeCore.IO` | `Data/*`, `DB.cs`, `LibreTranslateClient`, `Access.cs` (the save mechanism itself) | **CRITICAL** — persistence test on real `.prog` MUST pass |
| P2.6 | `BeeCore.Comm` | `EtherNetIP/*`, `Comunication.cs` (no rename until Phase 6) | — |
| P2.7 | `BeeCore.AI` | `NativeYolo`, `NativeRCNN`, `Native`, `Init`, `Logs` | Confirm OpenVINO native still excluded |
| P2.8 | `BeeCore` shim | Reduce to `Properties/Forward_*.cs` + `<ProjectReference>` to 7 sub-projects | Full Release\|x64 build pass |

**Per-sub-project pattern**:
1. Create new `.csproj` (Class Library, .NET Framework 4.8, x64).
2. `git mv` files into new project (one move per commit if folder is large).
3. Change namespace `BeeCore` → `BeeCore.<X>` only in moved files.
4. Add `<ProjectReference>` from `BeeCore` to new sub-project.
5. Add `BeeCore/Properties/Forward_<X>.cs` with `[assembly: TypeForwardedTo(typeof(BeeCore.<X>.<Type>))]` for every public type.
6. Build + persistence test + smoke.

**Exit criteria**: full Release\|x64 build passes · loading existing `.prog` works · `BeeCore.dll` contains zero source types (only forwards).

---

## 7. Block E — Tooling and CI (parallel with D)

Goal: manual guards become automatic. No commit can merge without passing them.

| Task | Deliverable |
|---|---|
| E.1 | `tools/check_propety_tools.ps1` — wraps the existing PowerShell guard. |
| E.2 | `tools/check_event_pairs.ps1` — counts `+=` vs `-=` for known events; fails if unequal. |
| E.3 | `tools/run_persistence_tests.ps1` — wrapper around `dotnet test tests/BeeCore.Persistence.Tests/`. |
| E.4 | `tools/build_full.ps1` — Debug + Release MSBuild + all guards + tests. |
| E.5 | `.github/workflows/build.yml` (or local runner equivalent) — runs E.4 on push/PR. |
| E.6 | Update `CLAUDE.md` § 7 to reference automated guards instead of manual PowerShell snippets. |

---

## 8. Block F — UI shell cleanup (P4.1 → P4.3)

Only after Block D is stable.

| # | Task | Notes |
|---|---|---|
| P4.1 | Move `BeeInterface/Custom/*` into a new `BeeShared.UI` project | `RoundedPanel`, `GradientTabControl`, `AdjustControl`, `LayoutPersistence`, `ConvertImg`, `RoiToolbar`, `ScoreThresholdBar`, `ResultMiniGrid`. Leave shim namespace if needed. |
| P4.2 | Sort `BeeUi/Unit/*` files | Per-file: Vision helper → `BeeCore.Vision`; tool-specific → `Tool.<X>` (future); shared UI → `BeeShared.UI`. |
| P4.3 | Consolidate three `Global.cs` sources | UI state → `BeeShared.UI/UiState.cs`; runtime state → `BeeCore.Domain/RuntimeState.cs`; config → existing `BeeGlobal/Config.cs`. |

---

## 9. Block G — Native + tests (Phase 5)

| # | Task | Notes |
|---|---|---|
| P5.1 | De-duplicate `Pattern.dll` vs `BeeCV.dll` exports | Identify overlap (capture, shape utility); pick canonical, forward the other. |
| P5.2 | BeeCV C++17 | Mirror what `Pattern` got at P0.3. |
| P5.3 | One header per native DLL | `Pattern/BeeCpp.h`, `BeeCV/BeeCV.h`, `BeeNativeOnnx/BeeNativeOnnx.h`. C# P/Invoke goes through these only. |
| P5.4 | Expand test suite | `BeeCore.Algorithms.Tests` (≥ 5 cases), `Tool.Circle.Tests` (≥ 5 cases), reuse persistence test infra. |
| P5.5 | OpenVINO unblock (optional) | If user provides headers/libs, re-enable `BeeNativeOnnx` and `BeeNativeRCNN` in solution Configuration Manager. |

---

## 10. Block H — Renames (Phase 6, LAST)

One PR per rename. Git tag baseline before each. Use IDE refactor (F2) so designers + resx follow.

| # | Rename | Compat shim |
|---|---|---|
| P6.1 | `Propety` family → `Property` (`Propety`, `Propety2`, `PropetyTool`, `Common.PropetyTools`) | `[Obsolete] class Propety : Property { }` shim in `BeeCore.Domain` for one release. |
| P6.2 | `Comunication` → `Communication` | File + class rename only. |
| P6.3 | `dataMat` → `DataMat` | File + class rename. |
| P6.4 | Underscored filenames | `Position_Adjustment` → `PositionAdjustment`, `KEY_Send` → `KeySend`. |

After all renames pass one release with `[Obsolete]` shims green, remove the shims.

---

## 11. Block I — Final consolidation

Goal: solution layout matches CLAUDE.md § 3 target tree.

| # | Task |
|---|---|
| I.1 | Move all `Tool.<X>` modules into `src/Tools/Tool.<X>/` per § 3 target. Each tool = `Config.cs` (POCO) + `Engine.cs` (logic) + `Form.cs` (View) + `Form.Designer.cs` + `Form.resx`. |
| I.2 | Reorganize solution folders: `src/App/`, `src/Shared/`, `src/Core/`, `src/Tools/`, `src/UI/`, `src/Native/`, `tests/`. |
| I.3 | Delete the `BeeCore` shim if no consumer needs it. |
| I.4 | Update `EasyVision.sln` solution folders to match. |
| I.5 | Verify `CLAUDE.md` § 3 layout achieved; update Changelog § 13. |

---

## 12. Decisions

### 12.1 Resolved (do not re-ask)

| # | Question | Decision | Recorded in |
|---|---|---|---|
| R1 | ToolCounter fate | Bypass — marked `[Obsolete]`, no palette entry to remove | P3L.8 |
| R2 | Group B pilot order | OCR before CraftOCR | P3L.11 |
| R3 | LOC ceiling for `ToolXxx.cs` | ≤ 250 lines per main file (partials allowed) | P3L.12 / `TOOL_SLIMMING_PROTOCOL.md` |
| R4 | Persistence test framework | Console harness (no xUnit dep) — exit-code based | P3L.9 |

### 12.2 Open — must be answered before the listed block starts

| # | Question | Blocks |
|---|---|---|
| Q1 | **Pattern2 stability date** — when can `BeeCore/Unit/Patterns.cs` and `Pattern/Pattern2.*` be considered frozen for migration? | P3L.18, Block A finish |
| Q2 | **Group B order after Barcode + OCR** — confirm: MatchingShape → Pitch → MultiPattern → CraftOCR → Pattern? | Block A (P3L.14–18) |
| Q3 | **`BeeInterface/Group/View.cs`** — re-encode to UTF-8 BOM as a separate one-shot task, or keep handling with byte-exact edits? | Block F (P4.x) |
| Q4 | **`BeeCore/Unit/Edge.cs` and `Width.cs`** pre-existing local edits — review-and-commit, stash, or revert? | Block B (slimming touches them) |
| Q5 | **CI timing** — set up automation in parallel with Block D, or wait until Block D finishes? | Block E |
| Q6 | **OpenVINO** — provide headers to unblock `BeeNativeOnnx` / `BeeNativeRCNN`, or keep excluded for the whole reorg? | Block G (P5.5) |
| Q7 | **`.prog` external consumers** — any external tooling reads `.prog` files? Affects how aggressively we can change namespaces in P2.5. | Block D (P2.5) |
| Q8 | **`Propety → Property` rename** — confirm the rename is wanted; it WILL break any user code that does `using BeeCore;` and references `Propety` directly. | Block H (P6.1) |
| Q9 | **Real `.prog` fixture** — provide one curated sample for `tests/testdata/` so the persistence test exercises real file format? | Block 0 → ongoing |

---

## 13. Working rules (carry forward to every task)

These rules expand on §0 Hard Rules. **A task is not "done" until every rule below is satisfied.**

### 13.1 The read-do-record loop

Every Task Card follows this exact loop. No shortcuts.

```
┌──────────────────────────────────────────────────────────────────────┐
│  READ                                                                │
│    ① CODEX_HISTORY.md — full file or last 5 entries minimum.         │
│       Verify the Task Card ID is NOT already completed.              │
│    ② CLAUDE.md § 0 + § 11.                                           │
│    ③ This file: § 1 (snapshot) + the block containing your task.    │
│  ────────────────────────────────────────────────────────────────    │
│  DO                                                                  │
│    ④ Append `## YYYY-MM-DD — <id> <slug> [STARTED]` to               │
│       CODEX_HISTORY.md with one-line scope BEFORE first edit.        │
│    ⑤ Execute steps from the Task Card. After EACH substantive        │
│       step (move, edit, build, test), append a sub-bullet            │
│       under the [STARTED] header.                                    │
│    ⑥ Run all Verify commands listed in the Task Card.                │
│  ────────────────────────────────────────────────────────────────    │
│  RECORD                                                              │
│    ⑦ Replace the [STARTED] header with the final entry per           │
│       template 11.2 (Scope / Files touched / Verify / Notes /        │
│       Blockers).                                                     │
│    ⑧ One commit. Message: `[<id>] <summary>`.                        │
│    ⑨ Report back to user: files touched + verify result + any        │
│       blocker that remains.                                          │
└──────────────────────────────────────────────────────────────────────┘
```

### 13.2 Execute-once enforcement

1. **Each Task Card runs at most once.** Before starting, grep `CODEX_HISTORY.md` for the ID. If found and status is not `[ROLLED BACK]`, STOP and ask the user.
2. **Never overwrite a past history entry.** Append only. Corrections become a new dated entry.
3. **A partial completion is not "done".** P3L.5 is the cautionary example — it was scaffolded and marked done, but Group B tools were not actually hosted until P3L.7 / P3L.11 created follow-ups.
4. **If a task fails halfway**, append `[ROLLED BACK]` with the reason and revert. The next agent picks up from a clean baseline.

### 13.3 Per-step append (not just per-task)

Append to `CODEX_HISTORY.md` after each of these events, even mid-task:

- File moved, created, or deleted.
- Namespace or public API touched.
- Build attempted (record command + result).
- Test attempted (record command + pass/fail count).
- Smoke run on the app.
- Stop-and-ask question raised + user's answer.
- Decision made that affects later tasks.

The granularity matters: if the session is interrupted, the next agent must be able to resume exactly where the previous one stopped without re-doing work.

### 13.4 Invariant-protection rules

1. Never edit `BeeCore/Unit/*.cs` payload field layout, the 20 frozen keys in `PropetyTool.GetObjectData`, or anything that would make a serialized `.prog` fail to load. The persistence test (P3L.9) is the gate; run it before AND after any change in `BeeCore.Domain` / `BeeCore.IO`.
2. Never introduce a new `Common.PropetyTools[...]` call site outside `BeeCore/Common.cs`. Use `Common.TryGetTool` / `TryGetToolList` / `EnsureToolList` / `EnsureCurrentToolList` / `SetToolList`.
3. Always pair `X.Event -= Handler;` immediately above `X.Event += Handler;`. The CI guard from Block E will eventually enforce this.
4. Never edit a file outside the In-scope list of the active Task Card. The worktree is dirty in many places — unrelated edits stay untouched. Use `git add <specific-files>`, never `git add .`.
5. Never rename a public type and move it across projects in the same commit. Renames are Block H, after structural moves stabilize.

### 13.5 Process rules

1. One Task Card = one commit. Commit message: `[<id>] <summary>`. Body references the Task Card md path under `docs/architecture/tasks/`.
2. Use the Task Card template (CLAUDE.md § 11.1) before coding; copy it to `docs/architecture/tasks/YYYY-MM-DD-<id>-<slug>.md`.
3. After each block: `git tag block-<letter>-done`. Rollback = `git reset --hard <previous-tag>`.
4. If a Task Card cannot be completed without violating any rule above, STOP and ask the user — do not silently work around it.
5. Doc-only edits (like this one) still follow the read-do-record loop: append to CODEX_HISTORY.md, get a `[STARTED]` entry, finalize when done.

---

## 14. Estimated phase boundaries

| Block | Cards | Approx effort | Gate to next block |
|---|---|---|---|
| 0 | P3L.7 → P3L.13 | 5–7 days | Persistence test green; Release baseline; ≥ 2 Group B tools live |
| A | P3L.14 → P3L.18 | 1.5 weeks | All 8 Group B host dynamic tabs |
| B | P3L.19 → P3L.30 | 2 weeks | All migrated tools within LOC budget |
| C | P3L.31 → P3L.35 | 2 days (review only) | Each Group C tool tagged |
| D | P2.0 → P2.8 | 2–3 weeks | `BeeCore` is pure shim; full Release pass; persistence test green |
| E | parallel with D | 3 days | CI runs on every commit |
| F | P4.1 → P4.3 | 1 week | UI shell tidy |
| G | P5.1 → P5.5 | 1 week (no OpenVINO) / +1 week if OpenVINO | Native concerns separated; tests pass |
| H | P6.1 → P6.4 | 3–5 days | All renames done; shims kept |
| I | I.1 → I.5 | 3 days | Target layout achieved |

**Total**: ~10–12 weeks of focused effort, calendar-flex depending on user decisions.

---

## 15. Cross-references

- `docs/architecture/preview/code_map.json` — projects, tool inventory, save/load pipeline, frozen keys.
- `docs/architecture/preview/CODE_PREVIEW.md` — human-readable map of layers, tools, tab proposal.
- `docs/architecture/preview/TASK_CARDS_P3_LITE.md` — original P3L.0–P3L.7 cards (what was done).
- `CODEX_HISTORY.md` — canonical log of completed work (append after each task).

---

## 16. Visual layout — Current vs Target

This section visualizes every class / UC / Form / Custom control so the reorg destination is unambiguous. Use it as the single source of truth when moving files in Block D / F / I.

### 16.1 Current layout (snapshot 2026-05-03)

```
EasyVision_Unisen/
├── Program.cs                            ← BeeMain entry point
│
├── BeeGlobal/                            [Class Library — global state, params, protocol]
│   ├── *.cs (root)                       Config, Global, Enums, Model, ProgNo, ParaCamera, ParaCommon,
│   │                                     ParaIO, ParaShow, ValuePara, Comunication, BatchRename,
│   │                                     CycleTimerSplit, DASK, HistoryCheck, IntArrayWithEvent,
│   │                                     ItemNew, ItemRegsImg, Labels, LogsDashboard, PCI_Card,
│   │                                     QpcTimer, RectRotate, RotatedBoxInfo, Test, TimingUtils
│   ├── Common/                           AdjustControl, AdjustMoveMouse  ← UI helpers!
│   ├── General/                          DependencyScanner, PythonDepScanner
│   └── Protocol/                         ParaBit, ParaProtocol, ParaValue, PlcClient
│
├── BeeCore/                              [Class Library — GOD-PROJECT, ~50K LOC]
│   ├── *.cs (root, 17 files)             Common, PropetyTool, ToolState,
│   │                                     Vision, dataMat, Converts, BitmapExtensions, MatrixExtension,
│   │                                     KeepLargestAuto, Actions, AddTool, LocalTool, LabelItem,
│   │                                     ValueRobot, LibreTranslateClient, DB, Checking
│   ├── Algorithm/                        Colors, DetectIntersect, EdgePoints, FilletCornerMeasure(2),
│   │                                     FilterItem, Filters, Gap, Geometry2D, ImagePreprocessPipeline,
│   │                                     InsertLine, LineDetector, MonoSegmentation,
│   │                                     RansacCircleFitter, RansacFitLine
│   ├── Camera/                           HEROJE, USB, KEY_Send, HardwareEnum, About, DevStateCB,
│   │                                     DevStateDef, DeviceFindAndCom, GetCfgCB, HJ_CRC32, LogRecord,
│   │                                     ModuleSetting, PnPEntityInfo, ProtocolExtraDataStu,
│   │                                     ProtocolHeaderStu, SendCfgDataCB, SetCfgCB, ToolCfg
│   ├── Core/                             HSV, RGB, HsvConvert, HelpMouseView
│   ├── Data/                             Access (BinaryFormatter+Base64), SaveData, LoadData,
│   │                                     ClassProject (empty shell)
│   ├── EtherNetIP/                       AssemblyHelper, EDS, EIPAdapter, EIPScanner
│   ├── Func/                             Cal, Camera, CameraIOFast, CodeSymbologyCliExtensions,
│   │                                     ComboBoxExtensions, Converts, Crop, CustomGui, Draws,
│   │                                     FilterRect, General, ImageUtils, Init, Line2DTransform,
│   │                                     Logs, MatHelper, MatMerger, Native, NativeRCNN, NativeYolo,
│   │                                     PolyOffset, RectRotateGapChecker, ResultFilter, ResultItem,
│   │                                     ResultItemHelper, ResultMulti, TarProgramHelper
│   ├── Func/Engines/                     [P3L.2-4 ADDED] BarcodeRunner, CheckMissingRunner,
│   │                                     CircleRunner, ColorAreaRunner, CornerRunner, CropRunner,
│   │                                     EdgeRunner, EdgePixelRunner, IntersectRunner, MeasureRunner,
│   │                                     OCRRunner, PositionAdjustmentRunner, WidthRunner
│   ├── Items/                            ItemTool (UC + Designer)
│   ├── Parameter/                        G
│   ├── ShapeEditing/                     CanvasCursorKind, CanvasMouseArgs, CanvasMouseButton,
│   │                                     ColorPickedArgs, ColorPickerOverlay, DefaultShapeRepository,
│   │                                     GeometryHelper, IImageCanvas, IOverlayPainter, IShapeEditor,
│   │                                     IShapeRepository, InteractionMode, ShapeChangeKind,
│   │                                     ShapeChangedArgs, ShapeEditOptions, ShapeEditState
│   └── Unit/                             [PERSISTED ENGINES — DO NOT TOUCH]
│                                         AutoTrig, Barcode, CheckMissing, Circle, ColorArea, Counter,
│                                         CraftOCR, Crop, Edge, EdgePixel, Intersect, MatchingShape,
│                                         Measure, MeasureCorner, MultiOnnx, MultiPattern, OCR, OKNG,
│                                         OKNGAPI, PaperEnhance, Patterns, Pitch, PitchGdiPainter,
│                                         PositionAdj, Positions, VisualMatch, Width, Yolo
│
├── BeeInterface/                         [Class Library — UI shell, MIXED concerns]
│   ├── *.cs (root, 19 files)             AppRestart, ControlStylePersistence, ConvertImg, CustomGui,
│   │                                     DataTool, FileName, FormFlowChart, FormWarning, ForrmAlarm,
│   │                                     FrameRenderer, GeneralSetting, Global, GlobalIconManager,
│   │                                     ItemValue, LayoutPersistence, Load, MultiDockHost, ShowTool,
│   │                                     StepEdit, Tools
│   ├── Class/                            Cryto, Decompile, Func, GoogleDriveDllManager, Gui,
│   │                                     KbdListener, KeyAcitve, RsDirPermissions, SqlServer
│   ├── Comunications/                    WriteValuePLCSystems
│   ├── Custom/                           [reusable controls]
│   │                                     AdjustBarEx, AdjustNumberPad, AutoLabel, CollageRenderer,
│   │                                     CustomNumericEx, DbTableLayoutPanel, GradientTabControl,
│   │                                     RJButton, RoundedPanel, StepProgressBar, TextBoxAuto,
│   │                                     [P3L.1] ResultMiniGrid, RoiToolbar, ScoreThresholdBar
│   ├── DashBoard/                        DashboardImages, DashboardListCompact, FlipClockControl,
│   │                                     RegisterImgDashboard, ReportDashBoard, SaveProgressDialog,
│   │                                     StatusDashboard
│   ├── General/                          Account, FormCheckUpdate, FormChoose, FormReport, InforGroup,
│   │                                     ItemLogic, ItemRS, MessageChoose, QuickSetting, ToolPage
│   ├── Group/                            AddFilters, AddTool, BarRight, BtnHeaderBar, EditBar,
│   │                                     EditProg, EditRectRot, Header, HideBar, InforBar,
│   │                                     RegisterImgs, SimImgs, StatusBar, ToolSettings, View,
│   │                                     ViewHost, ucReport
│   ├── GroupControl/                     OK_Cancel
│   ├── PLC/                              ProtocolPLC, ucBitInput, ucBitOutput, ucValueOutput
│   ├── ShapeEditing/                     [DUPLICATE of BeeCore/ShapeEditing/!]
│   │                                     CanvasMouseArgs, CanvasViewportChangedArgs,
│   │                                     DefaultShapeRepository, GeometryHelper, IImageCanvas,
│   │                                     IOverlayPainter, IShapeEditor, IShapeRepository,
│   │                                     ImageCanvasControl, ImageCanvasInteractionState,
│   │                                     ImageCanvasShapeContext, InteractionMode, ShapeChangedArgs,
│   │                                     ShapeEditOptions, ShapeEditState
│   ├── Steps/                            SettingStep1, SettingStep2, SettingStep4
│   ├── Tool/                             [24 tool UCs — Designer + cs each]
│   │                                     ToolAutoTrig, ToolBarcode, ToolCheckMissing, ToolCircle,
│   │                                     ToolColorArea, ToolCorner, ToolCounter, ToolCraftOCR,
│   │                                     ToolCrop, ToolEdge, ToolEdgePixel, ToolIntersect,
│   │                                     ToolMatchingShape, ToolMeasure, ToolMultiOnnx,
│   │                                     ToolMultiPattern, ToolOCR, ToolOKNG, ToolPattern, ToolPitch,
│   │                                     ToolPosition_Adjustment, ToolVisualMatch, ToolWidth, ToolYolo
│   └── Tool/_Base/                       [P3L.0+P3L.5 ADDED]
│                                         IToolView, ToolTabContext, ToolTabRegistry,
│                                         ToolViewBase (+Designer), GroupBToolTabRegistrar
│
├── BeeUi/                                [WinExe — main forms]
│   ├── Form/                             Main, FormLoad, FormActive, ScanCCD, ShowEraser
│   ├── Tool/                             DataContractAttribute
│   ├── Unit/                             EditTool
│   ├── Commons/                          (empty)
│   └── Python/                           Learning.py, OcrWapper.py
│
├── BeeUpdate/                            [WinExe — updater]
│   └── Program.cs
│
├── PLC_Communication/                    [Class Library — already separated]
│
└── Native projects (C++):
    ├── Pattern/                          BeeCpp.dll (Pitch, Pattern2, Barcode, Ransac, Color, Mono)
    ├── BeeCV/                            OpenCV wrapper, capture, shape
    ├── BeeNativeOnnx/                    YOLO/OpenVINO inference [DISABLED — missing OpenVINO]
    ├── BeeNativeRCNN/                    Mask R-CNN [DISABLED — missing OpenVINO]
    ├── OKNG/                             OKNG API
    ├── PylonCli/                         Basler camera (C++/CLI)
    ├── ColorPixelsCPP/                   Color helpers
    └── BeeOnnxCLi/                       CLI wrapper
```

**Pain points visible in this tree**:

| # | Pain | Location |
|---|---|---|
| 1 | Duplicate `CustomGui.cs` | `BeeCore/Func/CustomGui.cs` + `BeeInterface/CustomGui.cs` (different namespaces, partial overlap) |
| 2 | Duplicate `Converts.cs` | `BeeCore/Converts.cs` (`Convert2`) + `BeeCore/Func/Converts.cs` (`Func.Converts`) |
| 3 | Duplicate `ShapeEditing/` | `BeeCore/ShapeEditing/` (16 files, abstractions) + `BeeInterface/ShapeEditing/` (15 files, UI impl) |
| 4 | Three `Global.cs` sources | `BeeGlobal/Global.cs` + `BeeInterface/Global.cs` + `BeeUi/Global.cs` |
| 5 | UI helpers in non-UI projects | `BeeGlobal/Common/AdjustControl.cs`, `BeeGlobal/Common/AdjustMoveMouse.cs` |
| 6 | 17 stray files at `BeeCore/` root | mix of payloads, helpers, vision, IO |
| 7 | 19 stray files at `BeeInterface/` root | mix of forms, helpers, persistence, dock host |
| 8 | Tool UCs and engines in different projects | `ToolCircle.cs` (UI) at `BeeInterface/Tool/`, `Circle.cs` (engine payload) at `BeeCore/Unit/`, runner at `BeeCore/Func/Engines/` |
| 9 | `Camera` exists in both `BeeCore/Camera/` (HEROJE driver) AND `BeeCore/Func/Camera.cs` | concerns crossed |
| 10 | `BeeUi/Unit/`, `BeeUi/Tool/`, `BeeUi/Commons/` are mostly empty placeholders | dead structure |

### 16.2 Target layout (per CLAUDE.md § 3, fully expanded)

```
EasyVision_Unisen/
├── EasyVision.sln
│
├── src/
│   ├── App/
│   │   ├── BeeMain/                      ← Program.cs only (entry point)
│   │   └── BeeUpdate/                    ← Updater (no change)
│   │
│   ├── Shared/                           ← LAYER 1 — no UI/Core dependencies
│   │   ├── BeeGlobal/                    Pure config & protocol
│   │   │   ├── Config.cs                 (was BeeGlobal/Global.cs runtime fields)
│   │   │   ├── Enums.cs
│   │   │   ├── ParaCamera/Common/IO/Show.cs
│   │   │   ├── ValuePara.cs, Model.cs, ProgNo.cs
│   │   │   ├── TimingUtils.cs, QpcTimer.cs, CycleTimerSplit.cs
│   │   │   ├── Labels.cs, RectRotate.cs, RotatedBoxInfo.cs
│   │   │   ├── BatchRename.cs, IntArrayWithEvent.cs
│   │   │   ├── HistoryCheck.cs, LogsDashboard.cs
│   │   │   └── Protocol/                 ParaBit, ParaProtocol, ParaValue, PlcClient
│   │   │
│   │   └── BeeShared.UI/                 ← UI helpers reused ≥ 3 places
│   │       ├── Controls/
│   │       │   ├── RoundedPanel.cs               (← BeeInterface/Custom)
│   │       │   ├── GradientTabControl.cs         (← BeeInterface/Custom)
│   │       │   ├── DbTableLayoutPanel.cs         (← BeeInterface/Custom)
│   │       │   ├── RJButton.cs                   (← BeeInterface/Custom)
│   │       │   ├── AdjustBarEx.cs                (← BeeInterface/Custom)
│   │       │   ├── AdjustNumberPad.cs            (← BeeInterface/Custom)
│   │       │   ├── AutoLabel.cs                  (← BeeInterface/Custom)
│   │       │   ├── CustomNumericEx.cs            (← BeeInterface/Custom)
│   │       │   ├── TextBoxAuto.cs                (← BeeInterface/Custom)
│   │       │   ├── StepProgressBar.cs            (← BeeInterface/Custom)
│   │       │   ├── CollageRenderer.cs            (← BeeInterface/Custom)
│   │       │   ├── AdjustControl.cs              (← BeeGlobal/Common — UI not Global)
│   │       │   ├── AdjustMoveMouse.cs            (← BeeGlobal/Common — UI not Global)
│   │       │   ├── RoiToolbar.cs                 (P3L.1)
│   │       │   ├── ScoreThresholdBar.cs          (P3L.1)
│   │       │   ├── ResultMiniGrid.cs             (P3L.1)
│   │       │   ├── ImagePreviewControl.cs        (NEW — wraps Cyotek.ImageBox)
│   │       │   ├── ReferenceCombo.cs             (NEW — for Pattern/Pitch reference)
│   │       │   ├── PresetCombo.cs                (NEW — for Group B Preset tab)
│   │       │   ├── AngleControl.cs               (NEW — Trackbar+numeric)
│   │       │   └── ColorPickerControl.cs         (NEW — for ColorArea/RGB)
│   │       ├── CustomGui.cs                      (← merged 2 sources)
│   │       ├── ConvertImg.cs                     (← BeeInterface root)
│   │       ├── LayoutPersistence.cs              (← BeeInterface root)
│   │       ├── ControlStylePersistence.cs        (← BeeInterface root)
│   │       ├── GlobalIconManager.cs              (← BeeInterface root)
│   │       ├── FrameRenderer.cs                  (← BeeInterface root)
│   │       └── UiState.cs                        (← merged Global.cs UI fields)
│   │
│   ├── Core/                             ← LAYER 2 — domain, depends on Shared only
│   │   ├── BeeCore.Domain/               POCO models, no OpenCV, no Forms
│   │   │   ├── Common.cs                 (resolver helpers — public API stable)
│   │   │   ├── Items/
│   │   │   │   ├── ItemTool.cs (+ Designer)
│   │   │   │   ├── LabelItem.cs          (← BeeCore root)
│   │   │   │   ├── ValueRobot.cs         (← BeeCore root)
│   │   │   │   ├── AddTool.cs            (← BeeCore root)
│   │   │   │   └── LocalTool.cs          (← BeeCore root)
│   │   │   ├── Persisted/                ← THE 28 FROZEN PAYLOAD CLASSES
│   │   │   │   ├── PropetyTool.cs        (renamed Property in P6.1)
│   │   │   │   ├── ToolState.cs
│   │   │   │   ├── Circle.cs, Width.cs, Edge.cs, Crop.cs, Measure.cs, MeasureCorner.cs,
│   │   │   │   ├── EdgePixel.cs, ColorArea.cs, CheckMissing.cs, Counter.cs, Intersect.cs,
│   │   │   │   ├── PositionAdj.cs, Positions.cs, AutoTrig.cs, Barcode.cs, OCR.cs,
│   │   │   │   ├── CraftOCR.cs, Patterns.cs, MultiPattern.cs, MatchingShape.cs, Pitch.cs,
│   │   │   │   ├── PitchGdiPainter.cs, Yolo.cs, MultiOnnx.cs, OKNG.cs, OKNGAPI.cs,
│   │   │   │   ├── PaperEnhance.cs, VisualMatch.cs
│   │   │   ├── Results/
│   │   │   │   ├── ResultItem.cs, ResultItemHelper.cs, ResultMulti.cs, ResultFilter.cs
│   │   │   ├── Actions.cs, Checking.cs   (← BeeCore root)
│   │   │   └── RuntimeState.cs           (← merged Global.cs runtime fields)
│   │   │
│   │   ├── BeeCore.Algorithms/           Pure CV math, depends on OpenCvSharp + Domain
│   │   │   ├── Colors.cs, DetectIntersect.cs, EdgePoints.cs, FilletCornerMeasure.cs,
│   │   │   ├── FilletCornerMeasure2.cs, FilterItem.cs, Filters.cs, Gap.cs, Geometry2D.cs,
│   │   │   ├── ImagePreprocessPipeline.cs, InsertLine.cs, LineDetector.cs,
│   │   │   ├── MonoSegmentation.cs, RansacCircleFitter.cs, RansacFitLine.cs
│   │   │   └── KeepLargestAuto.cs        (← BeeCore root)
│   │   │
│   │   ├── BeeCore.Vision/               Mat helpers, drawing, image utilities
│   │   │   ├── Vision.cs                 (← BeeCore root, 2100 LOC)
│   │   │   ├── DataMat.cs                (← dataMat.cs renamed P6.3)
│   │   │   ├── MatHelper.cs, MatMerger.cs, Crop.cs, Draws.cs, ImageUtils.cs
│   │   │   ├── PolyOffset.cs, Line2DTransform.cs, RectRotateGapChecker.cs, FilterRect.cs
│   │   │   ├── Cal.cs
│   │   │   ├── BitmapExtensions.cs       (← BeeCore root)
│   │   │   ├── MatrixExtension.cs        (← BeeCore root)
│   │   │   ├── Engines/                  ← all *EngineRunner.cs from P3L.2-4
│   │   │   │   └── BarcodeRunner, CircleRunner, …, WidthRunner
│   │   │   └── ShapeEditing/             ← merged BeeCore/ + BeeInterface/ ShapeEditing
│   │   │       ├── Abstractions/         IImageCanvas, IShapeEditor, IShapeRepository, …
│   │   │       └── Controls/             ImageCanvasControl, …
│   │   │
│   │   ├── BeeCore.Camera/               Hardware capture
│   │   │   ├── HEROJE.cs, USB.cs, KEY_Send.cs (KeySend P6.4), HardwareEnum.cs
│   │   │   ├── Camera.cs                 (← BeeCore/Func/Camera.cs)
│   │   │   ├── CameraIOFast.cs           (← BeeCore/Func)
│   │   │   └── Drivers/                  About, DevState*, GetCfgCB, HJ_CRC32, LogRecord,
│   │   │                                  ModuleSetting, PnPEntityInfo, Protocol*Stu, *CB, ToolCfg
│   │   │
│   │   ├── BeeCore.IO/                   Persistence — CRITICAL invariants here
│   │   │   ├── Access.cs                 (BinaryFormatter+Base64 — DO NOT MOVE namespace)
│   │   │   ├── SaveData.cs, LoadData.cs
│   │   │   ├── ClassProject.cs           (kept as empty shell or removed)
│   │   │   ├── DB.cs                     (← BeeCore root)
│   │   │   ├── LibreTranslateClient.cs   (← BeeCore root)
│   │   │   ├── TarProgramHelper.cs       (← BeeCore/Func)
│   │   │   └── ImagePersistence.cs       (extracted from Vision IO calls)
│   │   │
│   │   ├── BeeCore.Comm/                 Protocol & PLC bridges
│   │   │   ├── Communication.cs          (← Comunication.cs renamed P6.2)
│   │   │   └── EtherNetIP/               AssemblyHelper, EDS, EIPAdapter, EIPScanner
│   │   │
│   │   └── BeeCore.AI/                   Native ML inference
│   │       ├── NativeYolo.cs, NativeRCNN.cs, Native.cs
│   │       ├── Init.cs, Logs.cs          (← BeeCore/Func)
│   │       └── Helpers/                  CodeSymbologyCliExtensions, ComboBoxExtensions,
│   │                                      Converts (merged), General, ResultItem helpers
│   │
│   ├── Tools/                            ← LAYER 3 — one folder per tool
│   │   ├── Tool.Common/
│   │   │   ├── IToolEngine.cs, IToolConfig.cs, IToolResult.cs
│   │   │   ├── ToolEngineBase.cs
│   │   │   ├── ToolViewBase.cs (+Designer)         (← BeeInterface/Tool/_Base)
│   │   │   ├── IToolView.cs, ToolTabContext.cs, ToolTabRegistry.cs
│   │   │   ├── GroupBToolTabRegistrar.cs
│   │   │   ├── ToolEngineFactory.cs
│   │   │   └── ToolCustomLayoutAttribute.cs
│   │   │
│   │   ├── Tool.Circle/                  ← TEMPLATE for all tools
│   │   │   ├── CircleConfig.cs                     (extracted from Circle.cs payload)
│   │   │   ├── CircleEngine.cs                     (was CircleEngineRunner.cs)
│   │   │   ├── CircleForm.cs (+Designer + .resx)   (was ToolCircle.cs)
│   │   │   └── Controls/
│   │   │       └── CircleParamsControl.cs
│   │   │
│   │   ├── Tool.Width/                   same 3-file pattern
│   │   ├── Tool.Measure/
│   │   ├── Tool.Edge/
│   │   ├── Tool.EdgePixel/
│   │   ├── Tool.Corner/                  uses MeasureCorner payload
│   │   ├── Tool.Crop/
│   │   ├── Tool.Intersect/
│   │   ├── Tool.ColorArea/
│   │   ├── Tool.Counter/                 (status per P3L.8 decision)
│   │   ├── Tool.CheckMissing/
│   │   ├── Tool.PositionAdjustment/      (renamed P6.4)
│   │   ├── Tool.Barcode/
│   │   ├── Tool.OCR/
│   │   ├── Tool.CraftOCR/
│   │   ├── Tool.Pattern/                 dynamic Preset + Preprocess tabs
│   │   ├── Tool.MultiPattern/
│   │   ├── Tool.MatchingShape/
│   │   ├── Tool.Pitch/                   PitchGdiPainter promoted to Controls/
│   │   ├── Tool.Yolo/                    [ToolCustomLayout] OR migrated per P3L.31
│   │   ├── Tool.MultiOnnx/                [ditto P3L.32]
│   │   ├── Tool.OKNG/                     [ditto P3L.33]
│   │   ├── Tool.AutoTrig/                 [ditto P3L.34]
│   │   └── Tool.VisualMatch/              [ditto P3L.35]
│   │
│   ├── UI/                               ← LAYER 4 — application shell
│   │   ├── BeeUi/                        Main forms only
│   │   │   ├── Forms/
│   │   │   │   ├── Main.cs (+Designer)
│   │   │   │   ├── FormLoad.cs (+Designer)
│   │   │   │   ├── FormActive.cs (+Designer)
│   │   │   │   ├── ScanCCD.cs (+Designer)
│   │   │   │   └── ShowEraser.cs (+Designer)
│   │   │   ├── EditTool.cs               (← BeeUi/Unit)
│   │   │   ├── PCITriggerEngine.cs
│   │   │   ├── DataContractAttribute.cs  (← BeeUi/Tool)
│   │   │   └── Python/                   Learning.py, OcrWapper.py
│   │   │
│   │   └── BeeInterface/                 Shell host — NO ToolXxx.cs anymore
│   │       ├── Forms/
│   │       │   ├── FormFlowChart.cs (+Designer)
│   │       │   ├── FormWarning.cs (+Designer)
│   │       │   ├── ForrmAlarm.cs (+Designer) (rename → FormAlarm in P6.x)
│   │       │   ├── FormCheckUpdate.cs, FormChoose.cs, FormReport.cs (← General/)
│   │       │   ├── GeneralSetting.cs, QuickSetting.cs (← root + General/)
│   │       │   └── Account.cs (← General/)
│   │       ├── Dashboard/                DashboardImages, DashboardListCompact, FlipClockControl,
│   │       │                              RegisterImgDashboard, ReportDashBoard, SaveProgressDialog,
│   │       │                              StatusDashboard
│   │       ├── Steps/                    SettingStep1/2/4, StepEdit
│   │       ├── ToolHost/                 ToolPage, MultiDockHost, ShowTool, EditBar, ViewHost,
│   │       │                              ToolSettings, BarRight, BtnHeaderBar, Header, HideBar,
│   │       │                              StatusBar, InforBar, View
│   │       ├── ProgramEdit/              EditProg, AddTool, AddFilters, EditRectRot, RegisterImgs,
│   │       │                              SimImgs, ucReport
│   │       ├── ItemViews/                ItemValue, ItemLogic, ItemRS, InforGroup, MessageChoose
│   │       ├── PLC/                      ProtocolPLC, ucBitInput, ucBitOutput, ucValueOutput,
│   │       │                              WriteValuePLCSystems
│   │       ├── Helpers/                  AppRestart, FileName, Load, Tools, DataTool,
│   │       │                              GoogleDriveDllManager, KbdListener, KeyAcitve,
│   │       │                              RsDirPermissions, Cryto, Decompile, Func, Gui, SqlServer
│   │       └── GroupControl/             OK_Cancel
│   │
│   └── Native/
│       ├── Pattern/                      BeeCpp.dll
│       ├── BeeCV/
│       ├── BeeNativeOnnx/                [enabled when OpenVINO provided]
│       ├── BeeNativeRCNN/
│       ├── OKNG/
│       ├── PylonCli/
│       ├── ColorPixelsCPP/
│       └── BeeOnnxCLi/
│
├── tests/
│   ├── BeeCore.Persistence.Tests/        P3L.9 — .prog round-trip
│   ├── BeeCore.Algorithms.Tests/         P5.4
│   ├── BeeCore.Domain.Tests/             P5.4 — resolver helpers
│   └── Tool.Circle.Tests/                P3L.12 — pilot
│
├── tools/                                automation scripts
│   ├── check_propety_tools.ps1
│   ├── check_event_pairs.ps1
│   ├── run_persistence_tests.ps1
│   ├── build_full.ps1
│   └── gen_typeforward.ps1
│
└── docs/
    └── architecture/
        ├── CODEX_HISTORY.md
        ├── baseline_build.md
        ├── tasks/
        ├── adr/
        └── preview/
            ├── code_map.json
            ├── CODE_PREVIEW.md
            ├── TASK_CARDS_P3_LITE.md
            ├── NEXT_PLAN_FOR_CODEX.md
            └── FULL_PROJECT_REORG_PLAN.md
```

### 16.3 Layer dependency rule (target)

```
                 ┌─────────────────────┐
                 │   App (BeeMain)     │
                 └──────────┬──────────┘
                            ▼
                 ┌─────────────────────┐
                 │   UI shell          │  BeeUi, BeeInterface
                 │  (Forms only)       │
                 └──────────┬──────────┘
                            ▼
                 ┌─────────────────────┐
                 │   Tools             │  Tool.Circle, Tool.Pattern, …
                 │  (Config+Engine+    │
                 │   Form)             │
                 └─────┬───────┬───────┘
                       ▼       ▼
              ┌──────────────────────────┐
              │   Core                   │  BeeCore.{Domain, Algorithms,
              │  (no Forms references)   │   Vision, Camera, IO, Comm, AI}
              └────────────┬─────────────┘
                           ▼
              ┌──────────────────────────┐
              │   Shared                 │  BeeGlobal, BeeShared.UI
              │  (no Core references)    │
              └────────────┬─────────────┘
                           ▼
              ┌──────────────────────────┐
              │   Native (C++)           │
              └──────────────────────────┘
```

**Forbidden directions** (would cause circular ref):
- Core → UI shell.
- Shared → Core.
- Tool.X → Tool.Y (tools are siblings; cross-tool logic must live in `BeeCore.Vision` or `Tool.Common`).
- BeeGlobal → any UI assembly.
- BeeCore.Domain → OpenCvSharp (Domain stays POCO).

### 16.4 Per-tool target structure (all 24 tools, identical pattern)

```
src/Tools/Tool.<Name>/
├── <Name>Config.cs             ← POCO. Field layout = field layout in BeeCore/Unit/<Name>.cs.
│                                  In Block H (P6.1) Property family is renamed; until then
│                                  Config holds a reference to the Propety/Propety2 payload
│                                  unchanged.
├── <Name>Engine.cs             ← Logic. Was *EngineRunner.cs in P3L.2-4.
│                                  Pure: takes Config + Mat → Result. NO System.Windows.Forms.
├── <Name>Form.cs               ← View. Inherits ToolViewBase.
├── <Name>Form.Designer.cs      ← Auto-generated.
├── <Name>Form.resx             ← Resources.
├── Controls/                   ← Tool-specific UCs (non-shared)
│   └── <Name>ParamsControl.cs  ← Hosted in Params tab.
└── (Tests live in tests/Tool.<Name>.Tests/)
```

### 16.5 File-count delta (current → target)

| Project | Current files | Target files | Delta | Notes |
|---|---|---|---|---|
| BeeMain (root) | 1 (Program.cs) | 1 | 0 | move into `src/App/BeeMain/` |
| BeeGlobal | ~30 | ~25 | −5 | UI helpers move out |
| BeeCore root | 17 | 0 | −17 | all moved into sub-projects |
| BeeCore.Domain | — | ~50 | +50 | new |
| BeeCore.Algorithms | — | 16 | +16 | folder lift |
| BeeCore.Vision | — | ~30 | +30 | combines `Func/*` + `ShapeEditing/*` |
| BeeCore.Camera | — | ~22 | +22 | folder lift |
| BeeCore.IO | — | 7 | +7 | new |
| BeeCore.Comm | — | 5 | +5 | new |
| BeeCore.AI | — | ~10 | +10 | new |
| BeeShared.UI | — | ~25 | +25 | new |
| BeeInterface root | 19 | 0 | −19 | all moved to subfolders or BeeShared.UI |
| BeeInterface/Tool/ | 24 + Designers | 0 | −24 | each becomes Tool.<Name> |
| Tool.<Name> | — | 24 × 4 files | +96 | one folder per tool |
| BeeUi root | 9 | 0 | −9 | all moved into Forms/ subfolder |

### 16.6 Migration order anchors (which Block does each move)

| Source | Destination | Done by Block |
|---|---|---|
| `BeeCore/Func/CustomGui.cs` + `BeeInterface/CustomGui.cs` | `BeeShared.UI/CustomGui.cs` | F (P4.1) preceded by P3L.10 ref-map |
| `BeeCore/Converts.cs` + `BeeCore/Func/Converts.cs` | `BeeCore.AI/Helpers/Converts.cs` (or Domain) | F (P4.1) |
| `BeeCore/ShapeEditing/` + `BeeInterface/ShapeEditing/` | `BeeCore.Vision/ShapeEditing/` | D (P2.3) |
| `BeeGlobal/Common/Adjust*.cs` | `BeeShared.UI/Controls/` | F (P4.1) |
| `BeeCore/Unit/*.cs` payload classes | `BeeCore.Domain/Persisted/` (NO field changes) | D (P2.1) — gated by persistence test |
| `BeeCore/Func/Engines/*Runner.cs` | `Tool.<Name>/Engine.cs` | I (I.1) — after Block B slimming |
| `BeeInterface/Tool/Tool<Name>.cs` | `Tool.<Name>/Form.cs` | A → I |
| `BeeInterface/Tool/_Base/` | `Tool.Common/` | I (I.1) |
| `BeeInterface/Custom/*.cs` | `BeeShared.UI/Controls/` | F (P4.1) |
| `BeeUi/*.cs` (root) | `BeeUi/Forms/` | I (I.1) |
| Three `Global.cs` | split into `BeeShared.UI/UiState.cs` + `BeeCore.Domain/RuntimeState.cs` + `BeeGlobal/Config.cs` | F (P4.3) |

### 16.7 Visual block-by-block progression

```
NOW (2026-05-03)        Block 0          Block A          Block B          Block D          Block I
                        finishes P3L     hosts Group B    slimming         splits Core      target tree
─────────────────       ─────────────    ─────────────    ─────────────    ─────────────    ─────────────
BeeCore (god)           BeeCore (god)    BeeCore (god)    BeeCore (god)    BeeCore.Domain   src/Core/
BeeInterface            BeeInterface     BeeInterface     BeeInterface     BeeCore.*  ×7   src/Tools/
 ├ Tool/(24 UC)          ├ Tool/(24 UC)   ├ Tool/(24 UC)   ├ Tool/(slim)    ├ Tool/(slim)   src/UI/
 └ Custom/               └ Custom/        └ Custom/         └ Custom/        └ Custom/      src/Shared/
BeeUi (forms)           BeeUi            BeeUi            BeeUi            BeeUi           src/App/
                        + tests/         + tests/         + tests/         + tests/        + tests/
                        + Release log    8 of 8 Group B   all under LOC    BeeCore = shim  full target
                        + persistence    on ToolViewBase   budget          all forwards    layout
```

### 16.8 Quick-glance table — what changes for each tool

| Tool (24) | Current file | Target folder | Group | Block touching it |
|---|---|---|---|---|
| Circle | BeeInterface/Tool/ToolCircle.cs | Tool.Circle/ | A | P3L.12 (slim) → I |
| Width | BeeInterface/Tool/ToolWidth.cs | Tool.Width/ | A | B (slim) → I |
| Measure | BeeInterface/Tool/ToolMeasure.cs | Tool.Measure/ | A | B → I |
| Edge | BeeInterface/Tool/ToolEdge.cs | Tool.Edge/ | A | B → I |
| EdgePixel | BeeInterface/Tool/ToolEdgePixel.cs | Tool.EdgePixel/ | A | B → I |
| Corner | BeeInterface/Tool/ToolCorner.cs | Tool.Corner/ | A | B → I |
| Crop | BeeInterface/Tool/ToolCrop.cs | Tool.Crop/ | A | B → I |
| Intersect | BeeInterface/Tool/ToolIntersect.cs | Tool.Intersect/ | A | B → I |
| ColorArea | BeeInterface/Tool/ToolColorArea.cs | Tool.ColorArea/ | A | B → I |
| Counter | BeeInterface/Tool/ToolCounter.cs | Tool.Counter/ | A | P3L.8 decides → I |
| CheckMissing | BeeInterface/Tool/ToolCheckMissing.cs | Tool.CheckMissing/ | A | B → I |
| PositionAdj | BeeInterface/Tool/ToolPosition_Adjustment.cs | Tool.PositionAdjustment/ | A | P6.4 rename → I |
| Barcode | BeeInterface/Tool/ToolBarcode.cs | Tool.Barcode/ | B | P3L.7 → B → I |
| OCR | BeeInterface/Tool/ToolOCR.cs | Tool.OCR/ | B | P3L.11 → B → I |
| CraftOCR | BeeInterface/Tool/ToolCraftOCR.cs | Tool.CraftOCR/ | B | P3L.17 → B → I |
| Pattern | BeeInterface/Tool/ToolPattern.cs | Tool.Pattern/ | B | P3L.18 (last) → B → I |
| MultiPattern | BeeInterface/Tool/ToolMultiPattern.cs | Tool.MultiPattern/ | B | P3L.16 → B → I |
| MatchingShape | BeeInterface/Tool/ToolMatchingShape.cs | Tool.MatchingShape/ | B | P3L.14 → B → I |
| Pitch | BeeInterface/Tool/ToolPitch.cs | Tool.Pitch/ | B | P3L.15 → B → I |
| Yolo | BeeInterface/Tool/ToolYolo.cs | Tool.Yolo/ | C | P3L.31 → I |
| MultiOnnx | BeeInterface/Tool/ToolMultiOnnx.cs | Tool.MultiOnnx/ | C | P3L.32 → I |
| OKNG | BeeInterface/Tool/ToolOKNG.cs | Tool.OKNG/ | C | P3L.33 → I |
| AutoTrig | BeeInterface/Tool/ToolAutoTrig.cs | Tool.AutoTrig/ | C | P3L.34 → I |
| VisualMatch | BeeInterface/Tool/ToolVisualMatch.cs | Tool.VisualMatch/ | C | P3L.35 → I |

- `CLAUDE.md` — § 0 Hard Rules · § 3 target layout · § 5 detailed phase task cards · § 10 tab framework · § 11 templates · § 12 git workflow.
