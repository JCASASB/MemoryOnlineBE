Hola,

esta es la implementacion back end del juego memory online 

Tecnologia .net core, hub signalr, DDD, Mediator, REST, Entity Framework

## Sincronizar el repositorio público

Después de confirmar los cambios en `MemoryOnlineBE_azure/main`, ejecuta:

```powershell
.\scripts\sync-public-repo.ps1
```

El script reemplaza `MemoryOnlineBE/main` por una instantánea con un único
commit. No publica ningún `.env`, `appsettings*.json`, secreto ni perfil de
publicación, workflows de GitHub Actions ni informes `.appmod` de AppCAT. Las
contraseñas de los ejemplos Docker/Kubernetes se sustituyen por `CHANGE_ME`. El
historial completo permanece en `MemoryOnlineBE_azure`.

## ⚠️ Licencia y Derechos de Autor

Este proyecto es de **propiedad privada** exclusiva. El código está expuesto de forma
pública únicamente como parte de mi portafolio profesional. **No se otorga ninguna licencia**
para su uso, copia o modificación. Todos los derechos están reservados.

```

```
