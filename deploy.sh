#!/bin/sh
set -eu

cp /ratterscanner/cdn.env .env

echo "[docker] Building and starting containers..."
if docker compose up --build -d; then
	echo "[docker] Containers successfully started in the background."
	exit 0
else
	echo "[docker] docker compose failed." >&2
	exit 2
fi
