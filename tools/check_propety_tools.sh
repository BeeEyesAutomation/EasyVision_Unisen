#!/bin/bash
# CI guard: chặn truy cập trực tiếp Common.PropetyTools[..] ngoài Common.cs.
# Convention: mọi consumer phải dùng helper resolver Common.TryGetTool / TryGetToolList /
#            EnsureToolList / EnsureCurrentToolList / SetToolList / TryGetCurrentToolList.
# Reference: CLAUDE.md §0.2.2, §P1.5.

set -u

PROJECTS="BeeCore BeeInterface BeeUi BeeMain BeeGlobal"

# Tìm mọi call site Common.PropetyTools[ trừ chính Common.cs.
matches=$(grep -rn "Common\.PropetyTools\[" \
              --include="*.cs" \
              --exclude-dir=bin --exclude-dir=obj --exclude-dir=Properties \
              $PROJECTS 2>/dev/null | \
          grep -v -E "(^|/)Common\.cs:")

if [ -n "$matches" ]; then
  count=$(echo "$matches" | wc -l)
  echo "ERROR: $count direct Common.PropetyTools[..] access outside Common.cs:"
  echo "$matches"
  exit 1
fi

echo "OK: no direct Common.PropetyTools[..] access outside Common.cs"
exit 0
