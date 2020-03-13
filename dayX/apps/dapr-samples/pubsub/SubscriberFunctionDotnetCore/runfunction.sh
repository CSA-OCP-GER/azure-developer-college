#!/bin/bash

./azure-functions-host/Microsoft.Azure.WebJobs.Script.WebHost &
sleep 3
./daprd --dapr-id $1 --app-port $2 --dapr-http-port $3 --dapr-grpc-port $4