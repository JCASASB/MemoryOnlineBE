#!/bin/bash
# Script para configurar Nginx + Certbot + Docker en EC2
# Uso: ./scripts/setup-nginx-ec2.sh <dominio> <email> <puerto-app>

set -e

DOMAIN=${1:-"tu-dominio.duckdns.org"}
EMAIL=${2:-"tu-email@ejemplo.com"}
APP_PORT=${3:-5000}

echo "🚀 Configurando Nginx + Certbot para $DOMAIN..."

# 1. Actualizar paquetes
echo "📦 Actualizando paquetes..."
sudo apt update
sudo apt install -y nginx certbot python3-certbot-nginx

# 2. Generar certificado SSL
echo "🔒 Generando certificado SSL con Let's Encrypt..."
sudo certbot certonly --nginx -d $DOMAIN \
  --email $EMAIL \
  --agree-tos \
  --non-interactive \
  --keep-until-expiring

# 3. Crear configuración de Nginx
echo "⚙️  Configurando Nginx como reverse proxy..."
sudo tee /etc/nginx/sites-available/default > /dev/null << NGINX
server {
    listen 80;
    server_name $DOMAIN;
    return 301 https://\$server_name\$request_uri;
}

server {
    listen 443 ssl http2;
    server_name $DOMAIN;

    ssl_certificate /etc/letsencrypt/live/$DOMAIN/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/$DOMAIN/privkey.pem;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers HIGH:!aNULL:!MD5;
    ssl_prefer_server_ciphers on;

    access_log /var/log/nginx/access.log;
    error_log /var/log/nginx/error.log;

    location / {
        proxy_pass http://localhost:$APP_PORT;
        proxy_http_version 1.1;
        proxy_set_header Upgrade \$http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host \$host;
        proxy_set_header X-Real-IP \$remote_addr;
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto \$scheme;
        proxy_cache_bypass \$http_upgrade;
    }
}
NGINX

# 4. Validar configuración
echo "✅ Validando configuración de Nginx..."
sudo nginx -t

# 5. Reiniciar servicios
echo "🔄 Reiniciando Nginx..."
sudo systemctl enable nginx
sudo systemctl restart nginx

# 6. Configurar auto-renewal
echo "📅 Configurando renovación automática de certificados..."
sudo systemctl enable certbot.timer
sudo systemctl start certbot.timer

# 7. Mostrar estado
echo "
✅ Configuración completada!

📊 Estado de los servicios:
"
sudo systemctl status nginx --no-pager || true
echo ""
sudo systemctl status certbot.timer --no-pager || true
echo ""
echo "🔐 Certificados instalados:"
sudo certbot certificates

echo "
🎉 Tu aplicación está disponible en: https://$DOMAIN
🔒 El certificado se renovará automáticamente"
