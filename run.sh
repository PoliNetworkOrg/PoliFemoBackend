#!/bin/sh
while [ 1 != 0 ]
do
        pkill PoliFemoBackend
        sleep 5
        wget https://github.com/PoliNetworkOrg/PoliFemoBackend/releases/latest/download/PoliFemoBackend -O /home/ubuntu/PoliFemoBackend/PoliFemoBackend
        chmod +x PoliFemoBackend
        screen ./PoliFemoBackend
        sleep 1d
done