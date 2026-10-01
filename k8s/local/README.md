# Kubernetes local gratuito

Ejecuta la API, SignalR, MongoDB como replica set y OpenTelemetry Collector en un clúster `kind` sobre Podman. No modifica Azure y el collector solo escribe la telemetría en sus propios logs.

## Requisitos

- Podman Desktop o Podman CLI con una máquina creada.
- `kubectl` y `kind`.

```powershell
winget install Kubernetes.kubectl
winget install Kubernetes.kind
```

## Arrancar

```powershell
.\k8s\local\start.ps1
```

- API: `http://localhost:8088/api`
- SignalR: `http://localhost:8088/gamehub`

```powershell
kubectl get all -n memoryonline
kubectl logs -n memoryonline deployment/memoryonline-api -f
kubectl logs -n memoryonline deployment/memoryonline-signalr -f
kubectl logs -n memoryonline deployment/otel-collector -f
kubectl port-forward -n memoryonline service/mongodb 27017:27017
```

MongoDB desde el host, durante el `port-forward`:

`mongodb://admin:local-memoryonline-password@localhost:27017/memorydb?authSource=admin&replicaSet=rs0&directConnection=true`

## Eliminar

```powershell
.\k8s\local\stop.ps1
```

Al borrar el clúster también se elimina la base de datos local.
