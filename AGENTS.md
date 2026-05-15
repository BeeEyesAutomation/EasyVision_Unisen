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

## Agent Permission and Language Rules

- The agent has standing permission to inspect files, edit in-scope files, update maps/session logs, and run required non-destructive verification commands without asking first.
- Do not ask for confirmation before normal implementation, planning, refactoring, or build/test commands when they are within the selected map entry scope.
- Ask only when a repository stop condition is hit, when destructive filesystem or git history operations are requested, when a change would require out-of-scope files after a failed build, or when the requested behavior is technically ambiguous and a reasonable assumption would be risky.
- All agent-facing documentation must be written in English. This includes plans, map entries, AGENTS files, session logs, history notes, implementation notes, comments added for future agents, and handoff notes.
- Keep user-facing final summaries in the language the user uses, unless the user asks otherwise.

## Sparring Partner Rule

- When the user presents an idea, the first response must be three to five questions that challenge assumptions, uncover blind spots, and surface hidden risks before offering solutions or agreement.
- Challenge the idea constructively: focus on second-order effects, operational risks, technical ambiguity, opportunity cost, and failure modes without dismissing the user's intent.

---

## Daily Workflow (multi-step loop)

### Step 0 — Read history
1. Read `.codex-session.md` — find the last session block.
2. If a "Resume point" exists with a next ID, start from that ID.
3. Otherwise read all 6 maps and pick the highest-priority open entry.

### Step 1 — Before each ID
1. Check `Status` in the relevant map — confirm entry is still `open`.
2. Confirm no unresolved `Depends on` entry blocks this ID.
3. Rescan files only if `git log --since=1.day -- <files>` shows recent changes AND the entry is marked `stale`. Otherwise trust the existing entry.

### Work priority order
1. **Structure cleanup** — `Sxxx` open, low-risk first (includes duplicate extraction and dead code)
2. **Bug / regression / dead code** — `Rxxx` / `Ixxx` / `Vxxx` (confirmed breaks first, then dead code)
3. **Performance optimization** — `Oxxx`
4. **WinForms UI optimization** — `Uxxx`

### Picking one ID
- Select: `Status=open`, highest workflow priority, then lowest `Risk`.
- Skip if `Risk=high` — pick next.
- Skip if `Depends on` entry is still `open` — pick next.

---

## Build Rules — minimize build runs

**Never build for:**
- `.md`-only changes
- Adding/fixing `using` directives only (no logic change)
- Renaming a local variable inside one method
- Adding XML doc comments

**Build once per batch, not per file or per method:**
- Batch all changes of the same type together, then build once at the end.
- A "batch" is complete when all files in the entry's `Files` scope have been updated.

**Build individually only when:**
- Previous build failed and you need to isolate which change broke it
- The change touches a P/Invoke signature, native interop, or unmanaged memory
- The entry has `Risk=high`

**Build trigger table:**

| Change type | Build? |
| --- | --- |
| `.md` only | Never |
| `using` / doc comment only | Never |
| Dead code removal (1–5 methods, same file) | Once after all removals in that file |
| Dead code removal (multiple files) | Once after all files done |
| Duplicate extraction — add shared class + all replacements | Once after all replacements done |
| Duplicate extraction — partial (some replacements fail mid-way) | Build to isolate, fix, then continue |
| Bug fix in one method | Once after fix |
| Any P/Invoke, native interop, or unmanaged memory change | After every file touched |
| Risk=high entry | After every file touched |

---

## Step 2 — Implement

### Pre-flight before any extraction or removal (run once, not per method)
1. Collect all `using` namespaces needed by the target shared class — add them all upfront.
2. For duplicate extraction: read ALL copies of the method across all files first, confirm signatures are identical. If signatures differ, note the differences before writing anything.
3. For dead code: `grep -rn "<MethodName>" --include="*.cs" .` — confirm zero callers. If callers found, skip and mark entry `blocked`.

### Execution rules
- Minimal diff — no cleanup outside the entry's `Files` scope.
- **Dead code removal**: confirm zero callers → delete all target methods in one pass per file → build once after all files done.
- **Duplicate extraction**: create shared class with ALL methods first → replace all copies across all files → build once → only then remove originals if still present.
- Preserve C# ↔ C++ wrapper API: no P/Invoke signature changes.
- Preserve unmanaged memory ownership: `AllocHGlobal` paired with `FreeHGlobal` in `finally`.
- Preserve camera lifecycle: honour Start → Read → Stop sequence.
- Never block the UI thread: no synchronous SDK calls on WinForms dispatch thread.

---

## Step 3 — Verify

Build command:
```
& "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe" EasyVision.sln /t:Build /p:Configuration=Release /p:Platform=x64 /v:minimal
```

Only run this when the Build Rules table above says "build". Do not run it otherwise.

If build fails: fix the error, then build again. Do not revert unless the fix would require touching files outside the entry scope.

---

## Step 1b — Periodic dead code + duplicate scan
Run only when all existing Rxxx and Sxxx entries are `done` or `blocked`:

**Dead code:**
```
grep -rn "private.*void\|private.*bool\|private.*int\|private.*double" --include="*.cs" BeeCore/ BeeInterface/Tool/ BeeUi/
```
Verify each: `grep -rn "<MethodName>" --include="*.cs" .` returns only the definition line.
Confirmed dead → add new `Rxxx` (Status=open, Risk=low).

**Duplicates:**
```
grep -rh "void \|bool \|int \|double \|string " --include="*.cs" BeeCore/Algorithm/ BeeCore/Unit/ BeeCore/Func/ | grep -oP "(?<=void |bool |int |double |string )\w+(?=\()" | sort | uniq -c | sort -rn | head -30
```
Method in 3+ files with matching signatures → add new `Sxxx` (Risk=medium if 3-4 files, low if 2).
Only add confirmed issues — not speculative.

---

## Step 4 — Update map + session log
1. Set `Status=done` (or `partial`) in the affected map entry.
2. Increment `Next ID` in map header if a new entry was added.
3. Append to `.codex-session.md` — add row to Completed table, update Resume point.

---

## Step 5 — Loop or stop

Continue to the next ID unless a stop condition is met:

| Stop condition | Action |
| --- | --- |
| Build fails and fix requires touching out-of-scope files | Save state, ask user, stop |
| Risk escalates to `high` mid-task | Save state, ask user, stop |
| No more `open` entries | Run Step 1b; if nothing found write "All entries resolved", stop |
| Token budget running low | Save resume point to `.codex-session.md`, stop cleanly |

---

## Session log format

```
| <ID> | <phase> | <done|partial> — <pass/fail/n/a> | <changed files> |
```

Resume point block:
```
### Resume point
- **Next ID**: <Xxx> — reason
- **Blocker**: <none | description>
- **Notes**: <anything future-Claude/Codex needs to know>
```

---

## Required return format
```
ID: <Xxx>
Phase: <structure | bug | perf | ui>
Changed files: <list>
Verify result: <pass — N warnings> | <fail — reason> | <n/a — not required>
Next ID: <Xxx> — reason
```

---

## Stop-and-ask triggers
- Build fails and fix requires out-of-scope files.
- Risk rises to `high` mid-task.
- `Depends on` entry still `open`.
- Caller found during dead-code grep — method is not actually dead.
- Duplicate method signatures differ across files — cannot safely abstract.
