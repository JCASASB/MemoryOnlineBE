para mi local el docker compose ------
Signalr http://localhost:5000
WebApi http://localhost:5001

Jaeger http://localhost:16686
grafana loki http://localhost:3100
grafana http://localhost:3000
-------------------------------



¿Qué se hizo?
•	Se agregó <PackageReference Include="Snappier" Version="1.3.1" /> a los archivos de proyecto afectados
•	Esto fuerza a NuGet a usar la versión segura 1.3.1 en lugar de la vulnerable 1.0.0 que venía como dependencia transitiva de MongoDB.EntityFrameworkCore



memoryec2.hispalance.com
signalr.hispalance.com
api.hispalance.com
grafana.hispalance.com



podman compose -f docker/docker-compose.yml -f docker/mongodb-compose.yml up -d
podman compose -f docker/mongodb-compose.yml up -d
podman compose -f docker/sqlserverdb-compose.yml up -d