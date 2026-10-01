$ErrorActionPreference = "Stop"
$env:KIND_EXPERIMENTAL_PROVIDER = "podman"
$kind = (Get-Command kind -ErrorAction SilentlyContinue).Source
if (-not $kind) {
    $kind = (Get-ChildItem (Join-Path $env:LOCALAPPDATA "Microsoft\WinGet\Packages") -Recurse -Filter kind.exe | Select-Object -First 1).FullName
}
& $kind delete cluster --name memoryonline
