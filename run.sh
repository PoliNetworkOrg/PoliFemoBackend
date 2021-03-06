#!/bin/sh
sleep 2
sudo pkill PoliFemoBackend
sudo iptables -P INPUT ACCEPT
sudo iptables -P FORWARD ACCEPT
sudo iptables -P OUTPUT ACCEPT
sudo iptables -t nat -F
sudo iptables -t mangle -F
sudo iptables -F
sudo iptables -X
wget https://github.com/PoliNetworkOrg/PoliFemoBackend/releases/latest/download/PoliFemoBackend.zip -q -O /home/ubuntu/PoliFemoBackend/PoliFemoBackend.zip
unzip -o PoliFemoBackend.zip
sudo chmod +x run.sh
cp appsettings_production.json appsettings.json
SSLCert=$(cat secrets.json | jq -r '.SSLCert')
cat appsettings.json | jq --arg SSLCert "$SSLCert" '.Kestrel.Endpoints.HttpsInlineCertAndKeyFile.Certificate.Password |= $SSLCert' > appsettings.json.tmp
cp appsettings.json.tmp appsettings.json
sudo ./PoliFemoBackend