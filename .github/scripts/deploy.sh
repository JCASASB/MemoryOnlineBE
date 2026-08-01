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
MONGO_CONNECTION_STRING="${MONGO_CONNECTION_STRING:-}"
MONGO_SSM_PARAMETER_NAME="${MONGO_SSM_PARAMETER_NAME:-/backend/MongoConnectionString}"

# Si no se proporciona MongoDB por variable de entorno, se intenta leer desde AWS Systems Manager Parameter Store
if [ -z "$MONGO_CONNECTION_STRING" ]; then
    echo "☁️  MONGO_CONNECTION_STRING no proporcionada. Se intentará leer desde AWS SSM: $MONGO_SSM_PARAMETER_NAME"
fi

# Configuración de servicios
SIGNALR_CONTAINER="memoryonline-signalr"
SIGNALR_IMAGE="memoryonline-signalr"
SIGNALR_PORT=5000
SIGNALR_SUBDOMAIN="signalr.${BASE_DOMAIN}"

WEBAPI_CONTAINER="memoryonline-webapi"
WEBAPI_IMAGE="memoryonline-webapi"
WEBAPI_PORT=5001
WEBAPI_SUBDOMAIN="api.${BASE_DOMAIN}"

# Variable de entorno para MongoDB (proviene del GitHub Secret)
MONGO_CONNECTION_STRING="${MONGO_CONNECTION_STRING}"

# Configuración de Monitoring (deshabilitado)
# OTEL_CONTAINER="otel-collector"
# OTEL_IMAGE="memoryonline-otel"
# OTEL_PORT_1=4317
# OTEL_PORT_2=4318

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
    case "$OS" in
        ubuntu|debian)
            sudo apt-get update -y
            sudo apt-get install -y docker.io
            ;;
        amzn|amazonlinux|fedora|rhel|centos)
            sudo yum update -y
            sudo yum install -y docker
            ;;
    esac
    sudo systemctl start docker
    sudo systemctl enable docker
    sudo usermod -aG docker $USER
fi

# ===========================================
# 3. Instalar AWS CLI si no existe (necesario para leer SSM Parameter Store)
# ===========================================
if ! command -v aws &> /dev/null; then
    echo "☁️  Instalando AWS CLI v2..."
    case "$OS" in
        ubuntu|debian)
            sudo apt-get update -y
            sudo apt-get install -y curl unzip
            ;;
        amzn|amazonlinux|fedora|rhel|centos)
            sudo yum update -y
            sudo yum install -y curl unzip
            ;;
    esac
    curl -fsSL "https://awscli.amazonaws.com/awscli-exe-linux-$(uname -m).zip" -o awscliv2.zip
    unzip -q awscliv2.zip
    sudo ./aws/install --update
    rm -rf aws awscliv2.zip
    echo "✅ AWS CLI instalado"
fi

# Si no se proporcionó MongoDB por variable de entorno, leer desde AWS SSM Parameter Store
if [ -z "$MONGO_CONNECTION_STRING" ]; then
    echo "☁️  Leyendo MONGO_CONNECTION_STRING desde AWS SSM Parameter Store..."
    MONGO_CONNECTION_STRING=$(aws ssm get-parameter \
        --name "$MONGO_SSM_PARAMETER_NAME" \
        --with-decryption \
        --query 'Parameter.Value' \
        --output text 2>/dev/null) || true

    if [ -z "$MONGO_CONNECTION_STRING" ]; then
        echo "❌ No se pudo leer MONGO_CONNECTION_STRING desde SSM ($MONGO_SSM_PARAMETER_NAME)."
        echo "   Verifica que la EC2 tenga rol de IAM con permiso ssm:GetParameter y que el parámetro exista."
        exit 1
    fi

    echo "✅ MONGO_CONNECTION_STRING leída correctamente desde SSM (${#MONGO_CONNECTION_STRING} caracteres)"
fi

# ===========================================
# 4. Instalar y configurar Cloudflare Tunnel
# ===========================================
echo "☁️  Configurando Cloudflare Tunnel..."

# Detectar arquitectura de la instancia (t4g.nano es ARM64)
ARCH=$(uname -m)
if [ "$ARCH" = "aarch64" ] || [ "$ARCH" = "arm64" ]; then
    CLOUDFLARED_ARCH="arm64"
else
    CLOUDFLARED_ARCH="amd64"
fi
echo "🔧 Arquitectura detectada: $ARCH → cloudflared-linux-$CLOUDFLARED_ARCH"

# Verificar si cloudflared ya está instalado
if ! command -v cloudflared &> /dev/null; then
    echo "📥 Instalando cloudflared..."

    if [ "$OS" = "ubuntu" ] || [ "$OS" = "debian" ]; then
        # Para Ubuntu/Debian
        curl -L "https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-linux-${CLOUDFLARED_ARCH}.deb" -o cloudflared.deb
        sudo dpkg -i cloudflared.deb
        rm cloudflared.deb
    elif [ "$OS" = "amzn" ] || [ "$OS" = "amazonlinux" ]; then
        # Para Amazon Linux
        curl -L "https://github.com/cloudflare/cloudflared/releases/latest/download/cloudflared-linux-${CLOUDFLARED_ARCH}.rpm" -o cloudflared.rpm
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

    # Registrar longitud del token para depuración (sin mostrarlo)
    echo "🔐 Longitud del token recibido: ${#CLOUDFLARE_TOKEN}"

    # Validar que el token tiene longitud mínima razonable
    if [ "${#CLOUDFLARE_TOKEN}" -lt 10 ]; then
        echo "❌ CLOUDFLARE_TOKEN parece estar vacío o incompleto."
        exit 1
    fi

    # Detener servicio existente si está corriendo
    sudo systemctl stop cloudflared 2>/dev/null || true

    # Desinstalar servicio existente si existe para poder instalarlo nuevamente con el token
    if [ -f "/etc/systemd/system/cloudflared.service" ]; then
        sudo cloudflared service uninstall
        # Recargar los demonios de systemd para limpiar el estado anterior
        sudo systemctl daemon-reload
    fi

    # Crear directorio de token antes de instalar (por seguridad/defensiva)
    sudo mkdir -p /etc/cloudflared
    printf '%s' "$CLOUDFLARE_TOKEN" | sudo tee /etc/cloudflared/token > /dev/null
    sudo chmod 600 /etc/cloudflared/token

    # Instalar/reinstalar el servicio con el token, forzando la creación de errores amigables si falla
    if ! sudo cloudflared service install "$CLOUDFLARE_TOKEN"; then
        echo "⚠️  Cloudflared install reportó un problema o requirió recarga manual."
    fi
    sudo systemctl daemon-reload

    # Parchear el servicio para forzar IPv4 y HTTP/2 (evita fallos con IPv6/QUIC en EC2)
    CLOUDFLARED_SERVICE="/etc/systemd/system/cloudflared.service"
    if [ -f "$CLOUDFLARED_SERVICE" ]; then
        sudo sed -i 's|--no-autoupdate tunnel run|--no-autoupdate --edge-ip-version 4 --protocol http2 tunnel run|g' "$CLOUDFLARED_SERVICE"
        # Añadir timeout y pausa entre reintentos solo si no existen ya
        if ! sudo grep -q "TimeoutStartSec" "$CLOUDFLARED_SERVICE"; then
            sudo sed -i '/\[Service\]/a TimeoutStartSec=90' "$CLOUDFLARED_SERVICE"
        fi
        if ! sudo grep -q "RestartSec" "$CLOUDFLARED_SERVICE"; then
            sudo sed -i '/\[Service\]/a RestartSec=10' "$CLOUDFLARED_SERVICE"
        fi
        sudo systemctl daemon-reload
        echo "✅ Servicio cloudflared parcheado con IPv4 + HTTP/2"
    fi

    # Iniciar y habilitar el servicio, manejando el posible fallo de start explícitamente
    sudo systemctl enable cloudflared.service || true
    if ! sudo systemctl start cloudflared.service; then
        echo "❌ Fallo al iniciar el servicio cloudflared. Error de systemd."
        echo "🔍 Últimos logs de journalctl para cloudflared:"
        sudo journalctl -xeu cloudflared.service --no-pager | tail -n 50

        echo "🔍 Estado actual del servicio cloudflared:"
        sudo systemctl status cloudflared.service --no-pager || true

        exit 1
    fi

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
# 5. Deploy del stack de monitoreo (deshabilitado)
# ===========================================
# echo "📊 Desplegando Stack de Monitoreo..."
# deploy_container "$OTEL_CONTAINER" "$OTEL_IMAGE" "$OTEL_PORT_1" "otel-image.tar.gz" "" "-p 4317:4317 -p 4318:4318 --network app-network"

# ===========================================
# 6. Deploy de ambos servicios (SignalR y WebApi)
# ===========================================
MONGO_CONNECTION_STRING="${MONGO_CONNECTION_STRING:-}"

if [ -z "$MONGO_CONNECTION_STRING" ]; then
    echo "⚠️  MONGO_CONNECTION_STRING no está definida. Se usará el valor de appsettings.json."
fi

# Limpiar posibles comillas dobles que se hayan colado al pasar la variable desde GitHub Actions
MONGO_CONNECTION_STRING=$(printf '%s' "$MONGO_CONNECTION_STRING" | tr -d '"')

# Comprobar que el valor parseado tenga contenido sensato
if [ -n "$MONGO_CONNECTION_STRING" ] && ! printf '%s' "$MONGO_CONNECTION_STRING" | grep -qE '^mongodb(\+srv)?://'; then
    echo "❌ MONGO_CONNECTION_STRING no parece una URL de MongoDB válida. Valor actual: $MONGO_CONNECTION_STRING"
    exit 1
fi

ENV_API="-e ASPNETCORE_ENVIRONMENT=Production -e ASPNETCORE_URLS=http://+:8080"
MONGO_ENV_VAR="DBSection__MongoConnectionString=$MONGO_CONNECTION_STRING"

# Crear directorio de datos compartido si no existe
mkdir -p /home/$EC2_USER/data

VOLUME_MOUNT="-v /home/$EC2_USER/data:/app/data"

deploy_container "$SIGNALR_CONTAINER" "$SIGNALR_IMAGE" "$SIGNALR_PORT" "signalr-image.tar.gz" "$ENV_API" "-p $SIGNALR_PORT:8080 --network app-network $VOLUME_MOUNT -e $MONGO_ENV_VAR"
deploy_container "$WEBAPI_CONTAINER" "$WEBAPI_IMAGE" "$WEBAPI_PORT" "webapi-image.tar.gz" "$ENV_API" "-p $WEBAPI_PORT:8080 --network app-network $VOLUME_MOUNT -e $MONGO_ENV_VAR"

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
