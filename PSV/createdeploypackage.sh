#!/bin/bash

OUTPUT_DIR="/home/fedora/Projects/PSV_DEPLOY"
ZIP_FILE="$OUTPUT_DIR/DEPLOY_PACKAGE.zip"

rm -r ./wwwroot/images/OrdersData
mkdir ./wwwroot/images/OrdersData
rm -r ./Deploy
mkdir ./Deploy
dotnet ef migrations script -o DB_CREATE_SCRIPT.sql
dotnet publish -c Release -r win-x64 --self-contained -o ./Deploy

cp DB_CREATE_SCRIPT.sql "$OUTPUT_DIR"
cp -r ./Deploy "$OUTPUT_DIR"

cd "$OUTPUT_DIR"
cd ./Deploy
rm -r ./Deploy
cd ./wwwroot
mkdir ordersFiles
mkdir temp
cd "$OUTPUT_DIR" 
zip -r "$(basename "$ZIP_FILE")" ./*