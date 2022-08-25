#!/bin/sh
sleep 2
screen -S backend -p 0 -X stuff "^C"
sudo iptables -P INPUT ACCEPT
sudo iptables -P FORWARD ACCEPT
sudo iptables -P OUTPUT ACCEPT
sudo iptables -t nat -F
sudo iptables -t mangle -F
sudo iptables -F
sudo iptables -X
wget https://github.com/PoliNetworkOrg/PoliFemoBackend/releases/latest/download/PoliFemoBackend.zip -q -O PoliFemoBackend.zip
unzip -o PoliFemoBackend.zip
sudo chmod +x run.sh
cp appsettings_production.json appsettings.json
SSLCert=$(cat secrets.json | jq -r '.SSLCert')
cat appsettings.json | jq --arg SSLCert "$SSLCert" '.Kestrel.Endpoints.HttpsInlineCertAndKeyFile.Certificate.Password |= $SSLCert' > appsettings.json.tmp
cp appsettings.json.tmp appsettings.json
screen -S backend -p 0 -X stuff "sudo ./PoliFemoBackend\n"