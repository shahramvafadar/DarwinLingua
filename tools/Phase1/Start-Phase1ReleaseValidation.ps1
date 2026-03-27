[CmdletBinding()]
param(
    [string]$ContentPackageVersion = "TBD",
    [string]$ReleaseLabel = "Phase 1 MVP Candidate",
    [string]$Validator = "TBD",
    [string]$SupportedDeviceMatrix = "TBD",
    [string]$OutputRoot = "artifacts/release-validation",
    [string]$Configuration = "Debug",
    [string]$SolutionPath = "DarwinLingua.slnx"
)

$ErrorActionPreference = "Stop"
$scriptRoot = Split-Path -Parent $PSCommandPath
$repoRoot = Split-Path -Parent (Split-Path -Parent $scriptRoot)
Set-Location $repoRoot

$prepScript = Join-Path $scriptRoot "Invoke-Phase1ReleasePrep.ps1"
$notesScript = Join-Path $scriptRoot "New-Phase1ReleaseNotesDraft.ps1"
$worksheetTemplate = Join-Path $repoRoot "docs\44-Phase-1-Manual-Validation-Worksheet.md"

& $prepScript `
    -OutputRoot $OutputRoot `
    -Configuration $Configuration `
    -SolutionPath $SolutionPath `
    -ContentPackageVersion $ContentPackageVersion

$validationDirectory = Join-Path $repoRoot $OutputRoot
$latestRun = Get-ChildItem -Path $validationDirectory -Directory | Sort-Object Name -Descending | Select-Object -First 1

if ($null -eq $latestRun)
{
    throw "No validation run directory was created under $validationDirectory"
}

$worksheetCopyPath = Join-Path $latestRun.FullName "Phase1ManualValidationWorksheet.md"
Copy-Item -Path $worksheetTemplate -Destination $worksheetCopyPath -Force

& $notesScript `
    -ValidationRoot $OutputRoot `
    -ReleaseLabel $ReleaseLabel `
    -Validator $Validator `
    -SupportedDeviceMatrix $SupportedDeviceMatrix

Write-Host "Release-validation bundle ready in $($latestRun.FullName)"
Write-Host "Worksheet copy: $worksheetCopyPath"
