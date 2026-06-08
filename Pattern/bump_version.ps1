# Pre-build versioning for the Pattern (BeeCpp) project.
# Auto-increments VER_REV in version.h ONLY when source files actually change.
#
# Fixes over the previous inline one-liner:
#   1. version.h is EXCLUDED from the hashed set -> no self-trigger / infinite bump.
#   2. version.h is ALWAYS (re)written full & valid -> self-heals empty/partial files.
#   3. No "$null -notmatch" PowerShell quirk and no [regex]::Match($null) crash.
#   4. Newlines via array + Set-Content (no single-quote backtick escaping bug).
#
# Invoked from Pattern.vcxproj PreBuildEvent as:
#   powershell -NoProfile -ExecutionPolicy Bypass -File "$(ProjectDir)bump_version.ps1"

$ErrorActionPreference = 'Stop'

$projectDir = $PSScriptRoot
$ver        = Join-Path $projectDir 'version.h'
$hashFile   = Join-Path $projectDir '.version.hash'

function Write-VersionHeader([string]$path, [int]$rev) {
    $lines = @(
        '#define VER_MAJOR 1',
        '#define VER_MINOR 0',
        '#define VER_BUILD 0',
        "#define VER_REV $rev",
        '#define STR_HELPER(x) #x',
        '#define STR(x) STR_HELPER(x)',
        '#define FILEVER_STR STR(VER_MAJOR) "." STR(VER_MINOR) "." STR(VER_BUILD) "." STR(VER_REV)',
        '#define VER_FILE_VERSION VER_MAJOR, VER_MINOR, VER_BUILD, VER_REV',
        '#define VER_FILE_VERSION_STR FILEVER_STR'
    )
    Set-Content -Encoding ascii -Path $path -Value $lines
}

# --- Hash all source files, excluding the generated version.h and build dirs ---
$files = Get-ChildItem $projectDir -Recurse -Include *.cpp,*.c,*.h,*.hpp,*.rc |
    Where-Object {
        ($_.FullName -notmatch '\\(Debug|Release|x64|Win32|ipch|\.git|\.vs)\\') -and
        ($_.Name -ne 'version.h')
    }

$concat = ''
foreach ($f in $files) { $concat += (Get-FileHash $f -Algorithm SHA256).Hash }
$hash = [BitConverter]::ToString(
    [Security.Cryptography.SHA256]::Create().ComputeHash(
        [Text.Encoding]::UTF8.GetBytes($concat))).Replace('-', '')

# --- Read current revision (default 0 if missing/empty/malformed) ---
$rev = 0
if (Test-Path $ver) {
    $raw = Get-Content $ver -Raw
    if ($raw) {
        $m = [regex]::Match($raw, '#define\s+VER_REV\s+(\d+)')
        if ($m.Success) { $rev = [int]$m.Groups[1].Value }
    }
}

# --- Does version.h need regeneration even if nothing changed? (self-heal) ---
$needsRegen = $true
if (Test-Path $ver) {
    $raw = Get-Content $ver -Raw
    if ($raw -and ($raw -match '#define\s+VER_MAJOR') -and ($raw -match '#define\s+VER_REV\s+\d+')) {
        $needsRegen = $false
    }
}

$hashOld = ''
if (Test-Path $hashFile) { $hashOld = (Get-Content $hashFile -Raw).Trim() }

if ($hashOld -eq $hash) {
    # No source change: heal version.h if broken, but do NOT bump.
    if ($needsRegen) { Write-VersionHeader $ver $rev }
    exit 0
}

# Source changed: bump revision and write a full, valid header.
Write-VersionHeader $ver ($rev + 1)
Set-Content -Encoding ascii -Path $hashFile -Value $hash
exit 0
