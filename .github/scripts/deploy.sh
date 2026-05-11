#!/bin/bash
set -e

# ===========================================
# Script de Deploy para EC2 con Cloudflare Tunnel
# Despliega: SignalR + WebApi con subdominios
# ===========================================

# Variables (pasadas como argumentos o variables de entorno)
CLOUDFLARE_TOKEN="${CLOUDFLARE_TOKEN}"
EC2_USER="${EC2_USER}"
BASE_DOMAIN="${DOMAIN:-hispalance.com}"

# Configuración de servicios
SIGNALR_CONTAINER="memoryonline-signalr"
SIGNALR_IMAGE="memoryonline-signalr"
SIGNALR_PORT=5000
SIGNALR_SUBDOMAIN="signalr.${BASE_DOMAIN}"

WEBAPI_CONTAINER="memoryonline-webapi"
WEBAPI_IMAGE="memoryonline-webapi"
WEBAPI_PORT=5001
WEBAPI_SUBDOMAIN="api.${BASE_DOMAIN}"

# Configuración de Monitoring
OTEL_CONTAINER="otel-collector"
OTEL_IMAGE="memoryonline-otel"
OTEL_PORT_1=4317
OTEL_PORT_2=4318

TEMPO_CONTAINER="tempo"
TEMPO_IMAGE="grafana/tempo:latest"
TEMPO_PORT=3200

LOKI_CONTAINER="loki"
LOKI_IMAGE="grafana/loki:3.0.0"
LOKI_PORT=3100

GRAFANA_CONTAINER="grafana"
GRAFANA_IMAGE="grafana/grafana:latest"
GRAFANA_PORT=3000

echo "🚀 Iniciando deploy"
echo "   SignalR: https://$SIGNALR_SUBDOMAIN"
echo "   WebApi:  https://$WEBAPI_SUBDOMAIN"

# ===========================================
# 1. Detectar tipo de SO
# ===========================================
if [ -f /etc/os-release ]; then
    . /etc/os-release
    OS=$ID
else
    OS=$(uname -s | tr '[:upper:]' '[:lower:]')
fi
echo "🖥️  SO Detectado: $OS"

# ===========================================
# 2. Instalar Docker si no existe
# ===========================================
if ! command -v docker &> /dev/null; then
    echo "🐳 Instalando Docker..."
    if [ "$OS" = "ubuntu" ] || [ "$OS" = "debian" ]; then
        sudo apt-get update -y
        sudo apt-get install -y docker.io
    elif [ "$OS" = "amzn" ] || [ "$OS" = "amazonlinux" ]; then
        sudo yum update -y
        sudo yum install -y docker
    fi
    sudo systemctl start docker
    sudo systemctl enable docker
    sudo usermod -aG docker $USER
fi

# ===========================================
# 3. Instalar y configurar Cloudflare Tunnel
# ===========================================
echo "☁️  Configurando Cloudflare Tunnel..."

# Verificar si cloudflared ya está instalado
if ! command -v cloudflared &> /dev/null; then
    echo "📥 Instalando cloudflared..."

    if [ "$OS" = "ubuntu" ] || [ "$OS" = "debian" ]; then
        # Para Ubuntu/Debian
        curl -L https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-linux-amd64.deb -o cloudflared.deb
        sudo dpkg -i cloudflared.deb
        rm cloudflared.deb
    elif [ "$OS" = "amzn" ] || [ "$OS" = "amazonlinux" ]; then
        # Para Amazon Linux
        curl -L https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-linux-amd64.rpm -o cloudflared.rpm
        sudo yum install -y cloudflared.rpm
        rm cloudflared.rpm
    fi

    echo "✅ cloudflared instalado correctamente"
else
    echo "✅ cloudflared ya está instalado"
fi

# Configurar el túnel si no está configurado y si se proporciona el token
if [ -n "$CLOUDFLARE_TOKEN" ]; then
    echo "🔧 Configurando túnel de Cloudflare..."

    # Detener servicio existente si está corriendo
    sudo systemctl stop cloudflared 2>/dev/null || true

    # Instalar/reinstalar el servicio con el token
    sudo cloudflared service install --token "$CLOUDFLARE_TOKEN"

    # Iniciar y habilitar el servicio
    sudo systemctl start cloudflared
    sudo systemctl enable cloudflared

    echo "✅ Túnel de Cloudflare configurado y iniciado"
else
    echo "⚠️  CLOUDFLARE_TOKEN no proporcionado, verificando túnel existente..."

    # Verificar si el servicio ya está corriendo
    if sudo systemctl is-active --quiet cloudflared; then
        echo "✅ Túnel de Cloudflare ya está activo"
    else
        echo "❌ Túnel de Cloudflare no está configurado y no se proporcionó token"
        echo "   Por favor configura la variable CLOUDFLARE_TOKEN"
    fi
fi

# ===========================================
# 4. Función de deploy
# ===========================================
deploy_container() {
    local CONTAINER_NAME=$1
    local IMAGE_NAME=$2
    local PORT=$3
    local IMAGE_FILE=$4
    local ENV_VARS=$5
    local EXTRA_ARGS=$6

    echo ""
    echo "🐳 Desplegando: $CONTAINER_NAME"
    
    if [ -f "/home/$EC2_USER/$IMAGE_FILE" ]; then
        echo "   📦 Cargando imagen..."
        sudo docker load -i /home/$EC2_USER/$IMAGE_FILE
    fi
        
    echo "   🛑 Deteniendo contenedor anterior..."
    sudo docker stop $CONTAINER_NAME 2>/dev/null || true
    sudo docker rm $CONTAINER_NAME 2>/dev/null || true

    echo "   🚀 Iniciando contenedor..."
    sudo docker run -d \
        --name $CONTAINER_NAME \
        --network host \
        --restart unless-stopped \
        $ENV_VARS \
        $EXTRA_ARGS \
        $IMAGE_NAME:latest

    if [ -f "/home/$EC2_USER/$IMAGE_FILE" ]; then
        rm -f /home/$EC2_USER/$IMAGE_FILE
    fi
    echo "   ✅ $CONTAINER_NAME desplegado"
}

# Crear network si no existe
sudo docker network create app-network 2>/dev/null || true

# ===========================================
# 5. Deploy del stack de monitoreo
# ===========================================
echo "📊 Desplegando Stack de Monitoreo..."

# Tempo
sudo docker stop $TEMPO_CONTAINER 2>/dev/null || true
sudo docker rm $TEMPO_CONTAINER 2>/dev/null || true
sudo docker run -d --name $TEMPO_CONTAINER --network app-network --restart unless-stopped -p $TEMPO_PORT:3200 -p 4317 -v /home/$EC2_USER/tempo.yml:/etc/tempo.yml $TEMPO_IMAGE -config.file=/etc/tempo.yml

# Loki
sudo docker stop $LOKI_CONTAINER 2>/dev/null || true
sudo docker rm $LOKI_CONTAINER 2>/dev/null || true
sudo docker run -d --name $LOKI_CONTAINER --network app-network --restart unless-stopped -p $LOKI_PORT:3100 $LOKI_IMAGE -config.file=/etc/loki/local-config.yaml

# Grafana
sudo docker stop $GRAFANA_CONTAINER 2>/dev/null || true
sudo docker rm $GRAFANA_CONTAINER 2>/dev/null || true
sudo bash -c "sudo chmod -R 777 /home/$EC2_USER/grafana" # Permisos para volumen de grafana
sudo docker run -d --name $GRAFANA_CONTAINER --network app-network --restart unless-stopped -p $GRAFANA_PORT:3000 -v /home/$EC2_USER/grafana/provisioning:/etc/grafana/provisioning -e GF_SECURITY_ADMIN_USER=admin -e GF_SECURITY_ADMIN_PASSWORD=admin $GRAFANA_IMAGE

# Otel Collector (de la imagen que construimos)
deploy_container "$OTEL_CONTAINER" "$OTEL_IMAGE" "$OTEL_PORT_1" "otel-image.tar.gz" "" "-p 4317:4317 -p 4318:4318 --network app-network"

# ===========================================
# 6. Deploy de ambos servicios (SignalR y WebApi)
# ===========================================
ENV_API="-e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080 -e OTEL_EXPORTER_OTLP_ENDPOINT=http://otel-collector:4317"

deploy_container "$SIGNALR_CONTAINER" "$SIGNALR_IMAGE" "$SIGNALR_PORT" "signalr-image.tar.gz" "$ENV_API" "-p $SIGNALR_PORT:8080 --network app-network"
deploy_container "$WEBAPI_CONTAINER" "$WEBAPI_IMAGE" "$WEBAPI_PORT" "webapi-image.tar.gz" "$ENV_API" "-p $WEBAPI_PORT:8080 --network app-network"

# ===========================================
# 7. Limpieza
# ===========================================
echo ""
echo "🧹 Limpiando imágenes no utilizadas..."
sudo docker image prune -f

# ===========================================
# 8. Diagnóstico
# ===========================================
echo ""
echo "========== 📊 DIAGNÓSTICO FINAL =========="

echo ""
echo "🖥️  SO: $OS"

echo ""
echo "☁️  Cloudflare Tunnel:"
if sudo systemctl is-active --quiet cloudflared; then
    echo "   Status: ACTIVO ✅"
else
    echo "   Status: INACTIVO ❌"
fi

echo ""
echo "🐳 Contenedores:"
echo "   ┌─────────────────────┬──────────┬────────┐"
echo "   │ Nombre              │ Estado   │ Puerto │"
echo "   ├─────────────────────┼──────────┼────────┤"

for CONTAINER in "$SIGNALR_CONTAINER:$SIGNALR_PORT" "$WEBAPI_CONTAINER:$WEBAPI_PORT"; do
    IFS=':' read -r NAME PORT <<< "$CONTAINER"
    if sudo docker ps | grep -q $NAME; then
        printf "   │ %-19s │ ✅ UP    │ %6s │\n" "$NAME" "$PORT"
    else
        printf "   │ %-19s │ ❌ DOWN  │ %6s │\n" "$NAME" "$PORT"
    fi
done
echo "   └─────────────────────┴──────────┴────────┘"

echo ""
echo "🔗 Conectividad local:"
if curl -s --max-time 2 http://localhost:$SIGNALR_PORT/gamehub > /dev/null 2>&1; then
    echo "   SignalR (localhost:$SIGNALR_PORT): ✅"
else
    echo "   SignalR (localhost:$SIGNALR_PORT): ⚠️"
fi

if curl -s --max-time 2 http://localhost:$WEBAPI_PORT > /dev/null 2>&1; then
    echo "   WebApi  (localhost:$WEBAPI_PORT): ✅"
else
    echo "   WebApi  (localhost:$WEBAPI_PORT): ⚠️"
fi

echo ""
echo "========== 🎉 DEPLOY COMPLETADO =========="
echo ""
echo "📍 Endpoints públicos (requieren configuración en Cloudflare):"
echo ""
echo "   🎮 SignalR Hub:"
echo "      https://$SIGNALR_SUBDOMAIN/gamehub"
echo "      └─> localhost:$SIGNALR_PORT"
echo ""
echo "   🔌 WebApi:"
echo "      https://$WEBAPI_SUBDOMAIN/api"
echo "      https://$WEBAPI_SUBDOMAIN/scalar/v1  (Documentación)"
echo "      https://$WEBAPI_SUBDOMAIN/openapi/v1.json"
echo "      └─> localhost:$WEBAPI_PORT"
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "⚙️  CONFIGURACIÓN REQUERIDA EN CLOUDFLARE TUNNEL:"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""
echo "1. Ve a: https://one.dash.cloudflare.com/"
echo "2. Zero Trust → Networks → Tunnels → [Tu tunnel]"
echo "3. Pestaña 'Public Hostname' → Agrega estas 2 rutas:"
echo ""
echo "   📌 Ruta 1 - SignalR:"
echo "      Subdomain: signalr"
echo "      Domain: $BASE_DOMAIN"
echo "      Service: http://localhost:$SIGNALR_PORT"
echo ""
echo "   📌 Ruta 2 - WebApi:"
echo "      Subdomain: api"
echo "      Domain: $BASE_DOMAIN"
echo "      Service: http://localhost:$WEBAPI_PORT"
echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""
