const CosmosClient = require('@azure/cosmos').CosmosClient;
const axios = require('axios').default;
const client = new CosmosClient({ endpoint: process.env.COSMOSDB, key: process.env.COSMOSKEY });
const databaseId = 'scmvisitreports';
const containerId = 'visitreports';
const container = client.database(databaseId).container(containerId);
const TA_SUBSCRIPTION_KEY = process.env.TA_SUBSCRIPTION_KEY;
var HEADERS_TEMPLATE = {
    'Ocp-Apim-Subscription-Key': TA_SUBSCRIPTION_KEY,
};
const TA_SUBSCRIPTIONENDPOINT = process.env.TA_SUBSCRIPTIONENDPOINT;
const TA_LANGUAGE_PATH = '/text/analytics/v2.1/languages';
const TA_SENTIMENT_PATH = '/text/analytics/v2.1/sentiment';
const TA_KEYPHRASE_PATH = '/text/analytics/v2.1/keyPhrases';

module.exports = async function (context, mySbMsg) {
    context.log('ServiceBus topic trigger function processing message: ', mySbMsg);
    if (mySbMsg.body != null) {
        if (mySbMsg.body.eventType == "VisitReportUpdatedEvent" || mySbMsg.body.eventType == "VisitReportUpdatedEvent") {
            context.log('Message is of type "create" or "update".');
            if (mySbMsg.body.visitResult != null && mySbMsg.body.visitResult != undefined && mySbMsg.body.visitResult != '') {
                var current_language = await detectLanguage(mySbMsg);
                var current_sentiment_score = await getSentimentScore(mySbMsg, current_language);
                var current_keyphrases = await getKeyPhrases(mySbMsg, current_language);

                // Query CosmosDB object
                const querySpec = {
                    query: "SELECT * FROM c where c.id = @id AND c.type = 'visitreport'",
                    parameters: [
                        {
                            name: "@id",
                            value: mySbMsg.body.id
                        }
                    ]
                };

                // Read CosmosDB + update with results.
                try {
                    const { resources: results } = await container.items.query(querySpec).fetchAll();
                    var currentitem = results.length > 0 ? results[0] : null;
                    if(currentitem == null) {
                        return context.done();
                    }
                    currentitem.detectedLanguage = current_language;
                    currentitem.visitResultSentimentScore = current_sentiment_score;
                    currentitem.visitResultKeyPhrases = current_keyphrases;
                    await container.item(currentitem.id, 'visitreport').replace(currentitem);
                    return context.done();
                } catch (error) {
                    context.log.error(error.message);
                    return context.done();
                }
            }
        }
    }
    return context.done();
};

async function getKeyPhrases(mySbMsg, current_language) {
    var current_keyphrases = []; // default.
    var response_keyphrases = await axios.post(`${TA_SUBSCRIPTIONENDPOINT}${TA_KEYPHRASE_PATH}`, [{ id: mySbMsg.body.id, language: current_language, text: mySbMsg.body.visitResult }], {
        headers: HEADERS_TEMPLATE
    });
    var docs_kp = response_keyphrases.data.documents || [];
    if (docs_kp.length) {
        current_keyphrases = docs_kp[0].keyPhrases;
    }
    return current_keyphrases;
}

async function getSentimentScore(mySbMsg, current_language) {
    var current_sentiment_score = 0.0; // default.
    var response_sen = await axios.post(`${TA_SUBSCRIPTIONENDPOINT}${TA_SENTIMENT_PATH}`, [{ id: mySbMsg.body.id, language: current_language, text: mySbMsg.body.visitResult }], {
        headers: HEADERS_TEMPLATE
    });
    var docs_sen = response_sen.data.documents || [];
    if (docs_sen.length) {
        current_sentiment_score = docs_sen[0].score;
    }
    return current_sentiment_score;
}

async function detectLanguage(mySbMsg) {
    var current_language = "en";
    var response_ld = await axios.post(`${TA_SUBSCRIPTIONENDPOINT}${TA_LANGUAGE_PATH}`, [{ id: mySbMsg.body.id, text: mySbMsg.body.visitResult }], {
        headers: HEADERS_TEMPLATE
    });
    var docs_ld = response_ld.data.documents || [];
    if (docs_ld.length > 0 && docs_ld[0].detectedLanguages.length > 0) {
        current_language = docs_ld[0].detectedLanguages[0].iso6391Name;
    }
    return current_language;
}
