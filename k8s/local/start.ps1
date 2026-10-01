$ErrorActionPreference = "Stop"
$env:KIND_EXPERIMENTAL_PROVIDER = "podman"

$wingetPackages = Join-Path $env:LOCALAPPDATA "Microsoft\WinGet\Packages"
$kind = (Get-Command kind -ErrorAction SilentlyContinue).Source
$kubectl = (Get-Command kubectl -ErrorAction SilentlyContinue).Source
if (-not $kind) {
    $kind = (Get-ChildItem $wingetPackages -Recurse -Filter kind.exe | Select-Object -First 1).FullName
}
if (-not $kubectl) {
    $kubectl = (Get-ChildItem $wingetPackages -Recurse -Filter kubectl.exe | Select-Object -First 1).FullName
}
if (-not $kind -or -not $kubectl) {
    throw "No se encontraron kind y kubectl. Instálalos y abre una terminal nueva."
}

podman machine start 2>$null
if (-not (& $kind get clusters | Select-String -SimpleMatch "memoryonline")) {
    & $kind create cluster --name memoryonline --config "$PSScriptRoot/kind-config.yaml"
}

podman build -t localhost/memoryonline-api:local -f "$PSScriptRoot/../../src/01.Apis/MemoryOnline.Apis.WebApi/Dockerfile" "$PSScriptRoot/../.."
podman build -t localhost/memoryonline-signalr:local -f "$PSScriptRoot/../../src/01.Apis/MemoryOnline.Apis.Signalr/Dockerfile" "$PSScriptRoot/../.."
$apiArchive = Join-Path $env:TEMP "memoryonline-api.tar"
$signalrArchive = Join-Path $env:TEMP "memoryonline-signalr.tar"
podman save --format oci-archive -o $apiArchive localhost/memoryonline-api:local
podman save --format oci-archive -o $signalrArchive localhost/memoryonline-signalr:local
& $kind load image-archive $apiArchive --name memoryonline
& $kind load image-archive $signalrArchive --name memoryonline

& $kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.13.3/deploy/static/provider/kind/deploy.yaml
& $kubectl wait -n ingress-nginx --for=condition=Ready pod -l app.kubernetes.io/component=controller --timeout=180s
& $kubectl apply -k $PSScriptRoot
& $kubectl wait -n memoryonline --for=condition=Complete job/mongodb-replica-set-init --timeout=180s
& $kubectl rollout status -n memoryonline deployment/memoryonline-api --timeout=180s
& $kubectl rollout status -n memoryonline deployment/memoryonline-signalr --timeout=180s

Write-Host "API: http://localhost:8088/api"
Write-Host "SignalR: http://localhost:8088/gamehub"
