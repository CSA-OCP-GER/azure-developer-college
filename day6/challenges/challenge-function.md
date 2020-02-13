# Create an Azure function on Linux using a custom container

## Here is what you will learn
- Create an Azure function that uses a BlobTrigger
- Build a custom image using Docker
- Run the container on your local development machine


In this challenge you will learn how to use the Azure function runtime in a custom docker image. To get familiar with running the Azure function runtime in a custom docker image a sample Azure function is already created for you. The sample function listens for files in a StorageAccount (Blob). Each time a file is uploaded to a predefined container, the sample fumction is triggered and it receives the uploaded Blob. The blob is resized and stored to another location.

## Build the custom image using Docker

```Shell
docker build -t blobtriggerfunction:0.1 .
```

## Run the image

```Shell
docker run -it -e StorageAccountConnectionString='<your connectionstring>' -e AzureWebJobsStorage='<your connectionstring>'
```