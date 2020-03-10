# Challenge dapr docker

## Here is what you will learn
- run the demo application and al its components in docker

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
docker ps
```

You will see a dapr placement container and a dapr redis container. The placement container is only used when you want to use the dapr actor model.
The redis container was setup to have a state store and pub/sub streaming service.

### Build the SCM Contact API docker image

To run the SCM Contact API we need to build the docker image first. 
Navigate to the directory 'apps/dotnetcore/Scm' and run the following docker command:

``` Shell
docker build -t scmcontacts:0.1 -f ./Adc.Scm.Api/Dockerfile .
```

After the image was created, we can run the docker container. Wee need to set two environment variables to run the container:
- ASPNETCORE_ENVIRONMENT -> Development
- ConnectionStrings__DefaultConnectionString -> DataSource=:memory: 
These environment variables configures the SCM Contacts APi to use SQL Lite in memory.

```Shell
docker run -p 8082:5000 --name scmcontacts --network azuredevcollege-net -e ConnectionStrings__DefaultConnectionString=DataSource=:memory: -e ASPNETCORE_ENVIRONMENT=Development scmcontacts:0.1
```
### Build the SCM Contacts Search Indexer

Navigate to the folder apps/dotnetcore/Scm.Search/Adc.Scm.Search.Indexer

```shell
docker build -t scmindexer:01 .
```

Run the container

```Shell
docker run -p 8081:7071 scmindexer:0.1
```