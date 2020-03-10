# get dotnetcore sdk image to build the function project
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
COPY . /src/dotnet-function-app
RUN cd /src/dotnet-function-app && mkdir -p /home/site/wwwroot && dotnet publish *.csproj --output /home/site/wwwroot

# get the azure function image
FROM mcr.microsoft.com/azure-functions/dotnet:3.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true\
    ASPNETCORE_URLS=http://+:7071

# copy files from build image to runtime image
COPY --from=build ["/home/site/wwwroot", "/home/site/wwwroot"]

# copy run daprd script
COPY runfunction.sh .
# copy dapr components
COPY components components
# install dapr runtime (daprd) 
RUN apt-get update
RUN apt-get install -y wget
RUN wget https://github.com/dapr/dapr/releases/download/v0.4.1/daprd_linux_amd64.tar.gz
RUN tar -zxvf daprd_linux_amd64.tar.gz
# run daprd and azure function
CMD ["/bin/bash", "runfunction.sh", "scmindexer", "7071", "3500", "50000"]
