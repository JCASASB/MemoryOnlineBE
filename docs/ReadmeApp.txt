Para el entorno local se utiliza Kubernetes con Kind y Podman.
Consulta k8s/local/README.md para arrancar y detener el clúster.

¿Qué se hizo?
•	Se agregó <PackageReference Include="Snappier" Version="1.3.1" /> a los archivos de proyecto afectados
•	Esto fuerza a NuGet a usar la versión segura 1.3.1 en lugar de la vulnerable 1.0.0 que venía como dependencia transitiva de MongoDB.EntityFrameworkCore

memoryec2.hispalance.com
signalr.hispalance.com
api.hispalance.com
grafana.hispalance.com
