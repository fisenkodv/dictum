#!/usr/bin/env bash

cd $(pwd)/$(dirname "$0")/../

printf "Building docker image...\n\n"
docker build --rm --tag fisenkodv/dictum:latest --file docker/api/Dockerfile .
#docker push fisenkodv/dictum:latest