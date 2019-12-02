var mb, logger;
const { ReceiveMode } = require("@azure/service-bus");
const CosmosClient = require('@azure/cosmos').CosmosClient;
const dbclient = new CosmosClient({ endpoint: process.env.COSMOSDB, key: process.env.CUSTOMCONNSTR_COSMOSKEY });
const databaseId = 'scmvisitreports';
const containerId = 'visitreports';

function init(messageBus, loggerinstance) {
    mb = messageBus;
    logger = loggerinstance;

    const client = mb.serviceBus.createSubscriptionClient('scmtopic', 'scmcontactvisitreport');
    const receiver = client.createReceiver(ReceiveMode.peekLock, { maxSessionAutoRenewLockDurationInSeconds: 300, sessionId: null });
    receiver.registerMessageHandler((message, ) => {
        consume(message);
    }, (err) => {
        console.log(err);
    }, { autoComplete: false, });
}

function consume(contactsMsg) {
    logger.info('Got new contacts message.: ' + contactsMsg.body.Id);

    var querySpec = {
        query: "SELECT * FROM c where c.type = 'visitreport' AND c.contact.id = @contactid",
        parameters: [
            {
                name: "@contactid",
                value: contactsMsg.body.Id
            }
        ]
    }

    updateInBackground(querySpec, contactsMsg);
}

function updateInBackground(querySpec, contactsMsg) {
    dbclient.database(databaseId).container(containerId).items.query(querySpec).fetchAll().then(({ resources }) => {
        if (resources.length > 0) {
            logger.info("Processing updates in background.");
            resources.forEach(c => {
                c.contact.firstname = contactsMsg.body.Firstname;
                c.contact.lastname = contactsMsg.body.Lastname;
                c.contact.avatarLocation = contactsMsg.body.AvatarLocation;
                c.contact.company = contactsMsg.body.Company;
            });
            var promises = [];
            for (let i = 0; i < resources.length; i++) {
                const element = resources[i];
                promises.push(dbclient.database(databaseId).container(containerId).item(element.id, 'visitreport').replace(element));
            }
            Promise.all(promises).then((responses) => {
                responses.forEach(r => {
                    if (r.statusCode != 200) {
                        logger.info("Error occured. Updates may have been partial.");
                    }
                });
                logger.info("Background updates: done!");
                contactsMsg.complete();
            }).catch(err => {
                logger.error(err);
                contactsMsg.abandon();
            });
        }
        else {
            contactsMsg.complete();
            logger.info("No visitreports found for bulk update.");
        }
    }).catch(() => contactsMsg.abandon());
}

module.exports = {
    initialize: init
}