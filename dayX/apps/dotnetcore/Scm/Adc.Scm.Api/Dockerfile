# Setup environment
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

## Setup build space
WORKDIR /app
COPY . ./

## 
#### This is a great place to set up environment information
#### such as versioning, build time etc.
##

## Build app
RUN dotnet publish -o /app/out -c Release ./Adc.Scm.Api/Adc.Scm.Api.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

## Copy build
WORKDIR /app
COPY --from=build /app/out .

# Select non-root port
ENV ASPNETCORE_URLS=http://+:5000

# copy run daprd script
COPY Adc.Scm.Api/runapi.sh .
# copy dapr compinents
COPY Adc.Scm.Api/components components
# install dapr runtime (daprd)
RUN apt-get update
RUN apt-get install -y wget
RUN wget https://github.com/dapr/dapr/releases/download/v0.4.1/daprd_linux_amd64.tar.gz
RUN tar -zxvf daprd_linux_amd64.tar.gz

# Do not run as root user
RUN chown -R www-data:www-data /app
USER www-data

# Launch dll
#ENTRYPOINT ["dotnet", "Adc.Scm.Api.dll"]
# Launsch dll and daprd
CMD ["/bin/bash", "runapi.sh", "scmcontacts", "5000", "3500", "50000"]