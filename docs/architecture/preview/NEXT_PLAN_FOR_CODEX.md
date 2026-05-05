# Next Plan for Codex (after 2026-05-03 baseline)

> Read order: (1) `CLAUDE.md` section 0, (2) last 3 entries of `CODEX_HISTORY.md`, (3) this file. Append history entry after each task using template 11.2 in `CLAUDE.md`.

---

## 0. State (2026-05-03)

**Done**: P3L.0 base framework · P3L.1 shared UCs · P3L.2/3/4 runner facades for 10 Group A tools · P3L.5 Group B tab registrar (scaffold only — no tool hosts it yet) · P3L.6 dedup investigated, deferred · BeeInterface Debug\|x64 build green.

**Open**:
- Group B tools still inherit `UserControl` → registered tabs are dead UI.
- `ToolCounter` orphaned (wired to Yolo, `CounterInfor.*` deleted).
- Pattern2 dirty in worktree, blocks Pattern migration.
- Release\|x64 not verified.
- Runner facades did not reduce LOC (UI still mixed with engine plumbing).
- No persistence test for `.prog` round-trip.
- `CustomGui` / `Converts` duplicates not 1:1, need reference map.

**Hard invariants** (never change): `BeeCore/Unit/*` field layout · 20 frozen keys in `PropetyTool.GetObjectData` · public API of `BeeCore.Common` · zero `Common.PropetyTools[...]` outside `Common.cs` · event `-=` before `+=`.

---

## 1. Next batch (P3L.7 → P3L.13)

| # | Task | Goal | In-scope | Stop-and-ask |
|---|---|---|---|---|
| P3L.7 | Group B pilot — Barcode | Host `ToolBarcode` on `ToolViewBase`, prove dynamic Preset tab visible | `ToolBarcode.cs`, new `BarcodeEngineRunner.cs` | If `Unit/Barcode.cs` is dirty, stash first |
| P3L.8 | Resolve ToolCounter | Pick: Yolo-aux / rewire to `Counter` / deprecate | `ToolCounter.cs` (+ runner if rewire) | **YES** — user picks option |
| P3L.9 | Persistence safety net | xUnit project for `.prog` round-trip + frozen keys check | new `tests/BeeCore.Persistence.Tests/` | Need a sample `.prog` from user |
| P3L.10 | CustomGui / Converts ref map | Doc-only usage map to unblock dedup | new `*_USAGE_MAP.md` | — |
| P3L.11 | Group B pilot 2 — OCR | Host `ToolOCR` with Preset + Preprocess tabs | `ToolOCR.cs`, new `OCREngineRunner.cs` | OCR or CraftOCR first? |
| P3L.12 | Slimming protocol + Circle | Define LOC budget (≤ 250) and apply to `ToolCircle` | new `TOOL_SLIMMING_PROTOCOL.md`, `ToolCircle.cs`, `CircleEngineRunner.cs` | Budget realistic? |
| P3L.13 | Release\|x64 baseline | Restore Release log + numbers | `docs/architecture/baseline_build.md`, log file, minimal csproj patches | — |

**Per-task rules**: one commit · in-scope only · build + persistence test green · history entry appended.

---

## 2. Roadmap after P3L.13

| Block | Cards | Goal |
|---|---|---|
| **A** Finish Group B | P3L.14 MatchingShape → P3L.15 Pitch → P3L.16 MultiPattern → P3L.17 CraftOCR → P3L.18 Pattern (last, after Pattern2 stable) | All 8 Group B tools host dynamic tabs |
| **B** Slimming pass | P3L.19 → P3L.30 (one per migrated tool) | Every `ToolXxx.cs` ≤ LOC budget; engine code in runners |
| **C** Group C decisions | P3L.31 → P3L.35 for Yolo / MultiOnnx / OKNG / AutoTrig / VisualMatch | Each tagged `[ToolCustomLayout]` or `[Obsolete]` — STOP and ask per tool |
| **D** Split BeeCore | P2.0 shim infra → P2.1 Domain → P2.2 Algorithms → P2.3 Vision → P2.4 Camera → P2.5 IO (gate: persistence test) → P2.6 Comm → P2.7 AI → P2.8 BeeCore = pure shim | 7 sub-projects + `[TypeForwardedTo]` |
| **E** Tooling/CI (parallel D) | check_propety_tools.ps1 · check_event_pairs.ps1 · run_persistence_tests.ps1 · build workflow | Manual guards become automatic |
| **F** UI shell cleanup | P4.1 move `Custom/*` to new `BeeShared.UI` · P4.2 sort `BeeUi/Unit/*` · P4.3 consolidate 3 `Global.cs` | Clean UI layer |
| **G** Native + tests | P5.1 dedup Pattern vs BeeCV · P5.2 BeeCV C++17 · P5.3 one header per DLL · P5.4 expand `tests/` | Native concerns separated, tested |
| **H** Renames (LAST, 1 PR each) | P6.1 Propety→Property (with `[Obsolete]` shim) · P6.2 Comunication→Communication · P6.3 dataMat→DataMat · P6.4 underscored filenames | Naming consistency |

**Final state**: target layout in `CLAUDE.md` section 3 · every tool = Config + Engine + View · `BeeCore` is shim · CI gates each commit · renames done.

---

## 3. Decisions to surface to user

1. Pattern2 stability date (blocks Block A finish).
2. `BeeInterface/Group/View.cs` re-encode to UTF-8? (separate one-shot task).
3. `BeeCore/Unit/Edge.cs` and `Width.cs` pre-existing local edits — review or stash?
4. `ToolCounter` option a/b/c (Task P3L.8).
5. Group B order after Barcode + OCR confirmed?
6. LOC ceiling 250 for `ToolXxx.cs` — accept or per-tool budgets?
7. CI now or after Block D?

---

## 4. Cross-references

- `code_map.json` · `CODE_PREVIEW.md` · `TASK_CARDS_P3_LITE.md` — original preview & cards.
- `CODEX_HISTORY.md` — canonical log of completed work.
- `CLAUDE.md` — § 0 Hard Rules · § 5 full task cards · § 10 tab spec · § 11 templates.
