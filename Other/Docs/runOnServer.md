# Generazione del certificato

```
	mkdir PoliFemoBackend && cd PoliFemoBackend
	wget https://raw.githubusercontent.com/PoliNetworkOrg/PoliFemoBackend/main/run.sh
	sudo chmod +x run.sh
	wget https://raw.githubusercontent.com/PoliNetworkOrg/PoliFemoBackend/main/appsettings_production.json -O appsettings.json
	sudo apt install certbot
	sudo certbot certonly --standalone -d api.polinetwork.org
	mkdir conf.d && cd conf.d
	mkdir https
	sudo openssl pkcs12 -export -out $HOME/PoliFemoBackend/conf.d/https/dev_cert.pfx -inkey /etc/letsencrypt/live/api.polinetwork.org/privkey.pem -in /etc/letsencrypt/live/api.polinetwork.org/cert.pem -certfile /etc/letsencrypt/live/api.polinetwork.org/chain.pem
	cd ..
```

Inserire password a scelta
Inserirla nel file `appsettings.json`

```
	screen sudo ./run.sh
```