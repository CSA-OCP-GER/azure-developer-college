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

- [ARM Template Cosmos DB](/cosmos.json)

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

- Open a command prompt, create a new folder named git-samples, then close the command prompt.
    - md "C:\git-samples"
- Open a git terminal window, such as git bash, and use the cd command to change to the new folder to install the sample app.
    - cd "C:\git-samples"
- Run the following command to clone the sample repository. This command creates a copy of the sample app on your computer.
    - git clone https://github.com/Azure-Samples/azure-cosmos-db-sql-api-nodejs-getting-started.git

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


## Cross Partition Read ##


## Use Cosmos DB in Data Explorer ##



## Creating a Partitioned Container with .NET SDK ##

In this lab, you will create multiple Azure Cosmos DB containers using different partition keys and settings. In later labs, you will then use the SQL API and .NET SDK to query specific containers using a single partition key or across multiple partition keys.

> If you have not already completed setup for the lab content see the instructions for [Account Setup](00-account_setup.md) before starting this lab.

## Create Containers using the .NET SDK

> You will start by using the .NET SDK to create containers to use in the lab.

### Create a .NET Core Project

1. On your local machine, locate the CosmosLabs folder in your Documents folder and open the Lab01 folder that will be used to contain the content of your .NET Core project. If you are completing this lab through Microsoft Hands-on Labs, the CosmosLabs folder will be located at the path: **C:\labs\CosmosLabs**

1. In the Lab01 folder, right-click the folder and select the **Open with Code** menu option.

    ![Open with Visual Studio Code](../media/02-open_with_code.jpg)

    > Alternatively, you can run a terminal in your current directory and execute the ``code .`` command.

1. In the Visual Studio Code window that appears, right-click the **Explorer** pane and select the **Open in Terminal** menu option.

    ![Open in Terminal](../media/open_in_terminal.jpg)

1. In the open terminal pane, enter and execute the following command:

    ```sh
    dotnet new console --output .
    ```

    > This command will create a new .NET Core 2.2 project. The project will be a **console** project and the project will be created in the current directly since you used the ``--output .`` option.

1. Visual Studio Code will most likely prompt you to install various extensions related to **.NET Core** or **Azure Cosmos DB** development. None of these extensions are required to complete the labs.

1. In the terminal pane, enter and execute the following command:

    ```sh
    dotnet add package Microsoft.Azure.Cosmos --version 3.0.0
    ```

    > This command will add the [Microsoft.Azure.Cosmos](https://www.nuget.org/packages/Microsoft.Azure.Cosmos/) NuGet package as a project dependency. The lab instructions have been tested using the ``3.0.0`` version of this NuGet package.

1. In the terminal pane, enter and execute the following command:

    ```sh
    dotnet add package Bogus --version 22.0.8
    ```

    > This command will add the [Bogus](../media/https://www.nuget.org/packages/Bogus/) NuGet package as a project dependency. This library will allow us to quickly generate test data using a fluent syntax and minimal code. We will use this library to generate test documents to upload to our Azure Cosmos DB instance. The lab instructions have been tested using the ``22.0.8`` version of this NuGet package.

1. In the terminal pane, enter and execute the following command:

    ```sh
    dotnet restore
    ```

    > This command will restore all packages specified as dependencies in the project.

1. In the terminal pane, enter and execute the following command:

    ```sh
    dotnet build
    ```

    > This command will build the project.

1. Click the **🗙** symbol to close the terminal pane.

1. Observe the **Program.cs** and **[folder name].csproj** files created by the .NET Core CLI.

    ![Project files](../media/02-project_files.jpg)

1. Double-click the **[folder name].csproj** link in the **Explorer** pane to open the file in the editor.

1. We will now add a new **PropertyGroup** XML element to the project configuration within the **Project** element. To add a new **PropertyGroup**, insert the following lines of code under the line that reads ``<Project Sdk="Microsoft.NET.Sdk">``:

    ```xml
    <PropertyGroup>
        <LangVersion>latest</LangVersion>
    </PropertyGroup>
    ```

1. Your new XML should look like this:

    ```xml
    <Project Sdk="Microsoft.NET.Sdk">        
        <PropertyGroup>
            <LangVersion>latest</LangVersion>
        </PropertyGroup>        
        <PropertyGroup>
            <OutputType>Exe</OutputType>
            <TargetFramework>netcoreapp2.2</TargetFramework>
        </PropertyGroup>        
        <ItemGroup>
            <PackageReference Include="Bogus" Version="22.0.8" />
            <PackageReference Include="Microsoft.Azure.Cosmos" Version="3.0.0" />
        </ItemGroup>        
    </Project>
    ```

1. Double-click the **Program.cs** link in the **Explorer** pane to open the file in the editor.

    ![Open editor](../media/02-program_editor.jpg)

### Create CosmosClient Instance

*The CosmosClient class is the main "entry point" to using the SQL API in Azure Cosmos DB. We are going to create an instance of the **CosmosClient** class by passing in connection metadata as parameters of the class' constructor. We will then use this class instance throughout the lab.*

1. Within the **Program.cs** editor tab, Add the following using blocks to the top of the editor:

    ```csharp
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos;
    ```

1. Locate the **Program** class and replace it with the following class:

    ```csharp
    public class Program
    {
        public static async Task Main(string[] args)
        {         
        }
    }
    ```

1. Within the **Program** class, add the following lines of code to create variables for your connection information:

    ```csharp
    private static readonly string _endpointUri = "";
    private static readonly string _primaryKey = "";
    ```

1. For the ``_endpointUri`` variable, replace the placeholder value with the **URI** value and for the ``_primaryKey`` variable, replace the placeholder value with the **PRIMARY KEY** value from your Azure Cosmos DB account. Use [these instructions](00-account_setup.md) to get these values if you do not already have them:

    > For example, if your **uri** is ``https://cosmosacct.documents.azure.com:443/``, your new variable assignment will look like this: ``private static readonly string _endpointUri = "https://cosmosacct.documents.azure.com:443/";``.

    > For example, if your **primary key** is ``elzirrKCnXlacvh1CRAnQdYVbVLspmYHQyYrhx0PltHi8wn5lHVHFnd1Xm3ad5cn4TUcH4U0MSeHsVykkFPHpQ==``, your new variable assignment will look like this: ``private static readonly string _primaryKey = "elzirrKCnXlacvh1CRAnQdYVbVLspmYHQyYrhx0PltHi8wn5lHVHFnd1Xm3ad5cn4TUcH4U0MSeHsVykkFPHpQ==";``.

    > Keep the **URI** and **PRIMARY KEY** values recorded, you will use them again later in this lab.

1. Locate the **Main** method:

    ```csharp
    public static async Task Main(string[] args)
    { 
    }
    ```

1. Within the **Main** method, add the following lines of code to author a using block that creates and disposes a **CosmosClient** instance:

    ```csharp
    using (CosmosClient client = new CosmosClient(_endpointUri, _primaryKey))
    {        
    }
    ```

1. Your ``Program`` class definition should now look like this:

    ```csharp
    public class Program
    { 
        private static readonly string _endpointUri = "<your uri>";
        private static readonly string _primaryKey = "<your key>";
        public static async Task Main(string[] args)
        {    
            using (CosmosClient client = new CosmosClient(_endpointUri, _primaryKey))
            {
            }     
        }
    }
    ```

    > We will now execute a build of the application to make sure our code compiles successfully.

1. Save all of your open editor tabs.

1. In the Visual Studio Code window, right-click the **Explorer** pane and select the **Open in Terminal** menu option.

1. In the open terminal pane, enter and execute the following command:

    ```sh
    dotnet build
    ```

    > This command will build the console project.

1. Click the **🗙** symbol to close the terminal pane.

1. Close all open editor tabs.

### Create Database using the SDK

1. Locate the using block within the **Main** method:

    ```csharp
    using (CosmosClient client = new CosmosClient(_endpointUri, _primaryKey))
    {                        
    }
    ```

1. Add the following code to the method to create a new ``Database`` instance if one does not already exist:

    ```csharp
    DatabaseResponse databaseResponse = await client.CreateDatabaseIfNotExistsAsync("EntertainmentDatabase");
    Database targetDatabase = databaseResponse.Database;
    ```

    > This code will check to see if a database exists in your Azure Cosmos DB account that meets the specified parameters. If a database that matches does not exist, it will create a new database.

1. Add the following code to print out the ID of the database:

    ```csharp
    await Console.Out.WriteLineAsync($"Database Id:\t{targetDatabase.Id}");
    ```

    > The ``targetDatabase`` variable will have metadata about the database whether a new database is created or an existing one is read.

1. Save all of your open editor tabs.

1. In the Visual Studio Code window, right-click the **Explorer** pane and select the **Open in Terminal** menu option.

1. In the open terminal pane, enter and execute the following command:

    ```sh
    dotnet run
    ```

    > This command will build and execute the console project.

1. Observe the output of the running command.

    > In the console window, you will see the ID string for the database resource in your Azure Cosmos DB account.

1. In the open terminal pane, enter and execute the following command again:

    ```sh
    dotnet run
    ```

    > This command will build and execute the console project.

1. Again, observe the output of the running command.

    > Since the database already exists, the SDK detected that the database already exists and used the existing database instance instead of creating a new instance of the database.

1. Click the **🗙** symbol to close the terminal pane.


### Create a Partitioned Container using the SDK

*To create a container, you must specify a name and a partition key path. You will specify those values when creating a container in this task. A partition key is a logical hint for distributing data onto a scaled out underlying set of physical partitions and for efficiently routing queries to the appropriate underlying partition. To learn more, refer to [/docs.microsoft.com/azure/cosmos-db/partition-data](../media/https://docs.microsoft.com/en-us/azure/cosmos-db/partition-data).*

1. Locate the using block within the **Main** method and delete any existing code:

    ```csharp
    using (CosmosClient client = new CosmosClient(_endpointUri, _primaryKey))
    {
    }
    ```

1. Add the following code to the method to create a reference to an existing database:

    ```csharp
    Database targetDatabase = client.GetDatabase("EntertainmentDatabase");
    ```

1. Add the following code to create a new ``IndexingPolicy`` instance with a custom indexing policy configured:

    ```csharp
    IndexingPolicy indexingPolicy = new IndexingPolicy
    {
        IndexingMode = IndexingMode.Consistent,
        Automatic = true,
        IncludedPaths =
        {
            new IncludedPath
            {
                Path = "/*"
            }
        }
    };
    ```

    > By default, all Azure Cosmos DB data is indexed. Although many customers are happy to let Azure Cosmos DB automatically handle all aspects of indexing, you can specify a custom indexing policy for containers. This indexing policy is very similar to the default indexing policy created by the SDK.

1. Add the following code to create a new ``ContainerProperties`` instance with a single partition key of ``/type`` defined and including the previously created ``IndexingPolicy``:

    ```csharp
    var containerProperties = new ContainerProperties("CustomCollection", "/type")
    {
        IndexingPolicy = indexingPolicy
    };
    ```

    > This definition will create a partition key on the ``/type`` path. Partition key paths are case sensitive. This is especially important when you consider JSON property casing in the context of .NET CLR object to JSON object serialization.

1. Add the following lines of code to create a new ``Container`` instance if one does not already exist within your database. Specify the previously created settings and a value for **throughput**:

    ```csharp
    var containerResponse = await targetDatabase.CreateContainerIfNotExistsAsync(containerProperties, 10000);
    var customContainer = containerResponse.Container;
    ```

    > This code will check to see if a container exists in your database that meets all of the specified parameters. If a container that matches does not exist, it will create a new container. Here is where we can specify the RU/s allocated for a newly created container. If this is not specified, the SDK has a default value for RU/s assigned to a container.

1. Add the following code to print out the ID of the database:

    ```csharp
    await Console.Out.WriteLineAsync($"Custom Container Id:\t{customContainer.Id}");
    ```

    > The ``customContainer`` variable will have metadata about the container whether a new container is created or an existing one is read.

1. Save all of your open editor tabs.

1. In the Visual Studio Code window, right-click the **Explorer** pane and select the **Open in Terminal** menu option.

1. In the open terminal pane, enter and execute the following command:

    ```sh
    dotnet run
    ```

    > This command will build and execute the console project.

1. Observe the output of the running command.

1. Click the **🗙** symbol to close the terminal pane.

1. Close all open editor tabs.


## Populate a Container with Items using the SDK

> You will now use the .NET SDK to populate your container with various items of varying schemas. These items will be serialized instances of multiple C# classes in your project.

### Populate Container with Data

1. In the Visual Studio Code window, look in the **Explorer** pane and verify that you have a **DataTypes.cs** file in your project folder.

    > This file contains the data classes you will be working with in the following steps.

1. Double-click the **Program.cs** link in the **Explorer** pane to open the file in the editor.

1. Locate the using block within the **Main** method and delete any existing code:

    ```csharp
    using (CosmosClient client = new CosmosClient(_endpointUri, _primaryKey))
    {                        
    }
    ```

1. Add the following code to the method to create a reference to an existing container:

    ```csharp
    var targetDatabase = client.GetDatabase("EntertainmentDatabase");
    var customContainer = targetDatabase.GetContainer("CustomCollection");
    ```

1. Observe the code in the **Main** method.

    > For the next few instructions, we will use the **Bogus** library to create test data. This library allows you to create a collection of objects with fake data set on each object's property. For this lab, our intent is to **focus on Azure Cosmos DB** instead of this library. With that intent in mind, the next set of instructions will expedite the process of creating test data.

1. Add the following code to create a collection of ``PurchaseFoodOrBeverage`` instances:

    ```csharp
    var foodInteractions = new Bogus.Faker<PurchaseFoodOrBeverage>()
        .RuleFor(i => i.id, (fake) => Guid.NewGuid().ToString())
        .RuleFor(i => i.type, (fake) => nameof(PurchaseFoodOrBeverage))
        .RuleFor(i => i.unitPrice, (fake) => Math.Round(fake.Random.Decimal(1.99m, 15.99m), 2))
        .RuleFor(i => i.quantity, (fake) => fake.Random.Number(1, 5))
        .RuleFor(i => i.totalPrice, (fake, user) => Math.Round(user.unitPrice * user.quantity, 2))
        .GenerateLazy(500);
    ```

    > As a reminder, the Bogus library generates a set of test data. In this example, you are creating 500 items using the Bogus library and the rules listed above. The **GenerateLazy** method tells the Bogus library to prepare for a request of 500 items by returning a variable of type **IEnumerable**. Since LINQ uses deferred execution by default, the items aren't actually created until the collection is iterated.
    
1. Add the following foreach block to iterate over the ``PurchaseFoodOrBeverage`` instances:

    ```csharp
    foreach(var interaction in foodInteractions)
    {
    }
    ```

1. Within the ``foreach`` block, add the following line of code to asynchronously create a container item and save the result of the creation task to a variable:

    ```csharp
    ItemResponse<PurchaseFoodOrBeverage> result = await customContainer.CreateItemAsync(interaction);
    ```

    > The ``CreateItemAsync`` method of the ``CosmosItems`` class takes in an object that you would like to serialize into JSON and store as a document within the specified container. The ``id`` property, which here we've assigned to a unique Guid on each object, is a special required value in Cosmos DB that is used for indexing and must be unique for every item in a container.

1. Still within the ``foreach`` block, add the following line of code to write the value of the newly created resource's ``id`` property to the console:

    ```csharp
    await Console.Out.WriteLineAsync($"Item Created\t{result.Resource.id}");
    ```

    > The ``CosmosItemResponse`` type has a property named ``Resource`` that contains the object representing the item as well as other properties to give you access to interesting data about an item such as its ETag.

1. Your **Main** method should look like this:

    ```csharp
    public static async Task Main(string[] args)
    {    
        using (CosmosClient client = new CosmosClient(_endpointUri, _primaryKey))
        {
            var targetDatabase = client.GetDatabase("EntertainmentDatabase");
            var customContainer = targetDatabase.GetContainer("CustomCollection");
            var foodInteractions = new Bogus.Faker<PurchaseFoodOrBeverage>()
                .RuleFor(i => i.id, (fake) => Guid.NewGuid().ToString())
                .RuleFor(i => i.type, (fake) => nameof(PurchaseFoodOrBeverage))
                .RuleFor(i => i.unitPrice, (fake) => Math.Round(fake.Random.Decimal(1.99m, 15.99m), 2))
                .RuleFor(i => i.quantity, (fake) => fake.Random.Number(1, 5))
                .RuleFor(i => i.totalPrice, (fake, user) => Math.Round(user.unitPrice * user.quantity, 2))
                .GenerateLazy(500);
            foreach(var interaction in foodInteractions)
            {
                ItemResponse<PurchaseFoodOrBeverage> result = await customContainer.CreateItemAsync(interaction);
                await Console.Out.WriteLineAsync($"Item Created\t{result.Resource.id}");
            }
        }     
    }
    ```

    > As a reminder, the Bogus library generates a set of test data. In this example, you are creating 500 items using the Bogus library and the rules listed above. The **GenerateLazy** method tells the Bogus library to prepare for a request of 500 items by returning a variable of type **IEnumerable**. Since LINQ uses deferred execution by default, the items aren't actually created until the collection is iterated. The **foreach** loop at the end of this code block iterates over the collection and creates items in Azure Cosmos DB.

1. Save all of your open editor tabs.

1. In the Visual Studio Code window, right-click the **Explorer** pane and select the **Open in Terminal** menu option.

1. In the open terminal pane, enter and execute the following command:

    ```sh
    dotnet run
    ```

    > This command will build and execute the console project.

1. Observe the output of the console application.

    > You should see a list of item ids associated with new items that are being created by this tool.

1. Click the **🗙** symbol to close the terminal pane.

### Populate Container with Data of Different Types

1. Locate the **Main** method and delete any existing code:

    ```csharp
    public static async Task Main(string[] args)
    {                           
    }
    ```

1. Replace the **Main** method with the following implementation:

    ```csharp
    public static async Task Main(string[] args)
    {  
        using (CosmosClient client = new CosmosClient(_endpointUri, _primaryKey))
        {
            var targetDatabase = client.GetDatabase("EntertainmentDatabase");
            var customContainer = targetDatabase.GetContainer("CustomCollection");
            var tvInteractions = new Bogus.Faker<WatchLiveTelevisionChannel>()
                .RuleFor(i => i.id, (fake) => Guid.NewGuid().ToString())
                .RuleFor(i => i.type, (fake) => nameof(WatchLiveTelevisionChannel))
                .RuleFor(i => i.minutesViewed, (fake) => fake.Random.Number(1, 45))
                .RuleFor(i => i.channelName, (fake) => fake.PickRandom(new List<string> { "NEWS-6", "DRAMA-15", "ACTION-12", "DOCUMENTARY-4", "SPORTS-8" }))
                .GenerateLazy(500);
            foreach(var interaction in tvInteractions)
            {
                ItemResponse<WatchLiveTelevisionChannel> result = await customContainer.CreateItemAsync(interaction);
                await Console.Out.WriteLineAsync($"Item Created\t{result.Resource.id}");
            }
        }
    }
    ```

    > As a reminder, the Bogus library generates a set of test data. In this example, you are creating 500 items using the Bogus library and the rules listed above. The **GenerateLazy** method tells the Bogus library to prepare for a request of 500 items by returning a variable of type **IEnumerable**. Since LINQ uses deferred execution by default, the items aren't actually created until the collection is iterated. The **foreach** loop at the end of this code block iterates over the collection and creates items in Azure Cosmos DB.

1. Save all of your open editor tabs.

1. In the Visual Studio Code window, right-click the **Explorer** pane and select the **Open in Terminal** menu option.

1. In the open terminal pane, enter and execute the following command:

    ```sh
    dotnet run
    ```

    > This command will build and execute the console project.

1. Observe the output of the console application.

    > You should see a list of item ids associated with new items that are being created.

1. Click the **🗙** symbol to close the terminal pane.

1. Locate the **Main** method and delete any existing code:

    ```csharp
    public static async Task Main(string[] args)
    {                            
    }
    ```

1. Replace the **Main** method with the following implementation:

    ```csharp
    public static async Task Main(string[] args)
    {  
        using (CosmosClient client = new CosmosClient(_endpointUri, _primaryKey))
        {
            var targetDatabase = client.GetDatabase("EntertainmentDatabase");
            var customContainer = targetDatabase.GetContainer("CustomCollection");
            var mapInteractions = new Bogus.Faker<ViewMap>()
                .RuleFor(i => i.id, (fake) => Guid.NewGuid().ToString())
                .RuleFor(i => i.type, (fake) => nameof(ViewMap))
                .RuleFor(i => i.minutesViewed, (fake) => fake.Random.Number(1, 45))
                .GenerateLazy(500);
            foreach(var interaction in mapInteractions)
            {
                ItemResponse<ViewMap> result = await customContainer.CreateItemAsync(interaction);
                await Console.Out.WriteLineAsync($"Document Created\t{result.Resource.id}");
            }
        }
    }
    ```

    > As a reminder, the Bogus library generates a set of test data. In this example, you are creating 500 items using the Bogus library and the rules listed above. The **GenerateLazy** method tells the Bogus library to prepare for a request of 500 items by returning a variable of type **IEnumerable**. Since LINQ uses deferred execution by default, the items aren't actually created until the collection is iterated. The **foreach** loop at the end of this code block iterates over the collection and creates items in Azure Cosmos DB.

1. Save all of your open editor tabs.

1. In the Visual Studio Code window, right-click the **Explorer** pane and select the **Open in Terminal** menu option.

1. In the open terminal pane, enter and execute the following command:

    ```sh
    dotnet run
    ```

    > This command will build and execute the console project.

1. Observe the output of the console application.

    > You should see a list of item ids associated with new items that are being created.

1. Click the **🗙** symbol to close the terminal pane.

1. Close all open editor tabs.

1. Close the Visual Studio Code application.

> If this is your final lab, follow the steps in [Removing Lab Assets](11-cleaning_up.md) to remove all lab resources. 


# Load Data Into Cosmos DB with ADF

In this lab, you will populate an Azure Cosmos DB container from an existing set of data using tools built in to Azure. After importing, you will use the Azure portal to view your imported data.

> Before you start this lab, you will need to create an Azure Cosmos DB database and container that you will use throughout the lab. You will also use the **Azure Data Factory (ADF)** to import existing data into your container.

## Create Azure Cosmos DB Database and Container

*You will now create a database and container within your Azure Cosmos DB account.*

1. On the left side of the portal, click the **Resource groups** link.

    ![Resource groups](../media/03-resource_groups.jpg)

1. In the **Resource groups** blade, locate and select the **cosmoslabs** *Resource Group*.

    ![Lab resource group](../media/03-lab_resource_group.jpg)

1. In the **cosmoslabs** blade, select the **Azure Cosmos DB** account you recently created.

    ![Cosmos resource](../media/03-cosmos_resource.jpg)

1. In the **Azure Cosmos DB** blade, locate and click the **Overview** link on the left side of the blade. At the top click the **Add Container** button.

    ![Add container](../media/03-add_collection.jpg)

1. In the **Add Container** popup, perform the following actions:

    1. In the **Database id** field, select the **Create new** option and enter the value **ImportDatabase**.

    1. Do not check the **Provision database throughput** option.

        > Provisioning throughput for a database allows you to share the throughput among all the containers that belong to that database. Within an Azure Cosmos DB database, you can have a set of containers which shares the throughput as well as containers, which have dedicated throughput.

    1. In the **Container Id** field, enter the value **FoodCollection**.

    1. In the **Partition key** field, enter the value ``/foodGroup``.

    1. In the **Throughput** field, enter the value ``11000``.

    1. Click the **OK** button.

1. Wait for the creation of the new **database** and **container** to finish before moving on with this lab.

## Import Lab Data Into Container

You will use **Azure Data Factory (ADF)** to import the JSON array stored in the **nutrition.json** file from Azure Blob Storage. If you are completing the lab through Microsoft Hands-on Labs, you can use the pre-created Data Factory within your resource group. You do not need to do Steps 1-4 in this section and can proceed to Step 5 by opening your Data Factory (named importNutritionData with a random number suffix).

1. On the left side of the portal, click the **Resource groups** link.

    > To learn more about copying data to Cosmos DB with ADF, please read [ADF's documentation](https://docs.microsoft.com/en-us/azure/data-factory/connector-azure-cosmos-db). 

    ![Resource groups](../media/03-resource_groups.jpg)

1. In the **Resource groups** blade, locate and select the **cosmoslabs** *Resource Group*.

1. Click **Add** to add a new resource

    ![Add adf](../media/03-add_adf.jpg)

1. Search for **Data Factory** and select it. Create a new **Data Factory**. You should name this data factory **importnutritiondata** with a unique number appended and select the relevant Azure subscription. You should ensure your existing **cosmoslabs** resource group is selected as well as a Version **V2**. Select **East US** as the region. Do not select **Enable GIT** (this may be checked by default). Click **create**.

    ![df](../media/03-adf_selections.jpg)

1. After creation, open your newly created Data Factory. Select **Author & Monitor** and you will launch ADF. You should see a screen similar to the screenshot below. Select **Copy Data**. We will be using ADF for a one-time copy of data from a source JSON file on Azure Blob Storage to a database in Cosmos DB’s SQL API. ADF can also be used for more frequent data transfers from Cosmos DB to other data stores.
    ![](../media/03-adf_author&monitor.jpg)
    ![](../media/03-adf_copydata.jpg)

1. Edit basic properties for this data copy. You should name the task **ImportNutrition** and select to **Run once now**. Do not select **enable git**.

   ![adf-properties](../media/03-adf_properties.jpg)

1. **Create a new connection** and select **Azure Blob Storage**. We will import data from a json file on Azure Blob Storage. In addition to Blob Storage, you can use ADF to migrate from a wide variety of sources. We will not cover migration from these sources in this tutorial.

    ![](../media/03-adf_blob.jpg)

    ![](../media/03-adf_blob2.jpg)

1. Name the source **NutritionJson** and select **SAS URI** as the Authentication method. Please use the following SAS URI for read-only access to this Blob Storage container: 

    `https://cosmosdblabsv3.blob.core.windows.net/?sv=2018-03-28&ss=bfqt&srt=sco&sp=rlp&se=2022-01-01T04:55:28Z&st=2019-08-05T20:02:28Z&spr=https&sig=%2FVbismlTQ7INplqo6WfU8o266le72o2bFdZt1Y51PZo%3D`

    ![](../media/03-adf_connecttoblob.jpg)

1. Click **Next** and then **Browse** to select the **nutritiondata** folder. Then select **NutritionData.json**.

    ![](../media/03-adf_choosestudents.jpg)

1. Do not check **Copy file recursively** or **Binary Copy**. Also ensure that other fields are empty.

    ![](../media/03-adf_source_next.jpg)

1. Select the file format as **JSON format**. Then select **Next**.

    ![](../media/03-adf_source_dataset_format.jpg)

1. You have now successfully connected the Blob Storage container with the nutrition.json file as the source.

1. For the **Destination data store** add the Cosmos DB target data store by selecting **Create new connection** and selecting **Azure Cosmos DB (SQL API)**.

    ![](../media/03-adf_selecttarget.jpg)

1. Name the linked service **targetcosmosdb** and select your Azure subscription and Cosmos DB account. You should also select the Cosmos DB **ImportDatabase** that you created earlier.

    ![](../media/03-adf_selecttargetdb.jpg)

1. Select your newly created **targetcosmosdb** connection as the Destination date store.

    ![](../media/03-adf_destconnectionnext.jpg)

1. Select your **FoodCollection** container from the drop-down menu. You will map your Blob storage file to the correct Cosmos DB container. Click **Next** to continue.

    ![](../media/03-adf_correcttable.jpg)

1. Click through this screen.

    ![](../media/03-adf_destinationconnectionfinal.jpg)

1. There is no need to change any settings. Click **next**.

    ![](../media/03-adf_settings.jpg)

1. Click **Next** to begin deployment After deployment is complete, select **Monitor**.

    ![](../media/03-adf_deployment.jpg)

1. After a few minutes, refresh the page and the status for the ImportNutrition pipeline should be listed as **Succeeded**.

1. Once the import process has completed, close the ADF. You will now proceed to validate your imported data. 

## Validate Imported Data

*The Azure Cosmos DB Data Explorer allows you to view documents and run queries directly within the Azure Portal. In this exercise, you will use the Data Explorer to view the data stored in our container.*

*You will validate that the data was successfully imported into your container using the **Items** view in the **Data Explorer**.*

1. Return to the **Azure Portal** (<http://portal.azure.com>).

1. On the left side of the portal, click the **Resource groups** link.

    ![Resource groups](../media/03-resource_groups.jpg)

1. In the **Resource groups** blade, locate and select the **cosmoslabs** *Resource Group*.

    ![Lab resource group](../media/03-lab_resource_group.jpg)

1. In the **cosmoslabs** blade, select the **Azure Cosmos DB** account you recently created.

    ![Cosmos resource](../media/03-cosmos_resource.jpg)

1. In the **Azure Cosmos DB** blade, locate and click the **Data Explorer** link on the left side of the blade.

    ![Data Explorer pane](../media/03-data_explorer_pane.jpg)

1. In the **Data Explorer** section, expand the **ImportDatabase** database node and then expand the **FoodCollection** container node. 

    ![Container node](../media/03-collection_node.jpg)

1. Within the **FoodCollection** node, click the **Items** link to view a subset of the various documents in the container. Select a few of the documents and observe the properties and structure of the documents.

    ![Documents](../media/03-documents.jpg)

    ![Example document](../media/03-example_document.jpg)



# Querying in Azure Cosmos DB

Azure Cosmos DB SQL API accounts provide support for querying items using the Structured Query Language (SQL), one of the most familiar and popular query languages, as a JSON query language. In this lab, you will explore how to use these rich query capabilities directly through the Azure Portal. No separate tools or client side code are required.

If this is your first lab and you have not already completed the setup for the lab content see the instructions for [Account Setup](00-account_setup.md) before starting this lab.

## Query Overview

Querying JSON with SQL allows Azure Cosmos DB to combine the advantages of a legacy relational databases with a NoSQL database. You can use many rich query capabilities such as subqueries or aggregation functions but still retain the many advantages of modeling data in a NoSQL database.

Azure Cosmos DB supports strict JSON items only. The type system and expressions are restricted to deal only with JSON types. For more information, see the [JSON specification](https://www.json.org/).

## Running your first query

In this lab section, you will query your **FoodCollection**. If you prefer, you can also complete all lab steps using the [Azure Cosmos DB Query Playground](https://www.documentdb.com/sql/demo).

You will begin by running basic queries with `SELECT`, `WHERE`, and `FROM` clauses.

### Open Data Explorer

1. In the **Azure Cosmos DB** blade, locate and click the **Data Explorer** link on the left side of the blade.
2. In the **Data Explorer** section, expand the **NutritionDatabase** database node and then expand the **FoodCollection** container node.
3. Within the **FoodCollection** node, click the **Items** link.
4. View the items within the container. Observe how these documents have many properties, including arrays.
5. Click **New SQL Query**. Paste the following SQL query and select **Execute Query**.

```sql
SELECT *
FROM food
WHERE food.foodGroup = "Snacks" and food.id = "19015"
```

6. You will see that the query returned the single document where id is "19015" and the foodGroup is "Snacks".
7. Explore the structure of this item as it is representative of the items within the **FoodCollection** container that we will be working with for the remainder of this section.

## Dot and quoted property projection accessors

You can choose which properties of the document to project into the result using the dot notation. If you wanted to return only the item's id you could run the query below:
by clicking the **New SQL Query**. Paste the following SQL query and selecting **Execute Query**.

```sql
SELECT food.id
FROM food
WHERE food.foodGroup = "Snacks" and food.id = "19015"
```

Though less common, you can also access properties using the quoted property operator [""]. For example, SELECT food.id and SELECT food["id"] are equivalent. This syntax is useful to escape a property that contains spaces, special characters, or has the same name as a SQL keyword or reserved word.

```sql
SELECT food["id"]
FROM food
WHERE food["foodGroup"] = "Snacks" and food["id"] = "19015"
```

## WHERE clauses

Let’s explore WHERE clauses. You can add complex scalar expressions including arithmetic, comparison and logical operators in the WHERE clause.

1. Run the below query by clicking the **New SQL Query**. Paste the following SQL query and then click **Execute Query**.

```sql
SELECT food.id,
food.description,
food.tags,
food.foodGroup,
food.version
FROM food
WHERE (food.manufacturerName = "The Coca-Cola Company" AND food.version > 0)
```

This query will return the id, description, servings, tags, foodGroup, manufacturerName and version for items with "The Coca-Cola Company" for manufacturerName and a version greater than 0.

Your first result document should be:

```json
{
  "id": "14026",
  "description": "Beverages, Energy Drink, sugar-free with guarana",
  "tags": [
    {
      "name": "beverages"
    },
    {
      "name": "energy drink"
    },
    {
      "name": "sugar-free with guarana"
    }
  ],
  "foodGroup": "Beverages",
  "manufacturerName": "The Coca-Cola Company",
  "version": 1
}
```

You should note that where the query returned the results of tags node it projected the entire contents of the property which in this case is an array.

## Advanced projection

Azure Cosmos DB supports several forms of transformation on the resultant JSON. One of the simplest is to alias your JSON elements using the AS aliasing keyword as you project your results.

By running the query below you will see that the element names are transformed. In addition, the projection is accessing only the first element in the servings array for all items specified by the WHERE clause.

```sql
SELECT food.description,
food.foodGroup,
food.servings[0].description AS servingDescription,
food.servings[0].weightInGrams AS servingWeight
FROM food
WHERE food.foodGroup = "Fruits and Fruit Juices"
AND food.servings[0].description = "cup"
```

## ORDER BY clause

Azure Cosmos DB supports adding an ORDER BY clause to sort results based on one or more properties

```sql
SELECT food.description, 
food.foodGroup, 
food.servings[0].description AS servingDescription,
food.servings[0].weightInGrams AS servingWeight
FROM food
WHERE food.foodGroup = "Fruits and Fruit Juices" AND food.servings[0].description = "cup"
ORDER BY food.servings[0].weightInGrams DESC
```

You can learn more about configuring the required indexes for an Order By clause in the later Indexing Lab or by reading [our docs](
https://docs.microsoft.com/en-us/azure/cosmos-db/sql-query-order-by).

## Limiting query result size

Azure Cosmos DB supports the TOP keyword. TOP can be used to limit the number of returning values from a query.
Run the query below to see the top 20 results.

```sql
SELECT TOP 20 food.id,
food.description,
food.tags,
food.foodGroup
FROM food
WHERE food.foodGroup = "Snacks"
```

The OFFSET LIMIT clause is an optional clause to skip then take some number of values from the query. The OFFSET count and the LIMIT count are required in the OFFSET LIMIT clause.

```sql
SELECT food.id,
food.description,
food.tags,
food.foodGroup
FROM food
WHERE food.foodGroup = "Snacks"
ORDER BY food.id
OFFSET 10 LIMIT 10
```

When OFFSET LIMIT is used in conjunction with an ORDER BY clause, the result set is produced by doing skip and take on the ordered values. If no ORDER BY clause is used, it will result in a deterministic order of values.

## More advanced filtering

Let’s add the IN and BETWEEN keywords into our queries. IN can be used to check whether a specified value matches any element in a given list and BETWEEN can be used to run queries against a range of values. Run some sample queries below:

```sql
SELECT food.id,
       food.description,
       food.tags,
       food.foodGroup,
       food.version
FROM food
WHERE food.foodGroup IN ("Poultry Products", "Sausages and Luncheon Meats")
    AND (food.id BETWEEN "05740" AND "07050")
```

## More advanced projection

Azure Cosmos DB supports JSON projection within its queries. Let’s project a new JSON Object with modified property names. Run the query below to see the results.

```sql
SELECT { 
"Company": food.manufacturerName,
"Brand": food.commonName,
"Serving Description": food.servings[0].description,
"Serving in Grams": food.servings[0].weightInGrams,
"Food Group": food.foodGroup 
} AS Food
FROM food
WHERE food.id = "21421"
```

## JOIN within your documents

Azure Cosmos DB’s JOIN supports intra-document and self-joins. Azure Cosmos DB does not support JOINs across documents or containers.

In an earlier query example we returned a result with attributes of just the first serving of the food.servings array. By using the join syntax below, we can now return an item in the result for every item within the serving array while still being able to project the attributes from elsewhere in the item.

Run the query below to iterate on the food document’s servings.

```sql
SELECT
food.id as FoodID,
serving.description as ServingDescription
FROM food
JOIN serving IN food.servings
WHERE food.id = "03226"
```

JOINs are useful if you need to filter on properties within an array. Run the below example that has filter after the intra-document JOIN.

```sql
SELECT VALUE COUNT(1)
FROM c
JOIN t IN c.tags
JOIN s IN c.servings
WHERE t.name = 'infant formula' AND s.amount > 1
```

## System functions

Azure Cosmos DB supports a number of built-in functions for common operations. They cover mathematical functions like ABS, FLOOR and ROUND and type checking functions like IS_ARRAY, IS_BOOL and IS_DEFINED. [Learn more about supported system functions](https://docs.microsoft.com/en-us/azure/cosmos-db/sql-query-system-functions).

Run the query below to see example use of some system functions

```sql
SELECT food.id,
food.commonName,
food.foodGroup,
ROUND(nutrient.nutritionValue) AS amount,
nutrient.units
FROM food JOIN nutrient IN food.nutrients
WHERE IS_DEFINED(food.commonName)
AND nutrient.description = "Water"
AND food.foodGroup IN ("Sausages and Luncheon Meats", "Legumes and Legume Products")
AND food.id > "42178"
```

## Correlated subqueries

In many scenarios, a subquery may be effective. A correlated subquery is a query that references values from an outer query. We will walk through some of the most useful examples here. You can [learn more about subqueries](https://docs.microsoft.com/en-us/azure/cosmos-db/sql-query-subquery).

There are two types of subqueries: Multi-value subqueries and scalar subqueries. Multi-value subqueries return a set of documents and are always used within the FROM clause. A scalar subquery expression is a subquery that evaluates to a single value.

### Multi-value subqueries

You can optimize JOIN expressions with a subquery.

Consider the following query which performs a self-join and then applies a filter on name, nutritionValue, and amount. We can use a subquery to filter out the joined array items before joining with the next expression.

```sql
SELECT VALUE COUNT(1)
FROM c
JOIN t IN c.tags
JOIN n IN c.nutrients
JOIN s IN c.servings
WHERE t.name = 'infant formula' AND (n.nutritionValue > 0 
AND n.nutritionValue < 10) AND s.amount > 1
```

We could rewrite this query using three subqueries to optimize and reduce the Request Unit (RU) charge. Observe that the multi-value subquery always appears in the FROM clause of the outer query.

```sql
SELECT VALUE COUNT(1)
FROM c
JOIN (SELECT VALUE t FROM t IN c.tags WHERE t.name = 'infant formula')
JOIN (SELECT VALUE n FROM n IN c.nutrients WHERE n.nutritionValue > 0 AND n.nutritionValue < 10)
JOIN (SELECT VALUE s FROM s IN c.servings WHERE s.amount > 1)
```

### Scalar subqueries

One use case of scalar subqueries is rewriting ARRAY_CONTAINS as EXISTS.

Consider the following query that uses ARRAY_CONTAINS:

```sql
SELECT TOP 5 f.id, f.tags
FROM food f
WHERE ARRAY_CONTAINS(f.tags, {name: 'orange'})
```

Run the following query which has the same results but uses EXISTS:

```sql
SELECT TOP 5 f.id, f.tags
FROM food f
WHERE EXISTS(SELECT VALUE t FROM t IN f.tags WHERE t.name = 'orange')
```

The major advantage of using EXISTS is the ability to have complex filters in the EXISTS function, rather than just the simple equality filters which ARRAY_CONTAINS permits. Here is an example:

```sql
SELECT VALUE c.description
FROM c
JOIN n IN c.nutrients
WHERE n.units= "mg" AND n.nutritionValue > 0
```

> If this is your final lab, follow the steps in [Removing Lab Assets](11-cleaning_up.md) to remove all lab resources. 




# Indexing in Azure Cosmos DB

In this lab, you will modify the indexing policy of an Azure Cosmos DB container. You will explore how you can optimize indexing policy for write or read heavy workloads as well as understand the indexing requirements for different SQL API query features.

> If this is your first lab and you have not already completed the setup for the lab content see the instructions for [Account Setup](00-account_setup.md) before starting this lab.

## Indexing Overview

Azure Cosmos DB is a schema-agnostic database that allows you to iterate on your application without having to deal with schema or index management. By default, Azure Cosmos DB automatically indexes every property for all items in your container without the need to define any schema or configure secondary indexes. If you chose to leave indexing policy at the default settings, you can run most queries with optimal performance and never have to explicitly consider indexing. However, if you want control over adding or removing properties from the index, modification is possible through the Azure Portal or any SQL API SDK.

Azure Cosmos DB uses an inverted index, representing your data in a tree form. For a brief introduction on how this works, read our [indexing overview](https://docs.microsoft.com/en-us/azure/cosmos-db/index-overview) before continuing with the lab.

## Customizing the indexing policy

In this lab section, you will view and modify the indexing policy for your **FoodCollection**.

### Open Data Explorer

1. On the left side of the portal, click the **Resource groups** link.

2. In the **Resource groups** blade, locate and select the **cosmoslabs** *Resource Group*.

3. In the **cosmoslabs** blade, select your **Azure Cosmos DB** account.

4. In the **Azure Cosmos DB** blade, locate and click the **Data Explorer** link on the left side of the blade.

5. In the **Data Explorer** section, expand the **NutritionDatabase** database node and then expand the **FoodCollection** container node.

6. Within the **FoodCollection** node, click the **Items** link.

7. View the items within the container. Observe how these documents have many properties, including arrays. If we do not use a particular property in the WHERE clause, ORDER BY clause, or a JOIN, indexing the property does not provide any performance benefit.

8. Still within the **FoodCollection** node, click the **Scale & Settings** link. In the **Indexing Policy** section, you can edit the JSON file that defines your container's index. Indexing policy can also be modified through any Azure Cosmos DB SDK, but during this lab we will modify the indexing policy through the Azure Portal.

   ![indexingpolicy-initial](../media/04-indexingpolicy-initial.jpg)

### Including and excluding Range Indexes

Instead of including a range index on every property by default, you can chose to either include or exclude specific paths from the index. Let's go through some simple examples (no need to enter these into the Azure Portal, we can just review them here).

Within the **FoodCollection**, documents have this schema (some properties were removed for simplicity):

```json
{
    "id": "36000",
    "_rid": "LYwNAKzLG9ADAAAAAAAAAA==",
    "_self": "dbs/LYwNAA==/colls/LYwNAKzLG9A=/docs/LYwNAKzLG9ADAAAAAAAAAA==/",
    "_etag": "\"0b008d85-0000-0700-0000-5d1a47e60000\"",
    "description": "APPLEBEE'S, 9 oz house sirloin steak",
    "tags": [
        {
            "name": "applebee's"
        },
        {
            "name": "9 oz house sirloin steak"
        }
    ],

    "manufacturerName": "Applebee's",
    "foodGroup": "Restaurant Foods",
    "nutrients": [
        {
            "id": "301",
            "description": "Calcium, Ca",
            "nutritionValue": 16,
            "units": "mg"
        },
        {
            "id": "312",
            "description": "Copper, Cu",
            "nutritionValue": 0.076,
            "units": "mg"
        },
    ]
}
```

If you wanted to only index the manufacturerName, foodGroup, and nutrients array with a range index, you should define the following index policy:

```json
{
        "indexingMode": "consistent",
        "includedPaths": [
            {
                "path": "/manufacturerName/*"
            },
            {
                "path": "/foodGroup/*"
            },
            {
                "path": "/nutrients/[]/*"
            }
        ],
        "excludedPaths": [
            {
                "path": "/*"
            }
        ]
    }
```

In this example, we use the wildcard character '*' to indicate that we would like to index all paths within the nutrients array. However, it's possible we may just want to index the nutritionValue of each array element.

In this next example, the indexing policy would explicitly specify that the nutritionValue path in the nutrition array should be indexed. Since we don't use the wildcard character '*', no additional paths in the array are indexed.

```json
{
        "indexingMode": "consistent",
        "includedPaths": [
            {
                "path": "/manufacturerName/*"
            },
            {
                "path": "/foodGroup/*"
            },
            {
                "path": "/nutrients/[]/nutritionValue/*"
            }
        ],
        "excludedPaths": [
            {
                "path": "/*"
            }
        ]
    }
```

Finally, it's important to understand the difference between the `*` and `?` characters. The `*` character indicates that Azure Cosmos DB should index every path beyond that specific node. The `?` character indicates that Azure Cosmos DB should index no further paths beyond this node. In the above example, there are no additional paths under nutritionValue. If we were to modify the document and add a path here, having the wildcard character '*'  in the above example would ensure that the property is indexed without explicitly mentioning the name.

### Understand query requirements

Before modifying indexing policy, it's important to understand how the data is used the collection. If your workload is write-heavy or your documents are large, you should only index necessary paths. This will significantly decrease the amount of RU's required for inserts, updates, and deletes.

Let's imagine that the following queries are the only read operations that are executed on the **FoodCollection** container.

**Query #1**

```sql
SELECT * FROM c WHERE c.manufacturerName = <manufacturerName>
```

**Query #2**

```sql
SELECT * FROM c WHERE c.foodGroup = <foodGroup>
```

These queries only require that a range index be defined on **manufacturerName** and **foodGroup**, respectively. We can modify the indexing policy to index only these properties.

### Edit the indexing policy by including paths

1. Navigate back to the **FoodCollection** in the Azure Portal and click the **Scale & Settings** link. In the **Indexing Policy** section, replace the existing json file with the following:

```json
{
        "indexingMode": "consistent",
        "includedPaths": [
            {
                "path": "/manufacturerName/*"
            },
            {
                "path": "/foodGroup/*"
            }
        ],
        "excludedPaths": [
            {
                "path": "/*"
            }
        ]
    }
```

This new indexing policy will create a range index on only the manufacturerName and foodGroup properties. It will remove range indexes on all other properties. Click **Save**. Azure Cosmos DB will update the index in the container, using your excess provisioned throughput to make the updates.

During the container re-indexing, write performance is unaffected. However, queries may return incomplete results.

1. After defining the new indexing policy, navigate to your **FoodCollection** and select the **Add New SQL Query** icon. Paste the following SQL query and select **Execute Query**:

```sql
SELECT * FROM c WHERE c.manufacturerName = "Kellogg, Co."
```

Navigate to the **Query Stats** tab. You should observe that this query still has a low RU charge, even after removing some properties from the index. Because the **manufacturerName** was the only property used as a filter in the query, it was the only index that was required.

Now, replace the query text with the following and select **Execute Query**:

```sql
SELECT * FROM c WHERE c.description = "Bread, blue corn, somiviki (Hopi)"
```

You should observe that this query has a very high RU charge even though only a single document is returned. This is because no range index is currently defined for the `description` property.

Also observe the **Query Metrics** below:

![query-metrics](../media/04-querymetrics.JPG)

If a query does not use the index, the **Index hit document count** will be 0. We can see above that the query needed to retrieve 5,187 documents and ultimately ended up only returning 1 document.

### Edit the indexing policy by excluding paths

In addition to manually including certain paths to be indexed, you can exclude specific paths. In many cases, this approach can be simpler since it will allow all new properties in your document to be indexed by default. If there is a property that you are certain you will never use in your queries, you should explicitly exclude this path.

We will create an indexing policy to index every path except for the **description** property.

1. Navigate back to the **FoodCollection** in the Azure Portal and click the **Scale & Settings** link. In the **Indexing Policy** section, replace the existing json file with the following:

```json
{
        "indexingMode": "consistent",
        "includedPaths": [
            {
                "path": "/*"
            }
        ],
        "excludedPaths": [
            {
                "path": "/description/*"
            }
        ]
    }
```

This new indexing policy will create a range index on every property except for the description. Click **Save**. Azure Cosmos DB will update the index in the container, using your excess provisioned throughput to make the updates.

During the container re-indexing, write performance is unaffected. However, queries may return incomplete results.

1. After defining the new indexing policy, navigate to your **FoodCollection** and select the **Add New SQL Query** icon. Paste the following SQL query and select **Execute Query**:

```sql
SELECT * FROM c WHERE c.manufacturerName = "Kellogg, Co."
```

Navigate to the **Query Stats** tab. You should observe that this query still has a low RU charge since manufacturerName is indexed.

Now, replace the query text with the following and select **Execute Query**:

```sql
SELECT * FROM c WHERE c.description = "Bread, blue corn, somiviki (Hopi)"
```

You should observe that this query has a very high RU charge even though only a single document is returned. This is because the `description` property is explicitly excluded in the indexing policy.

## Adding a Composite Index

For ORDER BY queries that order by multiple properties, a composite index is required. A composite index is defined on multiple properties and must be manually created.

1. In the **Azure Cosmos DB** blade, locate and click the **Data Explorer** link on the left side of the blade.
2. In the **Data Explorer** section, expand the **NutritionDatabase** database node and then expand the **FoodCollection** container node.
3. Select the icon to add a **New SQL Query**. Paste the following SQL query and select **Execute Query**

```sql
    SELECT * FROM c ORDER BY c.foodGroup ASC, c.manufacturerName ASC
```

This query will fail with the following error:

```
"The order by query does not have a corresponding composite index that it can be served from."
```

In order to run a query that has an ORDER BY clause with one property, the default range index is sufficient. Queries with multiple properties in the ORDER BY clause require a composite index.

4. Still within the **FoodCollection** node, click the **Scale & Settings** link. In the **Indexing Policy** section, you will add a composite index.

Replace the **Indexing Policy** with the following text:

```json
{
    "indexingMode": "consistent",
    "automatic": true,
    "includedPaths": [
        {
            "path": "/manufacturerName/*"
        },
        {
            "path": "/foodGroup/*"
        }
    ],
    "excludedPaths": [
        {
            "path": "/*"
        },
        {
            "path": "/\"_etag\"/?"
        }
    ],
    "compositeIndexes": [
        [
            {
                "path": "/foodGroup",
                "order": "ascending"
            },
            {
                "path": "/manufacturerName",
                "order": "ascending"
            }
        ]
    ]
}
```

5. **Save** this new indexing policy. The update should take approximately 10-15 seconds to apply to your container.

This indexing policy defines a composite index that allows for the following ORDER BY queries. Test each of these by running them in your existing open query tab in the **Data Explorer**. When you define the order for properties in a composite index, they must either exactly match the order in the ORDER BY clause or be, in all cases, the opposite value.

```sql
    SELECT * FROM c ORDER BY c.foodGroup ASC, c.manufacturerName ASC
    SELECT * FROM c ORDER BY c.foodGroup DESC, c.manufacturerName DESC
```

Now, try to run the following query, which the current composite index does not support.

```sql
    SELECT * FROM c ORDER BY c.foodGroup DESC, c.manufacturerName ASC
```

This query will not run without an additional composite index. You can modify the indexing policy to include an additional composite index.

```json
{
    "indexingMode": "consistent",
    "automatic": true,
    "includedPaths": [
        {
            "path": "/manufacturerName/*"
        },
        {
            "path": "/foodGroup/*"
        }
    ],
    "excludedPaths": [
        {
            "path": "/*"
        },
        {
            "path": "/\"_etag\"/?"
        }
    ],
    "compositeIndexes": [
        [
            {
                "path": "/foodGroup",
                "order": "ascending"
            },
            {
                "path": "/manufacturerName",
                "order": "ascending"
            }
        ],
        [
            {
                "path": "/foodGroup",
                "order": "descending"
            },
            {
                "path": "/manufacturerName",
                "order": "ascending"
            }
        ]
    ]
}
```

You should now be able to run the query. After completing the lab, you can [learn more about defining composite indexes](https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-manage-indexing-policy#composite-indexing-policy-examples).

## Adding a spatial index

### Create a new container with volcano data

First, you will create a new Cosmos container named volcanoes inside a new database. Azure Cosmos DB supports querying of data in the GeoJSON format. During this lab, you will upload sample data to this container that is specified in this format. This volcano.json sample data is a better fit for geo-spatial queries than our existing nutrition dataset. The dataset contains the coordinates and basic information for many volcanoes around the world.

For this lab, we will only need to upload a few sample documents.

1. In the **Azure Cosmos DB** blade, locate and click the **Data Explorer** link on the left side of the blade.

2. Select the icon to add a **New Container**

3. In the **Add Container** popup, perform the following actions:

   1. In the **Database id** field, select the **Create new** option and enter the value **VolcanoDatabase**.

   2. Ensure the **Provision database throughput** option is not selected.

      > Provisioning throughput for a database allows you to share the throughput among all the containers that belong to that database. Within an Azure Cosmos DB database, you can have a set of containers which shares the throughput as well as containers, which have dedicated throughput.

   3. In the **Container Id** field, enter the value **VolcanoContainer**.

   4. In the **Partition key** field, enter the value ``/Country``.

   5. In the **Throughput** field, enter the value ``5000``.

   6. Click the **OK** button.

### Upload Sample Data

When you upload sample data, Azure Cosmos DB will automatically create a geo-spatial index for any GeoJSON data with the types "Point", "Polygon", or "LineString".

1. Navigate back to the **VolcanoesContainer** in the Azure Portal and click the **Items** section.
2. Select **Upload Item**
3. In the popup, navigate to the volcano.json file. This file is available [here](../setup/VolcanoData.json). If you followed the prelab steps, you already downloaded this file in your **setup** folder.

### Create geo-spatial indexes in the **Volcanoes** container

1. Navigate back to the **VolcanoesContainer** in the Azure Portal and click the **Scale & Settings** link. In the **Indexing Policy** section, replace the existing json file with the following:

```json
{
    "indexingMode": "consistent",
    "automatic": true,
    "includedPaths": [
        {
            "path": "/*"
        }
    ],
    "excludedPaths": [
        {
            "path": "/\"_etag\"/?"
        }
    ],
    "spatialIndexes": [
        {
            "path": "/*",
            "types": [
                "Point",
                "Polygon",
                "MultiPolygon",
                "LineString"
            ]
        }
    ]
}
```

Geo-spatial indexing is by default, disabled. This indexing policy will turn on geo-spatial indexing for all possible GeoJSON types which include Points, Polygons, MultiPolygon, and LineStrings. Similar to range indexes and composite indexes, there are no precision settings for geo-spatial indexes.

[Learn more about querying geo-spatial data in Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/geospatial#introduction-to-spatial-data).

### Query the Volcano Data

1. Navigate back to the **VolcanoesContainer** in the Azure Portal and click the **New SQL Query**. Paste the following SQL query and select **Execute Query**.

```sql
SELECT *
FROM volcanoes v
WHERE ST_DISTANCE(v.Location, {
"type": "Point",
"coordinates": [-122.19, 47.36]
}) < 100 * 1000
AND v.Type = "Stratovolcano"
AND v["Last Known Eruption"] = "Last known eruption from 1800-1899, inclusive"
```

Observe the **Query Stats** for this operation. Because the container has a geo-spatial index for Points, this query consumed a small amount of RU's.

> This query returns all the Stratovolcanoes that last erupted between 1800 and 1899 that are within 100 km of the coordinates (122.19, 47.36). These are the coordinates of Redmond, WA.

### Query sample polygon data

If you specify points within a Polygon in a counter-clockwise order, you will define the area within the coordinates as the polygon area. A Polygon specified in clockwise order represents the inverse of the region within it.

We can explore this concept through sample queries.

1. Navigate back to the **VolcanoesContainer** in the Azure Portal and click the **New SQL Query**. Paste the following SQL query and select **Execute Query**.

```sql
SELECT *
FROM volcanoes v
WHERE ST_WITHIN(v.Location, {
    "type":"Polygon",
    "coordinates":[[
        [-123.8, 48.8],
        [-123.8, 44.8],
        [-119.8, 44.8],
        [-119.8, 48.8],
        [-123.8, 48.8]
    ]]
    })
```

In this case, there are 8 volcanoes located within this rectangle.

2. In the *Query Editor* replace the text with the following query:

```sql
SELECT *
FROM volcanoes v
WHERE ST_WITHIN(v.Location, {
    "type":"Polygon",
    "coordinates":[[
        [-123.8, 48.8],
        [-119.8, 48.8],
        [-119.8, 44.8],
        [-123.8, 44.8],
        [-123.8, 48.8]
    ]]
    })
```

You should now see many items returned. There are thousands of volcanoes located outside our small rectangle region.

When creating a GeoJSON polygon, whether it be inside a query or item, the order of the coordinates specified matters. Azure Cosmos DB will not reject coordinates that indicate the inverse of a polygon's shape. In addition, GeoJSON requires that you specify coordinates in the format: (latitude, longitude).

## Lab Cleanup

### Restoring the **FoodCollection** Indexing Policy

You should restore the **FoodCollection** indexing policy to the default setting where all paths are indexed. 

1. In the **Azure Cosmos DB** blade, locate and click the **Data Explorer** link on the left side of the blade.
2. In the **Data Explorer** section, expand the **NutritionDatabase** database node and then expand the **FoodCollection** container node.
3. Within the **FoodCollection** node, click the **Scale & Settings** link. In the **Indexing Policy** section, replace the existing JSON file with the following:

```json
{
    "indexingMode": "consistent",
    "automatic": true,
    "includedPaths": [
        {
            "path": "/*"
        }
    ],
    "excludedPaths": [
        {
            "path": "/\"_etag\"/?"
        }
    ]
}
```

1. Select **Save** to apply these changes. This Indexing Policy is the same Indexing Policy as when we began the lab. It is required for subsequent labs.

### Delete the **VolcanoContainer**

You will not need the **VolcanoContainer** during additional lab sections. You should delete this container now.

1. Navigate to the **Data Explorer**
2. Select the three dots near your **VolcanoContainer**. From the menu, select **Delete Container**. 
3. Confirm the container's name and delete the container.
4. Close your browser window. You have now completed the indexing lab section.

> If this is your final lab, follow the steps in [Removing Lab Assets](11-cleaning_up.md) to remove all lab resources. 