# Generazione del certificato

Do per scontato che sia già configurato un reverse proxy che riscrive automaticamente verso https://localhost:5001 quando arrivi da http://api.polinetwork.org oppure da https://api.polinetwork.org

```
	mkdir PoliFemoBackend
	cd PoliFemoBackend
	wget https://raw.githubusercontent.com/PoliNetworkOrg/PoliFemoBackend/main/run.sh
	sudo chmod +x run.sh
	wget https://raw.githubusercontent.com/PoliNetworkOrg/PoliFemoBackend/main/appsettings.json
	sudo apt install certbot
	sudo certbot certonly --standalone -d api.polinetwork.org
	sudo openssl pkcs12 -export -out $HOME/PoliFemoBackend/conf.d/https/dev_cert.pfx -inkey /etc/letsencrypt/live/api.polinetwork.org/privkey.pem -in /etc/letsencrypt/live/api.polinetwork.org/cert.pem -certfile /etc/letsencrypt/live/api.polinetwork.org/chain.pem
```

Inserire password a scelta
Inserirla nel file `appsettings.json`

```
	screen ./run.sh
```