#!/bin/sh
sudo iptables -P INPUT ACCEPT
sudo iptables -P FORWARD ACCEPT
sudo iptables -P OUTPUT ACCEPT
sudo iptables -t nat -F
sudo iptables -t mangle -F
sudo iptables -F
sudo iptables -X
if [ "$1" = "dev" ]
  then
    echo "Downloading dev build..."
    GH_PAT=$(cat secrets.json | jq -r '.GitHubToken')
    ARTIFACT_URL=$(curl -s https://api.github.com/repos/PoliNetworkOrg/PoliFemoBackend/actions/artifacts | jq -r ".artifacts[0].archive_download_url")
    wget --header="Authorization: Bearer $GH_PAT" $ARTIFACT_URL -q
    unzip -o zip
  else
    echo "Downloading prod build..."
    wget https://github.com/PoliNetworkOrg/PoliFemoBackend/releases/latest/download/PoliFemoBackend.zip -q -O PoliFemoBackend.zip
fi

unzip -o PoliFemoBackend.zip
sudo chmod +x run.sh
cp appsettings_production.json appsettings.json
SSLCert=$(cat secrets.json | jq -r '.SSLCert')
cat appsettings.json | jq --arg SSLCert "$SSLCert" '.Kestrel.Endpoints.HttpsInlineCertAndKeyFile.Certificate.Password |= $SSLCert' > appsettings.json.tmp
if [ "$1" = "dev" ]
  then
    cat appsettings.json.tmp | jq '.Kestrel.Endpoints.HttpsInlineCertAndKeyFile.Url = "https://api.polinetwork.org:446" | del(.. | .Http?)' > appsettings.json.temp
    cp appsettings.json.temp appsettings.json
  else
    cp appsettings.json.tmp appsettings.json
fi

sudo ./PoliFemoBackend