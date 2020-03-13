### Start in custom docker bridge network
docker run --network azuredevcollege-net --mount src=$(pwd)/components,target=/components,type=bind subscriberfunction:0.1