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
# 6. Generar certificado SSL
# ===========================================
if [ -z "$CERTBOT_EMAIL" ]; then
    echo "⚠️  CERTBOT_EMAIL no configurado, saltando SSL..."
else
    # Detener Nginx para que Certbot use puerto 443
    sudo systemctl stop nginx 2>/dev/null || true
    
    if [ ! -d "/etc/letsencrypt/live/$DOMAIN" ]; then
        echo "🔐 Generando certificado SSL para $DOMAIN..."
        sudo certbot certonly --standalone -d "$DOMAIN" \
            --email "$CERTBOT_EMAIL" \
            --agree-tos \
            --non-interactive \
            --keep-until-expiring || echo "⚠️  Certbot falló, continuando con HTTP..."
    fi
fi

# ===========================================
# 7. Configurar Nginx con SSL (si existe cert)
# ===========================================
if [ -d "/etc/letsencrypt/live/$DOMAIN" ]; then
    echo "🔒 Certificado encontrado, configurando HTTPS..."
    sed "s/__DOMAIN__/$DOMAIN/g" /tmp/nginx-https.conf.template | sudo tee /etc/nginx/conf.d/reverse-proxy.conf > /dev/null
else
    echo "⚠️  Certificado no encontrado, usando solo HTTP..."
fi

# Validar y reiniciar Nginx
sudo nginx -t
sudo systemctl restart nginx

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
echo "========== 📊 DIAGNÓSTICO =========="
echo "=== SO Detectado ==="
if [ -f /etc/os-release ]; then . /etc/os-release; echo "OS: $ID $VERSION"; fi

echo ""
echo "=== Estado de Nginx ==="
sudo systemctl status nginx --no-pager 2>&1 | head -10 || echo "Nginx no disponible"

echo ""
echo "=== Estado del contenedor ==="
sudo docker ps -a | grep $CONTAINER_NAME || echo "Contenedor no encontrado"

echo ""
echo "=== Certificados ==="
sudo certbot certificates 2>&1 || echo "Certbot no disponible"

echo ""
echo "=== HTTPS Status ==="
if [ -d "/etc/letsencrypt/live/$DOMAIN" ]; then
    echo "✅ HTTPS configurado"
else
    echo "⚠️  Solo HTTP disponible"
fi

echo ""
echo "=== Conectividad ==="
curl -s http://localhost:5000 | head -5 || echo "No se pudo conectar"

echo ""
echo "=== Puertos ==="
sudo netstat -tulpn 2>/dev/null | grep LISTEN || ss -tulpn | grep LISTEN

echo ""
echo "🎉 Deploy completado!"
