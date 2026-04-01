#!/bin/bash
set -e

# ===========================================
# Script de Deploy para EC2 con Cloudflare Tunnel
# ===========================================

# Variables (pasadas como argumentos o variables de entorno)
CLOUDFLARE_TOKEN="${CLOUDFLARE_TOKEN}"
EC2_USER="${EC2_USER}"
CONTAINER_NAME="${CONTAINER_NAME}"
IMAGE_NAME="${IMAGE_NAME}"
DOMAIN="${DOMAIN:-memoryec2.hispalance.com}"

echo "🚀 Iniciando deploy para dominio: $DOMAIN"

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
# 4. Deploy del contenedor Docker
# ===========================================
echo "🐳 Desplegando contenedor Docker..."
sudo docker load -i /home/$EC2_USER/image.tar.gz
sudo docker stop $CONTAINER_NAME 2>/dev/null || true
sudo docker rm $CONTAINER_NAME 2>/dev/null || true

sudo docker run -d \
    --name $CONTAINER_NAME \
    --restart unless-stopped \
    -p 5000:8080 \
    -e ASPNETCORE_ENVIRONMENT=Production \
    -e ASPNETCORE_URLS="http://+:8080" \
    $IMAGE_NAME:latest

# ===========================================
# 5. Limpieza
# ===========================================
echo "🧹 Limpiando..."
rm -f /home/$EC2_USER/image.tar.gz
sudo docker image prune -f

# ===========================================
# 6. Diagnóstico
# ===========================================
echo ""
echo "========== 📊 DIAGNÓSTICO FINAL =========="

echo ""
echo "✅ SO Detectado:"
if [ -f /etc/os-release ]; then 
    . /etc/os-release
    echo "   OS: $ID $VERSION"
fi

echo ""
echo "✅ Estado de Cloudflare Tunnel:"
if sudo systemctl is-active --quiet cloudflared; then
    echo "   Status: ACTIVO ✅"
    sudo cloudflared service status 2>/dev/null || echo "   Servicio cloudflared está corriendo"
else
    echo "   Status: INACTIVO ❌"
fi

echo ""
echo "✅ Estado del contenedor:"
if sudo docker ps | grep $CONTAINER_NAME > /dev/null; then
    echo "   Status: EJECUTÁNDOSE ✅"
    sudo docker ps | grep $CONTAINER_NAME
else
    echo "   Status: NO EJECUTÁNDOSE ❌"
fi

echo ""
echo "✅ Configuración de Túnel:"
echo "   Dominio configurado: $DOMAIN"
echo "   Endpoint: https://$DOMAIN/gamehub"

echo ""
echo "✅ Conectividad local:"
if curl -s http://localhost:5000/gamehub > /dev/null 2>&1; then
    echo "   localhost:5000: RESPONDIENDO ✅"
else
    echo "   localhost:5000: SIN RESPUESTA ⚠️"
fi

echo ""
echo "========== 🎉 DEPLOY COMPLETADO =========="
echo ""
echo "📍 Acceso externo:"
echo "   https://$DOMAIN/gamehub (vía Cloudflare Tunnel)"
echo ""
