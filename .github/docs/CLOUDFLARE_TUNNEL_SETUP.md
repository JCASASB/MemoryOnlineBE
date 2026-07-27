# 🌐 Configuración de Cloudflare Tunnel con Subdominios

Esta guía explica cómo configurar Cloudflare Tunnel para exponer tus servicios usando subdominios.

## 📋 Arquitectura

```
Internet
   │
   ├─> signalr.hispalance.com → EC2:5000 (SignalR + WebSocket)
   │
   └─> api.hispalance.com     → EC2:5001 (WebApi + Scalar)
```

## 🚀 Paso 1: Crear el Tunnel en Cloudflare

### Opción A: Desde Cloudflare Dashboard

1. Ve a [Cloudflare Zero Trust Dashboard](https://one.dash.cloudflare.com/)
2. En el menú lateral: **Networks** → **Tunnels**
3. Click en **Create a tunnel**
4. Selecciona **Cloudflared**
5. Nombre del tunnel: `memoryonline-ec2`
6. Click **Save tunnel**

### Opción B: Usar tunnel existente

Si ya tienes un tunnel configurado en tu EC2, salta al Paso 2.

---

## 🔧 Paso 2: Configurar Public Hostnames

### 2.1 Configurar SignalR (WebSocket)

1. En la pestaña **Public Hostname**, click **Add a public hostname**

**Configuración:**
```
┌─────────────────────────────────────────────┐
│ Subdomain:  signalr                         │
│ Domain:     hispalance.com                  │
│ Path:       (vacío)                         │
│                                             │
│ Service:                                    │
│   Type:     HTTP                            │
│   URL:      localhost:5000                  │
│                                             │
│ Additional settings:                        │
│   ✅ No TLS Verify                          │
│   ✅ HTTP2 Connection                       │
│   ✅ WebSockets                             │
└─────────────────────────────────────────────┘
```

2. Click **Save hostname**

### 2.2 Configurar WebApi

1. Click **Add a public hostname** de nuevo

**Configuración:**
```
┌─────────────────────────────────────────────┐
│ Subdomain:  api                             │
│ Domain:     hispalance.com                  │
│ Path:       (vacío)                         │
│                                             │
│ Service:                                    │
│   Type:     HTTP                            │
│   URL:      localhost:5001                  │
│                                             │
│ Additional settings:                        │
│   ✅ No TLS Verify                          │
│   ✅ HTTP2 Connection                       │
└─────────────────────────────────────────────┘
```

2. Click **Save hostname**

---

## 🔑 Paso 3: Obtener el Token del Tunnel

1. En la página del tunnel, click en **Configure**
2. Copia el **tunnel token** (empieza con `eyJh...`)
3. Guarda este token como secret `CLOUDFLARE_TOKEN` en GitHub:
   - Ve a: https://github.com/JCASASB/MemoryOnlineBE/settings/secrets/actions
   - Click **New repository secret** o actualiza el existente
   - Name: `CLOUDFLARE_TOKEN`
   - Value: `eyJh...` (pega el token completo)

---

## ✅ Paso 4: Verificar Configuración

Después del deploy, tus servicios estarán disponibles en:

### 🎮 SignalR Hub
```
https://signalr.hispalance.com/gamehub
```

**Conexión desde cliente:**
```csharp
var connection = new HubConnectionBuilder()
    .WithUrl("https://signalr.hispalance.com/gamehub")
    .Build();
```

### 🔌 WebApi

**Endpoints principales:**
```
https://api.hispalance.com/api/...        (API REST)
https://api.hispalance.com/scalar/v1      (Documentación interactiva)
https://api.hispalance.com/openapi/v1.json (OpenAPI spec)
```

**Ejemplo de llamada:**
```bash
curl https://api.hispalance.com/api/users
```

---

## 🔒 CORS Configuration

Si accedes desde diferentes dominios, asegúrate de configurar CORS en tus APIs:

### SignalR (`Program.cs`):
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("https://signalr.hispalance.com")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

app.UseCors("AllowAll");
```

### WebApi (`Program.cs`):
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("https://api.hispalance.com")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

app.UseCors("AllowAll");
```

---

## 🐛 Troubleshooting

### Problema: SignalR no conecta

**Solución:** Verifica que WebSockets esté habilitado en Cloudflare:
1. Cloudflare Dashboard → tu dominio → **Network**
2. Asegúrate que **WebSockets** esté **ON**

### Problema: 502 Bad Gateway

**Solución:** El contenedor no está corriendo
```bash
# Conéctate a tu EC2
ssh ec2-user@54.172.220.11

# Verifica contenedores
sudo docker ps

# Ver logs
sudo docker logs memoryonline-signalr
sudo docker logs memoryonline-webapi
```

### Problema: Subdominios no resuelven

**Solución:** Verifica DNS en Cloudflare:
1. Cloudflare Dashboard → **DNS**
2. Deberías ver registros CNAME automáticos:
   - `signalr.hispalance.com` → `[tunnel-id].cfargotunnel.com`
   - `api.hispalance.com` → `[tunnel-id].cfargotunnel.com`

---

## 📚 Recursos

- [Cloudflare Tunnel Documentation](https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/)
- [Tunnel Configuration](https://developers.cloudflare.com/cloudflare-one/connections/connect-apps/install-and-setup/tunnel-guide/)
- [SignalR with Azure SignalR Service](https://learn.microsoft.com/en-us/aspnet/core/signalr/scale)

---

## 🆘 Soporte

Si tienes problemas:
1. Revisa los logs del workflow en GitHub Actions
2. Verifica logs del contenedor en EC2
3. Revisa la configuración de Public Hostnames en Cloudflare
4. Asegúrate que los puertos 5000 y 5001 no estén bloqueados
