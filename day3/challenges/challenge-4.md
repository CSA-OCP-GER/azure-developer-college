# Cognitive Services #

## Here is what you will learn ##

- Create and use a Sentiment Analysis Service
- Integrate Sentiment API into Application
- Create and use a Computer Vision Services
- Integrate Computer Vision API into Application

## Create and use a Sentiment Analysis Service ##

Create a Sentiment Analysis Service:

- Create a resource group
  - westeurope
- Add Sentiment Analysis
- TODO: Martha Doku
- Hit "Create"

##  Integrate Sentiment API into Application ##

- TODO: Martha Doku


## Create and use a Computer Vision Service ##

Create a Computer Vision Service:

- Create a resource group
  - westeurope
- Add Computer Vision
- TODO: Martha Doku
- Hit "Create"


## Integrate Computer Vision API into Application ##

- TODO: Martha Doku

## Computer Vision ##

In this repository you will find the materials and challenges for the "Azure AI - Bilder, Text und Sprache mit Azure AI verstehen und durchsuchbar machen" bootcamp.

The associated bootcamp teaches you how to deploy and use the following two Azure technologies:

* [Azure Cognitive Services](https://azure.microsoft.com/en-us/services/cognitive-services/)
* [Azure Search](https://azure.microsoft.com/en-us/services/search/) (including Cognitive Search)

The bootcamp is organized as six independent challenges, each one containing a high-level description and a few hints in case you get stuck. We'll touch on the following services:


|Service|Where?|
|---|---|
|[Azure Search + Cognitive Search](https://azure.microsoft.com/en-us/services/search/)|Challenge 01|
|Azure Cognitive Services - [Computer Vision API](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/) and [Custom Vision Service](https://azure.microsoft.com/en-us/services/cognitive-services/custom-vision-service/)|Challenge 02|
|Azure Cognitive Services - [Speech Services](https://azure.microsoft.com/en-us/services/cognitive-services/speech-services/)|Challenge 03|
|Azure Cognitive Services - [Language Understanding](https://azure.microsoft.com/en-us/services/cognitive-services/language-understanding-intelligent-service/)|Challenge 04|
|Azure Cognitive Services - [Text Analytics API](https://azure.microsoft.com/en-us/services/cognitive-services/text-analytics/)|Challenge 05|
|Azure Cognitive Services - [Search API](https://azure.microsoft.com/en-us/services/cognitive-services/directory/search/)|Challenge 06|

# Challenges

You can solve these challenges in a programming language of your choice (some even in `curl` :hammer:). For sake of convenience, we are providing hints in `Python`, which you can easily (and for free) run in [Azure Notebooks](https://notebooks.azure.com). SDK Support for `C#` or `.NET Core` is available for most challenges. Especially Azure Search features an easy-to-use `.NET SDK`. You can find code examples in the Azure documentation for the associated services.

## Challenge 1 (Azure Search & Cognitive Search)

:triangular_flag_on_post: **Goal:** Deploy an Azure Search instance and index a PDF-based data set 

1. Deploy an [Azure Search](https://docs.microsoft.com/en-us/azure/search/search-create-service-portal) instance
1. Index the unstructured PDF data set from [here](data/search-dataset-pdf.zip) - which document contains the term `Content Moderator`?

:question: **Questions:** 

1. What is an Index? What is an Indexer? What is a Data Source? How do they relate to each other?
1. Why would you want to use replicas? Why would you want more partitions?
1. How would you index `json` documents sitting in Azure Blob?

:triangular_flag_on_post: **Goal:** Index an unstructured data set with Cognitive Search

1. Add another index to the Azure Search instance, but this time enable Cognitive Search
1. Index an existing data set coming from `Azure Blob` (data set can be downloaded [here](data/search-dataset-cognitive.zip)) - which document contains the term `Pin to Dashboard`?

:question: **Questions:** 

1. Let's assume we've built a Machine Learning model that can detect beer glasses images (we'll do that in the next challenge) - how could we leverage this model directly in Azure Search for tagging our data?

:see_no_evil: [Hints for challenge 1](hints/challenge_01.md)

## Challenge 2 (Azure Cognitive Services - Vision & Custom Vision)

:triangular_flag_on_post: **Goal:** Leverage OCR to make a hand-written or printed text document in images machine-readable

In the language of your choice (Python solution is provided), write two small scripts that

1. Convert hand-written text from an image into text - Test data: [1](https://bootcamps.blob.core.windows.net/ml-test-images/ocr_handwritten_1.jpg), [2](https://bootcamps.blob.core.windows.net/ml-test-images/ocr_handwritten_2.jpg)
1. Convert printed text from an image into text - Test data: [1](https://bootcamps.blob.core.windows.net/ml-test-images/ocr_printed_1.jpg), [2](https://bootcamps.blob.core.windows.net/ml-test-images/ocr_printed_2.jpg)

:question: **Questions:** 

1. How well does the OCR service work with German text? How well with English?
1. What happens when the image is not oriented correctly?

:triangular_flag_on_post: **Goal:** Detect beer glasses in images

1. Use [Custom Vision](https://customvision.ai) to detect beer glasses in images - [Image Dataset for training and testing](https://bootcamps.blob.core.windows.net/ml-test-images/beer_glasses.zip)

:question: **Questions:** 

1. What could we do to increase the detection performance?
1. What happens if the beer glasses are really small in the image?

:see_no_evil: [Hints for challenge 2](hints/challenge_02.md)

## Challenge 3 (Azure Cognitive Services - Speech)

:triangular_flag_on_post: **Goal:** Leverage Speech-to-Text and Text-to-Speech

In the language of your choice (Python solution is provided), write two small scripts or apps that

1. Convert written text into speech (German or English)
1. Convert speech into written text (German or English)

You can use can use this file: [`data/test.wav`](data/test.wav) (English).

:question: **Questions:** 

1. What happens if you transcribe a long audio file with the speech-to-text API (>15s)?
1. What happens if you select the wrong language in the text-to-speech API? How could you solve this problem?

Now that we have converted a user's speech input into text, we'll try to determine the intent of that text in the next challenge.

:see_no_evil: [Hints for challenge 3](hints/challenge_03.md)

## Challenge 4 (Azure Cognitive Services - Language)

:triangular_flag_on_post: **Goal:** Make your application understand the meaning of text

In the language of your choice (Python solution is provided), write two small scripts or apps that

1. Translate the input text into German (using the Text Translator API)
1. Detect the intent and entities of the text (German) - see examples below (using [https://eu.luis.ai](https://eu.luis.ai))

Let's use an example where we want to detect a Pizza order from the user. We also want to detect if the user wants to cancel an order.

LUIS example data:

```
2 Intents: "CreateOrder", "CancelOrder"

Utterances:

(CreateOrder) Ich moechte eine Pizza Salami bestellen 
(CreateOrder) Vier Pizza Hawaii bitte 

(CancelOrder) Bitte Bestellung 123 stornieren
(CancelOrder) Cancel bitte Bestellung 42
(CancelOrder) Ich will Order 933 nicht mehr

(None) Wieviel Uhr ist heute?
(None) Wie ist das Wetter in Berlin?
(None) Bitte Termin fuer Montag einstellen
```

## Challenge 5 (Azure Cognitive Services - Text Analytics)

:triangular_flag_on_post: **Goal:** Leverage Text Analytics API for extracting language, sentiment, key phrases, and entities from text

In the language of your choice (Python solution is provided), write a small scripts that

1. Extracts sentiment, key phrases and entities from unstructured text using the [Text Analytics API](https://azure.microsoft.com/en-us/services/cognitive-services/text-analytics/)

:question: **Questions:** 

1. What happens if we do not pass in the `language` parameter while getting the sentiment? 

:see_no_evil: [Hints](hints/challenge_05.md)

:question: **Questions:** 

1. Why do we need to fill the `None` intent with examples?
1. What is the `Review endpoint utterances` feature in LUIS?

:see_no_evil: [Hints for challenge 4](hints/challenge_04.md)

## Challenge 6 (Azure Cognitive Services - Search)

:triangular_flag_on_post: **Goal:** Write a script for auto-suggestion of text

1. Leverage Bing Autosuggest to make predictions on how a user might wants to continue an half-written sentence

:question: **Questions:** 

1. What other services does Bing Search offer?
1. How does the service react in case of a denial-of-service (DoS) attack?

:see_no_evil: [Hints for challenge 6](hints/challenge_06.md)


## Text Analytics ## 

# Hints for Text Analytics challenge

Create a new `Python 3.6 Notebook` in [Azure Notebooks](https://notebooks.azure.com/).

Then create a `Text Analytics` API Key in the Azure Portal (in the `West Europe` region):

![alt text](../images/text_analytics_api.png "Text Analytics API")

Let's start with :

```python
import requests
from pprint import pprint

subscription_key = "xxx" # Paste your API key here
text_analytics_base_url = "https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.1/"
headers = {"Ocp-Apim-Subscription-Key": subscription_key}
```

## Detect Language

First, we can extract the language from text:

```python
language_api_url = text_analytics_base_url + "languages"

documents = { "documents": [
    { "id": "1", "text": "This is a document written in English." },
    { "id": "2", "text": "Este es un document escrito en Español." },
    { "id": "3", "text": "这是一个用中文写的文件" }
]}

response  = requests.post(language_api_url, headers=headers, json=documents)
languages = response.json()
pprint(languages)
```

## Detect Sentiment

Second, we can detect the sentiment of a given phrase:

```python
sentiment_url = text_analytics_base_url + "sentiment"

documents = {"documents" : [
  {"id": "1", "language": "en", "text": "I had a wonderful experience! The rooms were wonderful and the staff was helpful."},
  {"id": "2", "language": "en", "text": "I had a terrible time at the hotel. The staff was rude and the food was awful."},  
  {"id": "3", "language": "es", "text": "Los caminos que llevan hasta Monte Rainier son espectaculares y hermosos."},  
  {"id": "4", "language": "es", "text": "La carretera estaba atascada. Había mucho tráfico el día de ayer."}
]}

response  = requests.post(sentiment_url, headers=headers, json=documents)
sentiments = response.json()
pprint(sentiments)
```

## Detect Key Phrases

Third, we can easily detect key phrases from text:

```python
keyphrase_url = text_analytics_base_url + "keyPhrases"

documents = {"documents" : [
  {"id": "1", "language": "en", "text": "I had a wonderful experience! The rooms were wonderful and the staff was helpful."},
  {"id": "2", "language": "en", "text": "I had a terrible time at the hotel. The staff was rude and the food was awful."},  
  {"id": "3", "language": "es", "text": "Los caminos que llevan hasta Monte Rainier son espectaculares y hermosos."},  
  {"id": "4", "language": "es", "text": "La carretera estaba atascada. Había mucho tráfico el día de ayer."}
]}

response  = requests.post(keyphrase_url, headers=headers, json=documents)
key_phrases = response.json()
pprint(key_phrases)
```

## Detect Entities

And last but not least, we can detect the entities in text:

```python
entities_url = text_analytics_base_url + "entities"

documents = {"documents" : [
  {"id": "1", "text": "Microsoft was founded by Bill Gates and Paul Allen on April 4, 1975, to develop and sell BASIC interpreters for the Altair 8800."}
]}

response  = requests.post(entities_url, headers=headers, json=documents)
entities = response.json()
pprint(entities)
```

If you want to directly create a dashboard within Power BI, have a look at [this tutorial](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/tutorials/tutorial-power-bi-key-phrases).