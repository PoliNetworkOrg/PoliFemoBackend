#!/bin/sh
sleep 2
wget https://github.com/PoliNetworkOrg/PoliFemoBackend/releases/latest/download/PoliFemoBackend.zip -O /home/ubuntu/PoliFemoBackend/PoliFemoBackend.zip
unzip -o PoliFemoBackend.zip
cp appsettings_production.json appsettings.json
SSLCert=$(cat secrets.json | jq -r '.SSLCert')
cat appsettings.json | jq --arg SSLCert "$SSLCert" '.Kestrel.Endpoints.HttpsInlineCertAndKeyFile.Certificate.Password |= $SSLCert' > appsettings.json.tmp
cp appsettings.json.tmp appsettings.json
screen sudo ./PoliFemoBackend