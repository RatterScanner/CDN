#!/bin/sh
set -eu

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$ROOT_DIR"

echo "[deploy] Pulling latest changes..."
if ! git pull --ff-only; then
	echo "[deploy] git pull failed. Aborting." >&2
	exit 1
fi

echo "[deploy] Building and starting containers..."
if docker compose up --build -d; then
	echo "[deploy] Containers started (detached)."
	exit 0
else
	echo "[deploy] docker compose failed." >&2
	exit 2
fi
