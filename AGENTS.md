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

## Session Log

Progress is saved to `.codex-session.md` after every completed ID.
On restart (new session or token limit hit), read the last block in `.codex-session.md` to find the resume point before touching any map or code.

---

## Daily Workflow (multi-step loop)

### Step 0 — Read history
1. Read `.codex-session.md` — find the last session block.
2. If a "Resume point" exists with a next ID, start from that ID.
3. Otherwise read all 6 maps and pick the highest-priority open entry.

### Step 1 — Before each ID
1. Check `Status` column in the relevant map — confirm entry is still `open`.
2. Run `git log --since=1.day -- <files>` for entries marked `stale`; rescan only those.
3. Confirm no unresolved `Depends on` entry blocks this ID.

### Work priority order
1. **Structure cleanup** — `Sxxx` (open, low-risk)
2. **Bug / regression** — `Rxxx` / `Ixxx` / `Vxxx` (confirmed breaks first)
3. **Performance optimization** — `Oxxx`
4. **WinForms UI optimization** — `Uxxx`

### Picking one ID
- Select: `Status=open`, highest workflow priority, then lowest `Risk`.
- Skip if `Risk=high` — pick next.
- If `Depends on` entry is still `open`, skip and pick next.

### Step 2 — Implement
- Minimal diff — no opportunistic cleanup outside the entry's `Files` scope.
- Preserve C# ↔ C++ wrapper API: no P/Invoke signature changes.
- Preserve unmanaged memory ownership: `AllocHGlobal` paired with `FreeHGlobal` in `finally`.
- Preserve camera lifecycle: honour Start → Read → Stop sequence.
- Never block the UI thread: no synchronous SDK calls on WinForms dispatch thread.

### Step 3 — Verify
Build command (required for any `.cs` / `.cpp` / `.csproj` / `.vcxproj` change):
```
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal
```
Doc-only changes (`.md` only): set verify result to `n/a — doc-only` and skip build.

### Step 4 — Update map + session log
1. Set `Status=done` (or `partial`) in the affected map entry.
2. Increment `Next ID` in map header if a new entry was added.
3. **Append** to `.codex-session.md`:
   - If today's session block already exists, add a row to its "Completed" table and update "Resume point".
   - If no block for today exists, create a new `## YYYY-MM-DD — Session N` block.

### Step 5 — Loop or stop
Continue immediately to the next ID **unless** any stop condition is met:

| Stop condition | Action |
| --- | --- |
| Build fails | Save state to `.codex-session.md`, report blocker, stop |
| Risk escalates to `high` mid-task | Save state, ask user, stop |
| File outside entry scope must be touched | Save state, ask user, stop |
| No more `open` entries in any map | Write "All entries resolved" in session log, stop |
| Token budget running low | Save resume point to `.codex-session.md`, stop cleanly |

---

## Session log format (append per completed ID)

Add to the current session block's "Completed" table:
```
| <ID> | <phase> | <done|partial> — <build pass/n/a> | <changed files> |
```

Update "Resume point" to the next ID after each step:
```
### Resume point
- **Next ID**: <Xxx> — reason
- **Blocker**: <none | description>
- **Notes**: <anything future-Codex needs to know>
```

---

## Required return format (printed after every ID, before looping)
```
ID: <Xxx>
Phase: <structure | bug | perf | ui>
Changed files: <list>
Verify result: <pass — N warnings> | <fail — reason> | <n/a — doc-only>
Next ID: <Xxx> — reason
```

---

## Stop-and-ask triggers
- Build fails after a code change.
- Risk assessment rises to `high` mid-task.
- A file outside the entry's `Files` scope must be touched.
- `Depends on` entry is still `open`.
- Unresolved ambiguity about ownership or API contract.
