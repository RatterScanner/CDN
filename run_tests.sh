#!/bin/sh
# On any error, exit with non-zero code
set -e

# Build the Docker image
docker build -t ratter-scanner-cdn-tests -f tests/Dockerfile .

# Run the tests using the Docker image we just built
docker run -v "./sample_data:/storage" --rm ratter-scanner-cdn-tests