# Despliegue en Azure Container Apps

Guía para preparar Azure (pasos manuales, una sola vez) y dejar funcionando el despliegue automático con GitHub Actions.

## Arquitectura

- **memoryonline-webapi** → API REST (`src/01.Apis/MemoryOnline.Apis.WebApi/Dockerfile`)
- **memoryonline-signalr** → Hub SignalR (`src/01.Apis/MemoryOnline.Apis.Signalr/Dockerfile`)
- Imágenes en **GitHub Container Registry** (`ghcr.io/<tu-usuario>/memoryonline-webapi|signalr`)
- Base de datos: gestionada por ti (fuera de este flujo). Se inyecta como variable de entorno/secret en cada Container App.

## 1. Crear los recursos en Azure (una vez)

```powershell
# Variables (ajústalas)
$RG = "rg-memoryonline"
$LOCATION = "westeurope"
$ENV = "env-memoryonline"

az login
az group create --name $RG --location $LOCATION

# Extensión y entorno de Container Apps
az extension add --name containerapp --upgrade
az provider register --namespace Microsoft.App
az provider register --namespace Microsoft.OperationalInsights

az containerapp env create --name $ENV --resource-group $RG --location $LOCATION
```

## 2. Dar acceso de las Container Apps a ghcr.io

Las imágenes en ghcr.io son **privadas** por defecto. Dos opciones:

### Opción A (recomendada): credenciales de registro con un PAT de GitHub

1. En GitHub: **Settings → Developer settings → Personal access tokens (classic)** → crea un token con el scope `read:packages`.
2. Crea las Container Apps con ese registro:

```powershell
$GH_USER = "<tu-usuario-github>"
$GH_PAT  = "<tu-pat>"

az containerapp create --name memoryonline-webapi --resource-group $RG --environment $ENV `
  --image ghcr.io/$GH_USER/memoryonline-webapi:latest `
  --registry-server ghcr.io --registry-username $GH_USER --registry-password $GH_PAT `
  --target-port 8080 --ingress external --min-replicas 0 --max-replicas 2

az containerapp create --name memoryonline-signalr --resource-group $RG --environment $ENV `
  --image ghcr.io/$GH_USER/memoryonline-signalr:latest `
  --registry-server ghcr.io --registry-username $GH_USER --registry-password $GH_PAT `
  --target-port 8080 --ingress external --min-replicas 0 --max-replicas 2
```

### Opción B: hacer públicos los paquetes en ghcr.io

En GitHub → **Packages** → selecciona cada paquete → **Package settings → Change visibility → Public**. Entonces puedes omitir `--registry-*` al crear las apps.

## 3. Configurar la base de datos

Añade la cadena de conexión como secret/variable de entorno en cada app (ajusta el nombre de la variable al que use tu configuración, p. ej. `ConnectionStrings__DefaultConnection`):

```powershell
az containerapp secret set --name memoryonline-webapi --resource-group $RG `
  --secrets db-connection="<tu-cadena-de-conexion>"

az containerapp update --name memoryonline-webapi --resource-group $RG `
  --set-env-vars "ConnectionStrings__DefaultConnection=secretref:db-connection"
```

Repite para `memoryonline-signalr` si también la necesita.

## 4. Configurar GitHub Actions

### Secreto `AZURE_CREDENTIALS`

Crea un Service Principal con permisos sobre el grupo de recursos:

```powershell
az ad sp create-for-rbac --name "sp-memoryonline-gh" --role contributor `
  --scopes /subscriptions/<SUBSCRIPTION_ID>/resourceGroups/rg-memoryonline `
  --json-auth
```

Copia el JSON completo y guárdalo en el repositorio:
**Settings → Secrets and variables → Actions → New repository secret** → nombre `AZURE_CREDENTIALS`.

### Variables del workflow

En `.github/workflows/deploy-azure-container-apps.yml` ajusta si usaste otros nombres:

```yaml
env:
  AZURE_RESOURCE_GROUP: rg-memoryonline
  AZURE_CONTAINERAPPS_ENVIRONMENT: env-memoryonline
```

> El primer `az containerapp update` del workflow requiere que las apps ya existan (creadas en el paso 2). Si alguna no existe, créala antes con `az containerapp create`.

## 5. Desplegar

- Push a `main` → se construyen ambas imágenes, se suben a ghcr.io y se actualizan las dos Container Apps.
- Manual: pestaña **Actions → Deploy to Azure Container Apps → Run workflow** (puedes indicar una etiqueta de imagen personalizada).

## Verificación

```powershell
az containerapp show --name memoryonline-webapi --resource-group $RG --query properties.configuration.ingress.fqdn -o tsv
az containerapp logs show --name memoryonline-webapi --resource-group $RG --follow
```
