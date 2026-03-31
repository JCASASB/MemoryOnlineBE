#!/bin/bash
set -e

# ===========================================
# Script de Deploy para EC2
# ===========================================

# Variables (pasadas como argumentos o variables de entorno)
DUCKDNS_DOMAIN="${DUCKDNS_DOMAIN}"
DUCKDNS_TOKEN="${DUCKDNS_TOKEN}"
EC2_USER="${EC2_USER}"
CERTBOT_EMAIL="${CERTBOT_EMAIL}"
CONTAINER_NAME="${CONTAINER_NAME}"
IMAGE_NAME="${IMAGE_NAME}"
DOMAIN="${DUCKDNS_DOMAIN}.duckdns.org"

echo "🚀 Iniciando deploy para dominio: $DOMAIN"

# ===========================================
# 1. Configuración de DuckDNS
# ===========================================
echo "📡 Configurando DuckDNS..."
mkdir -p ~/duckdns
echo "echo url=\"https://www.duckdns.org/$DUCKDNS_DOMAIN/update?token=$DUCKDNS_TOKEN&ip=\" | curl -k -o /home/$EC2_USER/duckdns/duck.log -K -" > ~/duckdns/duck.sh
chmod 700 ~/duckdns/duck.sh
~/duckdns/duck.sh

# ===========================================
# 2. Detectar tipo de SO
# ===========================================
if [ -f /etc/os-release ]; then
    . /etc/os-release
    OS=$ID
else
    OS=$(uname -s | tr '[:upper:]' '[:lower:]')
fi
echo "🖥️  SO Detectado: $OS"

# ===========================================
# 3. Instalar Docker si no existe
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
# 4. Instalar Nginx y Certbot
# ===========================================
echo "🌐 Instalando Nginx y Certbot..."
if [ "$OS" = "ubuntu" ] || [ "$OS" = "debian" ]; then
    sudo apt-get install -y nginx certbot python3-certbot-nginx
elif [ "$OS" = "amzn" ] || [ "$OS" = "amazonlinux" ]; then
    sudo yum install -y nginx certbot python3-certbot-nginx
fi

# Detener Apache si está corriendo
echo "🛑 Parando Apache (httpd) si está corriendo..."
sudo systemctl stop httpd 2>/dev/null || true
sudo systemctl disable httpd 2>/dev/null || true

# ===========================================
# 5. Configurar Nginx temporal (HTTP)
# ===========================================
echo "⚙️  Configurando Nginx temporal (HTTP)..."
sudo mkdir -p /etc/nginx/conf.d

# Usar template HTTP y reemplazar dominio
sed "s/__DOMAIN__/$DOMAIN/g" /tmp/nginx-http.conf.template | sudo tee /etc/nginx/conf.d/reverse-proxy.conf > /dev/null

sudo systemctl enable nginx
sudo systemctl restart nginx

# ===========================================
# 6. Generar/Renovar certificado SSL
# ===========================================
if [ -z "$CERTBOT_EMAIL" ]; then
    echo "⚠️  CERTBOT_EMAIL no configurado, saltando SSL..."
else
    echo "🔐 Procesando certificado SSL..."

    # Detener Nginx para que Certbot use puerto 443
    echo "   - Deteniendo Nginx temporalmente..."
    sudo systemctl stop nginx 2>/dev/null || true

    # Esperar 2 segundos para que libere los puertos
    sleep 2

    # Si el certificado ya existe, renovarlo
    if [ -d "/etc/letsencrypt/live/$DOMAIN" ]; then
        echo "   - Renovando certificado SSL existente para $DOMAIN..."
        sudo certbot certonly --standalone -d "$DOMAIN" \
            --email "$CERTBOT_EMAIL" \
            --agree-tos \
            --non-interactive \
            --force-renewal || {
            echo "⚠️  Error: Renovación de certbot falló, usando certificado existente..."
        }
    else
        echo "   - Generando certificado SSL nuevo para $DOMAIN..."
        sudo certbot certonly --standalone -d "$DOMAIN" \
            --email "$CERTBOT_EMAIL" \
            --agree-tos \
            --non-interactive || {
            echo "⚠️  Error: Certbot falló, continuando con HTTP..."
        }
    fi

    # Verificar si el certificado existe
    if [ -d "/etc/letsencrypt/live/$DOMAIN" ]; then
        echo "   ✅ Certificado SSL validado/renovado"
    else
        echo "   ⚠️  Certificado NO disponible, usará HTTP"
    fi
fi

# ===========================================
# 7. Configurar Nginx con SSL (si existe cert)
# ===========================================
echo "⚙️  Configurando Nginx con SSL..."

if [ -d "/etc/letsencrypt/live/$DOMAIN" ]; then
    echo "   🔒 Certificado encontrado, aplicando configuración HTTPS..."
    sed "s/__DOMAIN__/$DOMAIN/g" /tmp/nginx-https.conf.template | sudo tee /etc/nginx/conf.d/reverse-proxy.conf > /dev/null
    echo "   ✅ Configuración HTTPS aplicada"
else
    echo "   ⚠️  Certificado no encontrado, usando configuración HTTP..."
    sed "s/__DOMAIN__/$DOMAIN/g" /tmp/nginx-http.conf.template | sudo tee /etc/nginx/conf.d/reverse-proxy.conf > /dev/null
    echo "   ℹ️  Usará HTTP en la próxima ejecución si se obtiene certificado"
fi

# Validar configuración
echo "   - Validando configuración Nginx..."
if sudo nginx -t 2>&1 | grep "successful"; then
    echo "   ✅ Configuración válida"
else
    echo "   ❌ Error en configuración Nginx"
    sudo nginx -t
    exit 1
fi

# Reiniciar Nginx
echo "   - Reiniciando Nginx..."
sudo systemctl restart nginx

# Verificar que inició correctamente
if sudo systemctl is-active --quiet nginx; then
    echo "   ✅ Nginx reiniciado correctamente"
else
    echo "   ❌ Error al reiniciar Nginx"
    sudo systemctl status nginx
    exit 1
fi

# ===========================================
# 8. Auto-renewal de certificados
# ===========================================
echo "🔄 Configurando auto-renewal..."
if [ "$OS" = "amzn" ] || [ "$OS" = "amazonlinux" ]; then
    sudo systemctl enable certbot-renew.timer 2>/dev/null || true
    sudo systemctl start certbot-renew.timer 2>/dev/null || true
else
    sudo systemctl enable certbot.timer 2>/dev/null || true
    sudo systemctl start certbot.timer 2>/dev/null || true
fi

# ===========================================
# 9. Deploy del contenedor Docker
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
# 10. Limpieza
# ===========================================
echo "🧹 Limpiando..."
rm -f /home/$EC2_USER/image.tar.gz
sudo docker image prune -f

# ===========================================
# 11. Diagnóstico
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
echo "✅ Estado de Nginx:"
if sudo systemctl is-active --quiet nginx; then
    echo "   Status: ACTIVO ✅"
    echo "   Escuchando en:"
    sudo netstat -tulpn 2>/dev/null | grep nginx || sudo ss -tulpn | grep nginx
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
echo "✅ Certificado SSL:"
if [ -d "/etc/letsencrypt/live/$DOMAIN" ]; then
    echo "   Status: VÁLIDO ✅"
    sudo certbot certificates 2>/dev/null | grep -A 5 "$DOMAIN"
else
    echo "   Status: NO INSTALADO ⚠️"
fi

echo ""
echo "✅ Configuración de Nginx:"
if [ -f /etc/nginx/conf.d/reverse-proxy.conf ]; then
    if grep -q "listen 443" /etc/nginx/conf.d/reverse-proxy.conf; then
        echo "   Modo: HTTPS ✅"
    elif grep -q "listen 80" /etc/nginx/conf.d/reverse-proxy.conf; then
        echo "   Modo: HTTP ⚠️"
    fi
    echo "   Dominio configurado: $DOMAIN"
else
    echo "   Archivo de configuración no encontrado ❌"
fi

echo ""
echo "✅ Conectividad local:"
if curl -s http://localhost:5000 > /dev/null 2>&1; then
    echo "   localhost:5000: RESPONDIENDO ✅"
else
    echo "   localhost:5000: SIN RESPUESTA ⚠️"
fi

echo ""
echo "========== 🎉 DEPLOY COMPLETADO =========="
echo ""
echo "📍 Acceso externo:"
echo "   http://hispalance.duckdns.org  (se redirige a HTTPS si está disponible)"
echo "   https://hispalance.duckdns.org (si SSL está configurado)"
echo ""
