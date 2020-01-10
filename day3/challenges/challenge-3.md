# Azure Search #

## Here is what you will learn ##

- Create an Azure Search Service
- Create an Index
- Add Cognitive Skills to Azure Search
- Use Monitoring

## Create an Azure Search Service ##

Create an Azure Search Service:

- Create a resource group
  - westeurope
- Add Azure Search
- TODO: Martha Doku
- Hit "Create"

##  Create an Index ##

- TODO: Martha Doku


## Add Cognitive Skills to Azure Search##

- TODO: Martha Doku


## Use Monitoring ##

- TODO: Martha Doku


# Hints for Challenge 1

## Indexing PDF data

First, create a `Azure Search` instance in the Azure Portal:

![alt text](../images/azure_search_service.png "Azure Search")

For our purposes, the `Free Tier` is sufficient:

![alt text](../images/azure_search_sizes.png "Azure Search Sizes")

However, the `Free Tier` does not support additional replicas, scaling and is only able to index documents with up to 32000 characters/document. If we want to index longer documents, we need to go to a bigger tier (64000 for `Basic`, 4m for `Standard` and above - as of November 2018).

Once provisioned, our service will be reachable via `https://xxxxxxx.search.windows.net`

Azure Search [can index](https://docs.microsoft.com/en-us/azure/search/search-indexer-overview) data from a variety of sources:

* Azure SQL Database or SQL Server on an Azure virtual machine
* Azure Cosmos DB
* Azure Blob Storage
* Azure Table Storage
* Indexing CSV blobs using the Azure Search Blob indexer
* Indexing JSON blobs with Azure Search Blob indexer

In our case, we'll upload our data to Blob Storage and let Azure Search index it from there. Hence, we need to create an new `Storage Account` and create a new `Blob container`, where we'll upload our [dataset](../data/search-dataset-pdf.zip) to. We can do this completely through the Azure Portal, use [Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/) or use the API/CLI.

Once we have uploaded the PDF files, we can go into our Azure Search instance and goto `Import Data`:

![alt text](../images/azure_search_import_data.png "Azure Search Import Data")

Next, we need to define the `Data Source`:

![alt text](../images/azure_search_new_datasource.png "Azure Search Existing Data Source")

We'll skip `Cognitive Search` for this example (we'll get back to it soon). Azure Search automatically looks at the Blob container and will now extract the content and the metadata from all the PDFs. Let's give our Index a better name:

![alt text](../images/azure_search_create_index.png "Azure Search Index Setting")

:question: Question: Does it make sense to have any of the fields `Filterable`, `Sortable`, or `Facetable`?

Lastly, we need to give our Indexer a name and also set the schedule. In our case, we'll only run it once, but in a real world scenario, we might want to keep it running to index new, incoming data:

![alt text](../images/azure_search_indexer.png "Azure Search Indexer")

After a minute or two, our Indexer will have indexed all the PDFs and we should be able to query them.

![alt text](../images/azure_search_indexed.png "Azure Search indexed our PDFs")

## Querying Content

Azure Search now indexed all our PDFs via the `pdf-blob-indexer` into the `pdf-index` index. Ideally, we would use the REST API of Azure Search to perform sophisticated queries, but in our case, we can use the `Azure Search Explorer`:

![alt text](../images/azure_search_explorer.png "Azure Search Explorer")

[Querying data](https://docs.microsoft.com/en-us/azure/search/search-query-overview) in Azure Search can get quite sophisticated, but for our example here, we can just put in a simple query:

![alt text](../images/azure_search_explorer_example.png "Azure Search Query Example")

Using double-quotes `"..."` will search for the whole string, rather than each substring. If we want to make a search term mandatory, we need to prefix a `+`. There is a billion more things we can do, but for now, we'll see that we get one document back, as one only one PDF contained the term `Content Moderator`:

```json
{
    "@odata.context": "https://bootcampsearch42.search.windows.net/indexes('pdf-index')/$metadata#docs",
    "value": [
        {
            "@search.score": 0.16848493,
            "content": "\n05/11/2018 What are Azure Cognitive Services? | Microsoft Docs\n\nhttps://docs.microsoft.com/en-us/azure/cognitive-services/welcome 1/4\n\nAzure Cognitive Services are APIs, SDKs, and services available to help developers build intelligent\n\napplications ...",
            "metadata_storage_path": "aHR0cHM6Ly9ib290Y2FtcHMuYmxvYi5jb3JlLndpbmRvd3MubmV0L2RhdGFzZXRzL1doYXQlMjBhcmUlMjBBenVyZSUyMENvZ25pdGl2ZSUyMFNlcnZpY2VzLnBkZg2"
        }
    ]
}
```

If we want to figure out the original file, we can look at: `metadata_storage_path`. Since it is base64-encoded, we need to decode it, either via command line or by using e.g., [www.base64decode.org](https://www.base64decode.org/):

```
https://bootcamps.blob.core.windows.net/datasets/What%20are%20Azure%20Cognitive%20Services.pdf
```

Perfect, now we know which document contained the term `Content Moderator`.

## Indexing unstructured content (e.g. images, audio, etc.)

In the first part, we've seen that Azure Search can index data like PDFs, PowerPoints, etc., as long as the documents are easily machine readable (=text). [Azure Cognitive Search](https://docs.microsoft.com/en-us/azure/search/cognitive-search-concept-intro) allows us to also index unstructured data. More precisely, it add capabilities for data extraction, natural language processing (NLP), and image processing to Azure Search indexing pipeline (for more see [here](https://docs.microsoft.com/en-us/azure/search/cognitive-search-concept-intro#key-features-and-concepts)). In Azure Cognitive Search, a skillset responsible for the pipeline of the data and consists of multiple skills. Some skills have been pre-included, but it is also possible for us to write our own skills.

[Azure Cognitive Search](https://docs.microsoft.com/en-us/azure/search/cognitive-search-quickstart-blob)

As before, let's upload our data to Blob Storage and let Azure Search index it from there - in a separate index obviously. In our existing Storage Account, we'll create a new `Blob container`, where we'll upload our [dataset](../data/search-dataset-cognitive.zip) to.

Once we're done, we'll repeat the steps from before, `Import Dataset`, walk through the wizard, but this time, we'll configure the `Cognitive Search` part in the second tab.

![alt text](../images/cognitive_search_setup.png "Setting up Cognitive Search")

Next, we need to define the skillset. In our case, we'll enable all features:

![alt text](../images/cognitive_search_skillset.png "Defining the skillset")

We might not want to make our `content` field retrievable, as it does not necessarily provide a lot of value - however, we want to keep it `searchable`, so that Azure Search can do its job. Since we have the original files in Blob and the location stored in `metadata_storage_path`, we can always just retrieve the original file.

Once we finished the next two tabs, Azure Cognitive Search will start indexing our data (this will take a bit longer, as it needs to run image recognition, OCR, etc. on the files). We might see some error, which should be expected:

```json
{
  "key": "https://xxxxxxx.blob.core.windows.net/dataset-cognitive/10-K-FY16.html",
  "message": "Truncated extracted text to 32768 characters."
}
```

Thank you `Free Tier` for only allowing 32768 characters per document...

Let's try some search queries:

* `"Pin to Dashboard"` --> returns `create-search-service.png` (text was recognized via OCR)
* `"Charlotte"` --> returns `MSFT_cloud_architecture_contoso.pdf` (location was recognized via OCR in image)

Good, so looks like our skillset worked. Please note that ideally we'd query through the API an directly specify the relevant fields, e.g., `location:Charlotte` in the second example:

* [Simple Query Syntax](https://docs.microsoft.com/en-us/rest/api/searchservice/simple-query-syntax-in-azure-search)
* [Lucene Query Syntax](https://docs.microsoft.com/en-us/rest/api/searchservice/lucene-query-syntax-in-azure-search)

## Using the API

We've been lazy and did everything through the portal - obviously not the way we want to work in the real world. Especially data ingestion and search should (and most likely needs) to be performed through the API. Luckily, the API is pretty easy to use (even using `curl` for it is easy):

* [Create an index](https://docs.microsoft.com/en-us/azure/search/search-create-index-rest-api)
* [Import Data](https://docs.microsoft.com/en-us/azure/search/search-import-data-rest-api)
* [Search](https://docs.microsoft.com/en-us/azure/search/search-query-rest-api)

For sake of time today we won't be able to go into more detail here, but feel free to have a look at it.



https://azjobsdemo.azurewebsites.net/


https://docs.microsoft.com/en-us/samples/azure-samples/search-dotnet-asp-net-mvc-jobs/search-dotnet-asp-net-mvc-jobs/