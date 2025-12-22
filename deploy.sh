#!/bin/sh
set -eu

# echo "[git] Pulling latest changes..."
# if ! git pull --ff-only; then
# 	echo "[git] git pull failed. Aborting." >&2
# 	exit 1
# fi

echo "[docker] Building and starting containers..."
if docker compose up --build -d; then
	echo "[docker] Containers successfully started in the background."
	exit 0
else
	echo "[docker] docker compose failed." >&2
	exit 2
fi
