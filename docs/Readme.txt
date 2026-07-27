#!/bin/bash

# 1. Configurar el repositorio oficial de Cloudflare
curl -fsSl https://cloudflare.com | sudo tee /etc/yum.repos.d/cloudflared.repo

# 2. Actualizar repositorios e instalar cloudflared
sudo yum update -y
sudo yum install -y cloudflared

# 3. Instalar el túnel como servicio del sistema
# REEMPLAZA "TU_TOKEN_AQUI" con el código largo de tu panel de Cloudflare
sudo cloudflared service install --token TU_TOKEN_AQUI

# 4. Iniciar el servicio y habilitarlo para que arranque tras cada reinicio
sudo systemctl start cloudflared
sudo systemctl enable cloudflared

# 5. Verificar el estado
echo "Verificando estado del túnel..."
sudo systemctl status cloudflared



-----------------------------------------------------------------

Instrucciones para ejecutarlo:

Crea el archivo en tu EC2: nano configurar-tunel.sh
Pega el código anterior (poniendo tu token).
Guarda con Ctrl+O, Enter y sal con Ctrl+X.
Dale permisos: chmod +x configurar-tunel.sh
Ejecútalo: ./configurar-tunel.sh

¿Qué hace este script exactamente?

Repo oficial: Usa la vía recomendada para Amazon Linux, así se actualizará solo cuando haya mejoras.
Servicio Systemd: Al ejecutar service install, Cloudflare crea un archivo de configuración en /etc/systemd/system/ que hace que el túnel se conecte automáticamente en cuanto la EC2 tenga internet tras un reinicio.
IP Dinámica: Como el túnel usa una conexión de salida, ya no importa si tu IP cambia; el servicio se reconectará solo.