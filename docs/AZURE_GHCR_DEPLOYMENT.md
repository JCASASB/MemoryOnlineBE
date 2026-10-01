# Despliegue en Azure Container Apps desde GHCR

El workflow `deploy-azure-container-apps.yml` publica dos imágenes públicas en GitHub Container Registry y actualiza las Container Apps existentes al hacer `push` a `main`.

## Recursos de Azure usados

| Recurso | Nombre |
| --- | --- |
| Grupo de recursos | `MemoryOnlineResourceGroup` |
| API REST | `memory-online-backend` |
| Hub SignalR | `memory-online-be-signalr` |
| Azure SignalR Service | `mi-signalr-gratis` |

## Preparación única

1. En GitHub, cree el entorno `production` en **Settings > Environments**.
2. Configure OIDC entre GitHub y Azure para la rama `main`. La identidad debe tener el rol `Contributor` sobre `MemoryOnlineResourceGroup`.
3. En **Settings > Secrets and variables > Actions**, agregue estos secretos: `AZURE_CLIENT_ID`, `AZURE_TENANT_ID` y `AZURE_SUBSCRIPTION_ID`.
4. La primera ejecución publica los paquetes. Después, en GitHub **Profile or Organization > Packages**, cambie `memoryonline-api` y `memoryonline-hub` a visibilidad **Public**. A continuación, ejecute de nuevo el workflow desde la pestaña **Actions**. Las Container Apps podrán descargarlos sin credenciales.
5. En ambas Container Apps, configure los secretos reales de JWT y base de datos como variables de entorno. No use los valores de ejemplo de `appsettings.json` en producción.

## Configuración exclusiva del Hub

En `memory-online-be-signalr`, cree el secreto `azure-signalr-connection-string` con la cadena de conexión de `mi-signalr-gratis`. Cree después una variable de entorno llamada `Azure__SignalR__ConnectionString` que haga referencia a ese secreto.

El Hub se publica en `/gamehub`. El cliente se conecta a la URL pública de la Container App seguida de esa ruta; Azure SignalR Service redirige la conexión persistente de forma transparente.

## Ingress

En las dos Container Apps habilite ingress externo HTTP, con puerto de destino `8080`. La imagen define `ASPNETCORE_URLS=http://+:8080`.

## Despliegues

Cada commit enviado a `main` crea imágenes con el SHA del commit y con el tag `latest`, y actualiza las dos Container Apps.
