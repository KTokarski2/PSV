#!/bin/bash

OUTPUT_DIR="/home/fedora/Projects/PSV_DEPLOY"

dotnet publish -c Release -r win-x64 --self-contained -o ./Deploy

cp db_create.sql "$OUTPUT_DIR"

cp -r ./Deploy "$OUTPUT_DIR"