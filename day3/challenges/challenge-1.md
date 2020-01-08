# Azure Cosmos DB #

## Here is what you will learn ##

- Create a Cosmos DB
- Add Data to Cosmos DB
- Create a Partition Key
- Use ARM Template for automated deployment

## Create a Cosmos DB ##

Create a Cosmos DB:

- Create a resource group
  - westeurope
- Add Cosmos DB
- Create an unique Account Name
- Select API (Core SQL/MongoDB API/ Cassandra/Azure Table/Gremlin(graph))
- Location: westeurope
- Geo-Redundancy: Disable
- Multi-region Writes: Enable
- Availability Zones: Disable 
- Hit "Create"

## Add Data to Cosmos DB ##

- Select Data Explorer from the left navigation on your Azure Cosmos DB account page, and then select New Container.
- In the Add container pane, enter the settings for the new container.
  - Database ID: name
  - Throughput: 400
  - Container ID: Items
  - Partition key
- Hit "Create"
- In Data Explorer, expand the (name) database, and expand the Items container. Next, select Items, and then select New Item.
- Add the following structure to the document on the right side of the Documents pane:
  - {
    "id": "1",
    "category": "personal",
    "name": "groceries",
    "description": "Pick up apples and strawberries.",
    "isComplete": false
    }

- Hit "Save"

- Select New Document again, and create and save another document with a unique id, and any other properties and values you   want. Your documents can have any structure, because Azure Cosmos DB doesn't impose any schema on your data.

- At the top of the Documents tab in Data Explorer, review the default query SELECT * FROM c. This query retrieves and displays all documents in the collection in ID order.

- To change the query, select Edit Filter, replace the default query with ORDER BY c._ts DESC, and then select Apply Filter.

## Create a Partition Key ##

- A single logical partition has an upper limit of 10 GB of storage.
- Azure Cosmos containers have a minimum throughput of 400 request units per second (RU/s). When throughput is provisioned on a   database, minimum RUs per container is 100 request units per second (RU/s). Requests to the same partition key can't exceed     the throughput that's allocated to a partition. If requests exceed the allocated throughput, requests are rate-limited. So,     it's important to pick a partition key that doesn't result in "hot spots" within your application.
- Choose a partition key that has a wide range of values and access patterns that are evenly spread across logical partitions.    This helps spread the data and the activity in your container across the set of logical partitions, so that resources for       data storage and throughput can be distributed across the logical partitions.
- Choose a partition key that spreads the workload evenly across all partitions and evenly over time. Your choice of partition    key should balance the need for efficient partition queries and transactions against the goal of distributing items across      multiple partitions to achieve scalability.
- Candidates for partition keys might include properties that appear frequently as a filter in your queries. Queries can be       efficiently routed by including the partition key in the filter predicate.
- Create unique Partition Key
- You can form a partition key by concatenating multiple property values into a single artificial partitionKey property. These    keys are referred to as synthetic keys. For example, consider the following example document:
    - {
    "deviceId": "abc-123",
    "date": 2018
    }
- For the previous document, one option is to set /deviceId or /date as the partition key. Use this option, if you want to        partition your container based on either device ID or date. Another option is to concatenate these two values into a            synthetic partitionKey property that's used as the partition key.
    - {
    "deviceId": "abc-123",
    "date": 2018,
    "partitionKey": "abc-123-2018"
    }


## Use ARM Template for automated deployment ##

- The following Azure Resource Manager template creates an Azure Cosmos account with:
    - Two containers that share 400 Requested Units per second (RU/s) throughput at the database level.
    - One container with dedicated 400 RU/s throughput.

- To create the Azure Cosmos DB resources, copy the following example template and deploy it as described, 
  either via PowerShell or Azure CLI.

- {
    "$schema": "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#",
    "contentVersion": "1.0.0.0",
    "parameters": {
        "accountName": {
            "type": "string",
            "defaultValue": "[concat('sql-', uniqueString(resourceGroup().id))]",
            "metadata": {
                "description": "Cosmos DB account name, max length 44 characters"
            }
        },
        "location": {
            "type": "string",
            "defaultValue": "[resourceGroup().location]",
            "metadata": {
                "description": "Location for the Cosmos DB account."
            }
        },
        "primaryRegion":{
            "type":"string",
            "metadata": {
                "description": "The primary replica region for the Cosmos DB account."
            }
        },
        "secondaryRegion":{
            "type":"string",
            "metadata": {
              "description": "The secondary replica region for the Cosmos DB account."
          }
        },
        "defaultConsistencyLevel": {
            "type": "string",
            "defaultValue": "Session",
            "allowedValues": [ "Eventual", "ConsistentPrefix", "Session", "BoundedStaleness", "Strong" ],
            "metadata": {
                "description": "The default consistency level of the Cosmos DB account."
            }
        },
        "maxStalenessPrefix": {
            "type": "int",
            "minValue": 10,
            "defaultValue": 100000,
            "maxValue": 2147483647,
            "metadata": {
                "description": "Max stale requests. Required for BoundedStaleness. Valid ranges, Single Region: 10 to 1000000. Multi Region: 100000 to 1000000."
            }
        },
        "maxIntervalInSeconds": {
            "type": "int",
            "minValue": 5,
            "defaultValue": 300,
            "maxValue": 86400,
            "metadata": {
                "description": "Max lag time (minutes). Required for BoundedStaleness. Valid ranges, Single Region: 5 to 84600. Multi Region: 300 to 86400."
            }
        },	
        "multipleWriteLocations": {
            "type": "bool",
            "defaultValue": false,
            "allowedValues": [ true, false ],
            "metadata": {
                "description": "Enable multi-master to make all regions writable."
            }
        },
        "automaticFailover": {
            "type": "bool",
            "defaultValue": false,
            "allowedValues": [ true, false ],
            "metadata": {
                "description": "Enable automatic failover for regions. Ignored when Multi-Master is enabled"
            }
        },
        "databaseName": {
            "type": "string",
            "metadata": {
                "description": "The name for the SQL database"
            }
        },
        "sharedThroughput": {
            "type": "int",
            "defaultValue": 400,
            "minValue": 400,
            "maxValue": 1000000,
            "metadata": {
                "description": "The throughput for the database to be shared"
            }			
        },
        "sharedContainer1Name": {
            "type": "string",
            "defaultValue": "sharedContainer1",
            "metadata": {
                "description": "The name for the first container with shared throughput"
            }
        },
        "sharedContainer2Name": {
            "type": "string",
            "defaultValue": "sharedContainer2",
            "metadata": {
                "description": "The name for the second container with shared throughput"
            }
        },
        "dedicatedContainer1Name": {
            "type": "string",
            "defaultValue": "dedicatedContainer1",
            "metadata": {
                "description": "The name for the container with dedicated throughput"
            }
        },
        "dedicatedThroughput": {
            "type": "int",
            "defaultValue": 400,
            "minValue": 400,
            "maxValue": 1000000,
            "metadata": {
                "description": "The throughput for the container with dedicated throughput"
            }			
        }
    },
    "variables": {
        "accountName": "[toLower(parameters('accountName'))]",
        "consistencyPolicy": {
            "Eventual": {
                "defaultConsistencyLevel": "Eventual"
            },
            "ConsistentPrefix": {
                "defaultConsistencyLevel": "ConsistentPrefix"
            },
            "Session": {
                "defaultConsistencyLevel": "Session"
            },
            "BoundedStaleness": {
                "defaultConsistencyLevel": "BoundedStaleness",
                "maxStalenessPrefix": "[parameters('maxStalenessPrefix')]",
                "maxIntervalInSeconds": "[parameters('maxIntervalInSeconds')]"
            },
            "Strong": {
                "defaultConsistencyLevel": "Strong"
            }
        },
        "locations": 
        [ 
            {
                "locationName": "[parameters('primaryRegion')]",
                "failoverPriority": 0,
                "isZoneRedundant": false
            }, 
            {
                "locationName": "[parameters('secondaryRegion')]",
                "failoverPriority": 1,
                "isZoneRedundant": false
            }
        ]
    },
    "resources": 
    [
        {
            "type": "Microsoft.DocumentDB/databaseAccounts",
            "name": "[variables('accountName')]",
            "apiVersion": "2019-08-01",
            "kind": "GlobalDocumentDB",
            "location": "[parameters('location')]",
            "properties": {
                "consistencyPolicy": "[variables('consistencyPolicy')[parameters('defaultConsistencyLevel')]]",
                "locations": "[variables('locations')]",
                "databaseAccountOfferType": "Standard",
                "enableAutomaticFailover": "[parameters('automaticFailover')]",
                "enableMultipleWriteLocations": "[parameters('multipleWriteLocations')]"
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlDatabases",
            "name": "[concat(variables('accountName'), '/', parameters('databaseName'))]",
            "apiVersion": "2019-08-01",
            "dependsOn": [ "[resourceId('Microsoft.DocumentDB/databaseAccounts', variables('accountName'))]" ],
            "properties":{
                "resource":{
                    "id": "[parameters('databaseName')]"
                },
                "options": { "throughput": "[parameters('sharedThroughput')]" }
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers",
            "name": "[concat(variables('accountName'), '/', parameters('databaseName'), '/', parameters('sharedContainer1Name'))]",
            "apiVersion": "2019-08-01",
            "dependsOn": [ "[resourceId('Microsoft.DocumentDB/databaseAccounts/sqlDatabases', variables('accountName'), parameters('databaseName'))]" ],
            "properties":
            {
                "resource":{
                    "id":  "[parameters('sharedContainer1Name')]",
                    "partitionKey": {
                        "paths": [
                        "/myPartitionKey"
                        ],
                        "kind": "Hash"
                    },
                    "indexingPolicy": {
                        "indexingMode": "consistent",
                        "includedPaths": [{
                                "path": "/*"
                            }
                        ],
                        "excludedPaths": [{
                                "path": "/myPathToNotIndex/*"
                            }
                        ]
                    }
                }
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers",
            "name": "[concat(variables('accountName'), '/', parameters('databaseName'), '/', parameters('sharedContainer2Name'))]",
            "apiVersion": "2019-08-01",
            "dependsOn": [ "[resourceId('Microsoft.DocumentDB/databaseAccounts/sqlDatabases', variables('accountName'), parameters('databaseName'))]" ],
            "properties":
            {
                "resource":{
                    "id":  "[parameters('sharedContainer2Name')]",
                    "partitionKey": {
                        "paths": [
                        "/myPartitionKey"
                        ],
                        "kind": "Hash"
                    },
                    "indexingPolicy": {
                        "indexingMode": "consistent",
                        "includedPaths": [{
                                "path": "/*"
                            }
                        ],
                        "excludedPaths": [{
                                "path": "/myPathToNotIndex/*"
                            }
                        ]
                    }
                }
            }
        },
        {
            "type": "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers",
            "name": "[concat(variables('accountName'), '/', parameters('databaseName'), '/', parameters('dedicatedContainer1Name'))]",
            "apiVersion": "2019-08-01",
            "dependsOn": [ "[resourceId('Microsoft.DocumentDB/databaseAccounts/sqlDatabases', variables('accountName'), parameters('databaseName'))]" ],
            "properties":
            {
                "resource":{
                    "id":  "[parameters('dedicatedContainer1Name')]",
                    "partitionKey": {
                        "paths": [
                        "/myPartitionKey"
                        ],
                        "kind": "Hash"
                    },
                    "indexingPolicy": {
                        "indexingMode": "consistent",
                        "includedPaths": [{
                                "path": "/*"
                            }
                        ],
                        "excludedPaths": [{
                                "path": "/myPathToNotIndex/*"
                            }
                        ],
                        "compositeIndexes":[  
                        [
                            {
                                "path":"/name",
                                "order":"ascending"
                            },
                            {
                                "path":"/age",
                                "order":"descending"
                            }
                        ]
                    ],
                    "spatialIndexes": [
                            {
                                "path": "/path/to/geojson/property/?",
                                "types": [
                                    "Point",
                                    "Polygon",
                                    "MultiPolygon",
                                    "LineString"
                                ]
                            }
                        ]
                    },
                    "defaultTtl": 86400,
                    "uniqueKeyPolicy": {
                        "uniqueKeys": [
                        {
                            "paths": [
                            "/phoneNumber"
                            ]
                        }
                        ]
                    }
                },
                "options": { "throughput": "[parameters('dedicatedThroughput')]" }
            }
        }
    ]
}

- To use Azure CLI to deploy the Azure Resource Manager template:
    - Copy the script.
    - Select Try it to open Azure Cloud Shell.
    - Right-click in the Azure Cloud Shell window, and then select Paste.

         -  read -p 'Enter the Resource Group name: ' resourceGroupName
            read -p 'Enter the location (i.e. westus2): ' location
            read -p 'Enter the account name: ' accountName
            read -p 'Enter the primary region (i.e. westus2): ' primaryRegion
            read -p 'Enter the secondary region (i.e. eastus2): ' secondaryRegion
            read -p 'Enter the database name: ' databaseName
            read -p 'Enter the shared database throughput: sharedThroughput
            read -p 'Enter the first shared container name: ' sharedContainer1Name
            read -p 'Enter the second shared container name: ' sharedContainer2Name
            read -p 'Enter the dedicated container name: ' dedicatedContainer1Name
            read -p 'Enter the dedicated container throughput: dedicatedThroughput

            az group create --name $resourceGroupName --location $location
            az group deployment create --resource-group $resourceGroupName \
            --template-uri https://raw.githubusercontent.com/azure/azure-quickstart-templates/master/101-cosmosdb-sql/azuredeploy.json \
            --parameters accountName=$accountName \
            primaryRegion=$primaryRegion \
            secondaryRegion=$secondaryRegion \
            databaseName=$databaseName \
            sharedThroughput=$sharedThroughput \
            sharedContainer1Name=$sharedContainer1Name \
            sharedContainer2Name=$sharedContainer2Name \
            dedicatedContainer1Name=$dedicatedContainer1Name \
            dedicatedThroughput=$dedicatedThroughput

            az cosmosdb show --resource-group $resourceGroupName --name accountName --output tsv


## Integrate Cosmos DB into Node.JS App ##

- The CosmosClient object is initialized.
    - const client = new CosmosClient({ endpoint, key });
- Create a new Azure Cosmos database.
    - const { database } = await client.databases.createIfNotExists({ id: databaseId });
- A new container (collection) is created within the database.
    - const { container } = await client.database(databaseId).containers.createIfNotExists({ id: containerId });
- An item (document) is created
    - const { item } = await client.database(databaseId).container(containerId).items.create(itemBody);

- A SQL query over JSON is performed on the family database. The query returns all the children of the "Anderson" family.
    - const querySpec = {
  	query: 'SELECT VALUE r.children FROM root r WHERE r.lastName = @lastName',
  	parameters: [
  	  {
  		name: '@lastName',
  		value: 'Andersen'
  	  }
  	]
    }
    const { resources: results } = await client
  	.database(databaseId)
  	.container(containerId)
  	.items.query(querySpec)
  	.fetchAll()
    for (var queryResult of results) {
  	let resultString = JSON.stringify(queryResult)
  	console.log(`\tQuery returned ${resultString}\n`)
    }

## Update your connection string ##

- Update your connection string
- In the Azure portal, in your Azure Cosmos account, in the left navigation click Keys, and then click Read-write Keys. You'll use the copy buttons on the right side of the screen to copy the URI and Primary Key into the config.js file in the next step.
- In Open the config.js file.
- Copy your URI value from the portal (using the copy button) and make it the value of the endpoint key in config.js.
  config.endpoint = "https://FILLME.documents.azure.com"
- Then copy your PRIMARY KEY value from the portal and make it the value of the config.key in config.js. You've now updated     your app with all the info it needs to communicate with Azure Cosmos DB.
config.key = "FILLME"