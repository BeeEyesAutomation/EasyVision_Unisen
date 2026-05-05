# AGENTS.md

## Map Files (source of truth)

| File | Prefix | Next ID |
| --- | --- | --- |
| `.structure-map.md` | `S` | see map header |
| `.code-review-map.md` | `R` | see map header |
| `.interop-map.md` | `I` | see map header |
| `.vision-map.md` | `V` | see map header |
| `.optimization-map.md` | `O` | see map header |
| `.ui-map.md` | `U` | see map header |

## Daily Workflow

### Before starting
1. Read all 6 maps — check `Status` column for each entry.
2. Run `git log --since=1.day -- <files>` for any entry marked `stale`; rescan only those areas.
3. Do NOT rescan areas with no recent git activity — trust the existing entry.
4. Reuse existing IDs. Auto-assign next sequential ID only for newly confirmed issues.
5. No duplicate IDs across maps.

### Work priority order
1. **Structure cleanup** — `Sxxx` (open, low-risk)
2. **Bug / regression** — `Rxxx` / `Ixxx` / `Vxxx` (confirmed breaks first)
3. **Performance optimization** — `Oxxx`
4. **WinForms UI optimization** — `Uxxx`

### Picking one ID
- Select the entry with: `Status=open`, highest workflow priority above, then lowest `Risk`.
- If the top candidate is `Risk=high`, skip and pick the next.
- One ID per session — no multi-ID batching.

### Execution rules
- Work on **one ID only** per session.
- Minimal diff — no opportunistic cleanup outside the entry's scope.
- Preserve C# ↔ C++ wrapper API: no signature changes to P/Invoke declarations.
- Preserve unmanaged memory ownership: every `AllocHGlobal` must have a paired `FreeHGlobal` in a `finally` block.
- Preserve camera lifecycle: honour the Start → Read → Stop sequence per SDK contract.
- Never block the UI thread: no synchronous SDK calls on the WinForms message-dispatch thread.
- **Stop immediately** if build fails or risk escalates to `high` mid-task.

### After finishing
1. Build: `MSBuild EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal`
2. Manual smoke if the change is UI-visible.
3. Update the affected map entry: set `Status=done` (or `partial` if partially resolved).
4. If a new entry was added, increment `Next ID` in the map header.

### Required return format
```
ID: <Xxx>
Phase: <structure | bug | perf | ui>
Changed files: <list>
Verify result: <pass/fail — warning count>
Next suggested ID: <Xxx> — reason
```

## Stop-and-ask triggers
- Build fails after the change.
- Risk assessment rises to `high` mid-task.
- A file outside the entry's `Files` scope must be touched.
- The entry has an unresolved `Depends on` entry that is still `open`.
