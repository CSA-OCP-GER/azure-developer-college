# Challenge dapr docker

## Here is what you will learn
- run a PubSub application in docker with dapr

### Setup Docker network

We need to create a new custom bridge network to connect containers running on the same Docker host.
With a custom bridge network it is possible for containers to communicate using the name of the container.

Setup your custom bridge network:

```Shell
docker network create --driver bridge azuredevcollege-net
```

Inspect the 'azuredevcollege-network'. This shows you its IP adress and the fact that no containers are connected to it.

```
docker inspect network azuredevcollege-net
```

### Setup Dapr-CLI on your local development machine

Open your browser and navigate to [https://github.com/dapr/cli](https://github.com/dapr/cli) an follow the instruction to download the Dapr-CLI.
After you have downloaded the binaries and installed the Dapr-CLI, wee need to initialize dapr in our newly created docker network.

```
dapr init --network azuredevcollege-net
```

After dapr was initialized, take a look at the created docker containers:

```Shell
docker ps -f network=azuredevcollege-net
```

You will see a dapr placement container and a dapr redis container. The placement container is only used when you want to use the dapr actor model.
The redis container was setup to have a state store and a pub/sub streaming service.

### Build the docker images for the Publisher

Open a shell and navidate to 'apps/dapr-sample/pubsub/PublisherAspNetCore' and run the following docker command to build the image:

```Shell
docker build -t publisheraspnetcore:0.1 -f Dockerfile-daprd .
```

### Run the Publisher

Now we can run the docker image in the newly created network 'azuredevcollege-net' and forward the local port 8080 to the containers port 5000 to access the Publisher from our local machine.

```Shell
docker run --name publisheraspnetcore --network azuredevcollege-net -p 8080:5000 --mount type=bind,source=$(pwd)/components-daprd,target=/app/components publisheraspnetcore:0.1
```

Now open your browser and navigate to [http://localhost:8080/publisher/swagger](http://localhost:5000/publisher/swagger) and publish a message.

### Build the docker images for the Subscriber (AspNetCore)

Open another shell and navidate to 'apps/dapr-sample/pubsub/SubscriberAspNetCore' and run the following docker command to build the image:

```Shell
docker build -t subscriberaspnetcore:0.1 -f ./Dockerfile.daprd .
```

### Run the Subscriber (AspNetCore)

Now we can run the docker image in the newly created network 'azuredevcollege-net.

```Shell
docker run --name subscriberaspnetcore --network azuredevcollege-net --mount type=bind,source=$(pwd)/components-daprd,target=/app/components subscriberaspnetcore:0.1
```

You see that the container is subscribed to the 'mytopic' topic:

```Shell
time="2020-03-13T17:09:38.4055135Z" level=info msg="App is subscribed to the following topics: [mytopic]" app_id=subscriberaspnetcore instance=1b722840af07 scope=dapr.runtime type=log ver=0.5.0
```

Now switch back to your browser and publish a message and watch the output logs from the subscriber shell.
