# 🎮 MemoryOnline Backend

Backend para el juego Memory Online construido con .NET 9, SignalR y desplegado en AWS EC2 con Cloudflare Tunnel.

## 🏗️ Arquitectura

```
┌─────────────────────────────────────────────────────────────┐
│                        Internet                              │
└──────────────────────┬──────────────────────────────────────┘
                       │
                       ├─> signalr.hispalance.com
                       │   (WebSocket para juego en tiempo real)
                       │
                       └─> api.hispalance.com
                           (REST API + Documentación)
                       │
            ┌──────────▼───────────┐
            │ Cloudflare Tunnel     │
            │ (SSL + DDoS)          │
            └──────────┬───────────┘
                       │
            ┌──────────▼───────────┐
            │   AWS EC2             │
            │                       │
            │  ┌─────────────────┐ │
            │  │ Docker:5000     │ │
            │  │ SignalR Hub     │ │
            │  └─────────────────┘ │
            │                       │
            │  ┌─────────────────┐ │
            │  │ Docker:5001     │ │
            │  │ WebApi          │ │
            │  └─────────────────┘ │
            └───────────────────────┘
```

## 🚀 URLs de Producción

| Servicio | URL | Descripción |
|----------|-----|-------------|
| **SignalR Hub** | `https://signalr.hispalance.com/gamehub` | WebSocket para juego en tiempo real |
| **WebApi** | `https://api.hispalance.com/api` | REST API |
| **Scalar Docs** | `https://api.hispalance.com/scalar/v1` | Documentación interactiva de API |
| **OpenAPI** | `https://api.hispalance.com/openapi/v1.json` | Especificación OpenAPI |

## 📦 Estructura del Proyecto

```
MemoryOnlineBE/
├── src/
│   ├── 01.Apis/
│   │   ├── MemoryOnline.Apis.Signalr/     # SignalR Hub
│   │   └── MemoryOnline.Apis.WebApi/      # REST API
│   ├── 02.Domain/                          # Modelos de dominio
│   ├── 03.Application/                     # Lógica de aplicación (CQRS)
│   └── 04.Infrastructure/                  # Repositorios, DB, etc.
├── .github/
│   ├── workflows/
│   │   └── deploy-ec2.yml                  # CI/CD con GitHub Actions
│   ├── scripts/
│   │   └── deploy.sh                       # Script de despliegue
│   └── docs/
│       └── CLOUDFLARE_TUNNEL_SETUP.md      # Guía de configuración
└── README.md
```

## 🛠️ Stack Tecnológico

- **.NET 9.0** - Framework principal
- **SignalR** - WebSocket para comunicación en tiempo real
- **Scalar** - Documentación interactiva de API
- **Docker** - Containerización
- **AWS EC2** - Hosting (Amazon Linux 2023)
- **Cloudflare Tunnel** - SSL, proxy reverso y protección DDoS
- **GitHub Actions** - CI/CD

## 🔧 Configuración Inicial

### 1. Secrets de GitHub

Configura estos secrets en tu repositorio:
- `EC2_USER` → Usuario SSH (ej: `ec2-user`)
- `EC2_SSH_KEY` → Clave privada SSH completa (.pem)
- `CLOUDFLARE_TOKEN` → Token del Cloudflare Tunnel

### 2. Cloudflare Tunnel

Configura dos subdominios en Cloudflare Zero Trust:

| Subdominio | Puerto | Service Type |
|------------|--------|--------------|
| `signalr.hispalance.com` | `localhost:5000` | HTTP (con WebSockets ✅) |
| `api.hispalance.com` | `localhost:5001` | HTTP |

👉 [Ver guía detallada](.github/docs/CLOUDFLARE_TUNNEL_SETUP.md)

## 📝 Deploy

El deploy es **automático** en cada push a `main`:

```bash
git add .
git commit -m "feat: nueva funcionalidad"
git push origin main
```

También puedes ejecutarlo manualmente:
1. Ve a **Actions** en GitHub
2. Selecciona **Deploy to EC2**
3. Click **Run workflow**

### Pipeline de Deploy

1. ✅ Build de proyectos .NET
2. ✅ Creación de imágenes Docker
3. ✅ Compresión y transferencia a EC2 vía SCP
4. ✅ Deploy de contenedores en EC2
5. ✅ Verificación de servicios

## 🧪 Desarrollo Local

### Requisitos
- .NET 9.0 SDK
- Docker (opcional)
- Visual Studio 2026 o VS Code

### Ejecutar localmente

```bash
# SignalR
cd src/01.Apis/MemoryOnline.Apis.Signalr
dotnet run

# WebApi (en otra terminal)
cd src/01.Apis/MemoryOnline.Apis.WebApi
dotnet run
```

Accede a:
- SignalR: `http://localhost:5000/gamehub`
- WebApi: `http://localhost:5001/api`
- Scalar: `http://localhost:5001/scalar/v1`

## 🔌 Ejemplo de Conexión SignalR

```csharp
using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("https://signalr.hispalance.com/gamehub")
    .WithAutomaticReconnect()
    .Build();

await connection.StartAsync();

// Enviar evento
await connection.InvokeAsync("JoinGame", "game-123");

// Escuchar eventos
connection.On<string>("GameStarted", (gameId) =>
{
    Console.WriteLine($"Juego {gameId} iniciado!");
});
```

## 📚 Documentación

- [Configuración de Cloudflare Tunnel](.github/docs/CLOUDFLARE_TUNNEL_SETUP.md)
- [Documentación de API (Scalar)](https://api.hispalance.com/scalar/v1)

## 🐛 Troubleshooting

### Ver logs en EC2

```bash
# Conectarse a EC2
ssh ec2-user@54.172.220.11

# Ver contenedores
sudo docker ps

# Ver logs de SignalR
sudo docker logs memoryonline-signalr

# Ver logs de WebApi
sudo docker logs memoryonline-webapi

# Reiniciar contenedor
sudo docker restart memoryonline-signalr
```

### Problemas comunes

| Problema | Solución |
|----------|----------|
| 502 Bad Gateway | El contenedor no está corriendo. Ver logs con `docker logs` |
| SignalR no conecta | Verifica que WebSockets esté habilitado en Cloudflare |
| CORS errors | Revisa configuración de CORS en `Program.cs` |

## 📄 Licencia

Este proyecto es privado y de uso exclusivo para MemoryOnline.

---

**Desarrollado con ❤️ usando .NET 9**
