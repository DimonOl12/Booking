#!/usr/bin/env bash
# Запускає PostgreSQL (Docker) і бекенд Reservio API
# Використання: ./start-backend.sh

set -e

echo "[1/3] Перевірка PostgreSQL контейнера..."
if docker ps --format '{{.Names}}' | grep -q '^reservio-pg$'; then
  echo "      PostgreSQL вже запущений."
else
  if docker ps -a --format '{{.Names}}' | grep -q '^reservio-pg$'; then
    echo "      Запуск існуючого контейнера reservio-pg..."
    docker start reservio-pg
  else
    echo "      Створення нового контейнера reservio-pg..."
    docker run -d --name reservio-pg \
      -e POSTGRES_PASSWORD=qwerty \
      -e POSTGRES_DB=postgres \
      -p 5433:5432 \
      postgres:16-alpine
  fi
  echo "      Очікування готовності бази..."
  sleep 5
fi

echo "[2/3] Збірка бекенду..."
dotnet build /workspaces/Booking/src/Reservio/Reservio.WebApi/Reservio.WebApi.csproj \
  --configuration Release \
  -o /tmp/reservio-api \
  -nologo -q 2>&1 | grep -E "error|warning|succeeded|failed" | head -5

echo "[3/3] Запуск бекенду на http://localhost:5292 ..."
pkill -f "Reservio.WebApi.dll" 2>/dev/null || true
sleep 1

cd /tmp/reservio-api
ASPNETCORE_ENVIRONMENT=Development \
ASPNETCORE_URLS="http://localhost:5292" \
nohup dotnet Reservio.WebApi.dll > /tmp/reservio-api.log 2>&1 &

echo "      PID: $!"
echo "      Очікування запуску..."
sleep 8

if curl -s http://localhost:5292/swagger/index.html | grep -q "swagger"; then
  echo ""
  echo "Бекенд запущений! http://localhost:5292/swagger"
else
  echo ""
  echo "Помилка запуску. Логи:"
  tail -20 /tmp/reservio-api.log
fi
