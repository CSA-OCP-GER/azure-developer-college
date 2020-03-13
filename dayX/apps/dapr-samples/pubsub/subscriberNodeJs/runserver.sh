#!/bin/sh
npm start & 
sleep 3
./daprd --dapr-id $1 --app-port $2 --dapr-http-port $3 --dapr-grpc-port $4