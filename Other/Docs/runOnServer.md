# Generazione del certificato

```
	mkdir PoliFemoBackend
	cd PoliFemoBackend
	sudo apt install certbot
	sudo certbot certonly --standalone -d api.polinetwork.org
	sudo openssl pkcs12 -export -out $HOME/PoliFemoBackend/conf.d/https/dev_cert.pfx -inkey /etc/letsencrypt/live/api.polinetwork.org/privkey.pem -in /etc/letsencrypt/live/api.polinetwork.org/cert.pem -certfile /etc/letsencrypt/live/api.polinetwork.org/chain.pem
```

Inserire password a scelta
Inserirla nel file 