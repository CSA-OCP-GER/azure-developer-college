# Break Out #1: Create CI/CD pipelines to deploy the Azure Dev College sample application to Azure

## Here is what you will learn
Deploy the sample application to Azure.

In [challenge-3](./challenge-3.md) and [challenge-4](./challenge-4.md) you have learned how to create a CI/CD Pipeline to continuously and consistently deploy services to Azure.
You have learned how to use PullRequests, validation builds and how you track your work with Azure Boards. You may have noticed that there is still some work left to do until the sample application is deployed to your Develeopment and Testing stage.

In this Break Out Session we want you to deploy the remaining Microservices to your stages:
- SCM Resource API
- SCM Search API
- SCM Visitreports API
- SCM Frontend

As in [challenge-4](./challenge-4.md) we will always perform the following steps for each service:
1. Set the corresponding UserStories to active
2. Create a new feature branch and check it out
3. Create the CI Build definition and validate it
4. Create a the PR validation Build
5. Create the CD Build for the stages *Development* and *Testing*
6. Merge the feature branch into the master branch
7. Update your master branch's policies to trigger the PR Build to validate a PullRequest
8. Test the build flow
9. Complete the UserStories


## SCM Resource API

Corresponding UserStories: __S6__ and __S7__

Feature branch: __features/scmresourceapicicd__

Projects to build: __apps/dotnetcore/Scm.Resources/Adc.Scm.Resources.Api__ and __Adc.Scm.Resources.ImageResizer__

Project runtime: __dotnetcore__, __ASP.NET Core__ and __AzureFunctions__

ARM Templates: __apps/infrastructure/templates/scm-resources-api-dotnetcore.json__

CI Build name: __SCM-Resources-CI__

PR Build name: __SCM-Resource-PR__

CD Build name: __SCM-Resources-CD__

CD Build variables stage *Development*:

   | Variable | Value | Scope | ARM Template Parameter |
   |----------|-------|-------|------------------------|
   |ResourceGroupName | ADC-DAY4-SCM-DEV | Development | |
   |Location| westeurope|Development| |
   |ApiAppName|__'prefix'__-day4scmresourceapi-dev|Development| webAppName |
   |AppServicePlanSKU|B1|Development| sku |
   |Use32BitWorker|false|Development| use32bitworker |
   |AlwaysOn|true|Development| alwaysOn|
   |StorageAccountName|__'prefix'__-day4scmres-dev|Development| storageAccountName |
   |ResizerFunctionName|__'prefix'__-day4resizer-dev|Development| functionAppName |
   |ApplicationInsightsName|your ApplicationInsights instance name of stage Development|Development| applicationInsightsName |
   |ServiceBusNamespaceName|your ServiceBus namespace name of stage Development|Development| serviceBusNamespaceName |


CD Build variables stage *Testing*:

   | Variable | Value | Scope | ARM Template Parameter |
   |----------|-------|-------|------------------------|
   |ResourceGroupName | ADC-DAY4-SCM-TEST | Testing | |
   |Location| westeurope|Testing| |
   |ApiAppName|__'prefix'__-day4scmresourcesapi-test|Testing| webAppName |
   |AppServicePlanSKU|B1|Testing| sku |
   |Use32BitWorker|false|Testing| use32bitworker |
   |AlwaysOn|true|Testing| alwaysOn |
   |StorageAccountName|__'prefix'__-day4scmres-test|Testing| storageAccountName |
   |ResizerFunctionName|__'prefix'__-day4resizer-test|Testing| functionAppName |
   |ApplicationInsightsName|your ApplicationInsights instance name of stage Testing|Testing| applicationInsightsName |
   |ServiceBusNamespaceName|your ServiceBus namespace name of stage Testing|Testing| serviceBusNamespaceName|


## SCM Search API

Corresponding UserStories: __S8__ and __S9__

Feature branch: __features/scmsearchapicicd__

Projects to build: __apps/dotnetcore/Scm.Search/Adc.Scm.Search.Api__ and __Adc.Scm.Search.Indexer__

Project runtime: __dotnetcore__, __ASP.NET Core__, __AzureFunctions__

ARM Templates: __apps/infrastructure/templates/scm-search-api-dotnetcore.json__

CI Build name: __SCM-Search-CI__

PR Build name: __SCM-Search-PR__

CD Build name: __SCM-Search-CD__

CD Build variables stage *Development*:

   | Variable | Value | Scope | ARM Template Parameter |
   |----------|-------|-------|------------------------|
   |ResourceGroupName | ADC-DAY4-SCM-DEV | Development | |
   |Location| westeurope|Development| |
   |ApiAppName|__'prefix'__-day4scmsearchapi-dev|Development| webAppName |
   |AppServicePlanSKU|B1|Development| appPlanSKU |
   |Use32BitWorker|false|Development| use32bitworker |
   |AlwaysOn|true|Development| alwaysOn|
   |StorageAccountName|__'prefix'__-day4scmsearch-dev|Development| storageAccountName |
   |IndexerFunctionName|__'prefix'__-day4indexer-dev|Development| functionAppName |
   |ApplicationInsightsName|your ApplicationInsights instance name of stage Development|Development| applicationInsightsName |
   |ServiceBusNamespaceName|your ServiceBus namespace name of stage Development|Development| serviceBusNamespaceName |
   |AzureSearchServiceName|__'prefix'__-day4search-dev|Development|azureSearchServiceName|
   |AzureSearchSKU|Basic|Development|azureSearchSKU|
   |AzureSearchReplicaCount|1|Development|azureSearchReplicaCount|
   |AzureSearchPartitionCount|1|Development|azureSearchPartitionCount|


CD Build variables stage *Testing*:

   | Variable | Value | Scope | ARM Template Parameter |
   |----------|-------|-------|------------------------|
   |ResourceGroupName | ADC-DAY4-SCM-TEST | Testing | |
   |Location| westeurope|Testing| |
   |ApiAppName|__'prefix'__-day4scmsearchapi-dev|Testing| webAppName |
   |AppServicePlanSKU|B1|Testing| appPlanSKU |
   |Use32BitWorker|false|Testing| use32bitworker |
   |AlwaysOn|true|Testing| alwaysOn|
   |StorageAccountName|__'prefix'__-day4scmsearch-dev|Testing| storageAccountName |
   |IndexerFunctionName|__'prefix'__-day4indexer-dev|Testing| functionAppName |
   |ApplicationInsightsName|your ApplicationInsights instance name of stage Testing|Testing| applicationInsightsName |
   |ServiceBusNamespaceName|your ServiceBus namespace name of stage Testing|Testing| serviceBusNamespaceName |
   |AzureSearchServiceName|__'prefix'__-day4search-dev|Testing|azureSearchServiceName|
   |AzureSearchSKU|Basic|Testing|azureSearchSKU|
   |AzureSearchReplicaCount|1|Testing|azureSearchReplicaCount|
   |AzureSearchPartitionCount|1|Testing|azureSearchPartitionCount|
