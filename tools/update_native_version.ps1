param(
    [Parameter(Mandatory = $true)]
    [string] $VersionFile,

    [int] $Major = 1,
    [int] $Minor = 1,
    [int] $DefaultBuild = 1
)

$ErrorActionPreference = 'Stop'

$build = $DefaultBuild
if (Test-Path -LiteralPath $VersionFile) {
    $existing = Get-Content -LiteralPath $VersionFile -Raw -ErrorAction SilentlyContinue
    if (-not [string]::IsNullOrWhiteSpace($existing)) {
        $match = [regex]::Match($existing, 'VER_BUILD\s+(\d+)')
        if ($match.Success) {
            $build = [int]$match.Groups[1].Value + 1
        }
    }
}

$revision = Get-Date -Format 'yyMMdd'
$content = @(
    '#pragma once',
    '',
    "#define VER_MAJOR $Major",
    "#define VER_MINOR $Minor",
    "#define VER_BUILD $build",
    "#define VER_REVISION $revision",
    '',
    '#define STRINGIZE2(x) #x',
    '#define STRINGIZE(x) STRINGIZE2(x)',
    '',
    '#define VER_FILEVERSION_STR STRINGIZE(VER_MAJOR) "." STRINGIZE(VER_MINOR) "." STRINGIZE(VER_BUILD) "." STRINGIZE(VER_REVISION)',
    '#define VER_PRODUCTVERSION_STR VER_FILEVERSION_STR'
)

$directory = Split-Path -Parent $VersionFile
if (-not [string]::IsNullOrEmpty($directory) -and -not (Test-Path -LiteralPath $directory)) {
    New-Item -ItemType Directory -Path $directory | Out-Null
}

$lastError = $null
for ($attempt = 1; $attempt -le 5; $attempt++) {
    try {
        Set-Content -LiteralPath $VersionFile -Value $content -Encoding ASCII
        exit 0
    }
    catch {
        $lastError = $_
        Start-Sleep -Milliseconds (100 * $attempt)
    }
}

throw $lastError
