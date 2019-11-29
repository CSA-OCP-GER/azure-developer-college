const CosmosClient = require('@azure/cosmos').CosmosClient;
const client = new CosmosClient({ endpoint: process.env.COSMOSDB, key: process.env.COSMOSKEY });
const uuidv4 = require('uuid/v4');
const databaseId = 'scm';
const containerId = 'visitreports';

async function listReports(contactid) {
    var querySpec = null;
    if (contactid == undefined || contactid == null) {
        querySpec = {
            query: "SELECT c.id, c.contact, c.subject, c.visitDate FROM c where c.type = 'visitreport'"
        }
    } else {
        querySpec = {
            query: "SELECT c.id, c.contact, c.subject, c.visitDate FROM c where c.type = 'visitreport' AND c.contact.id = @contactid",
            parameters: [
                {
                    name: "@contactid",
                    value: contactid
                }
            ]
        }
    }

    try {
        const { resources } =
            await client.database(databaseId).container(containerId).items.query(querySpec,
                { enableCrossPartitionQuery: true }).fetchAll();
        return resources;
    } catch (error) {
        throw new Error(error.message);
    }
}

async function createReports(report) {
    report.id = uuidv4();
    report.type = "visitreport";
    try {
        const { item } = await client.database(databaseId).container(containerId).items.upsert(report);
        return item.id;
    } catch (error) {
        throw new Error(error.message);
    }
}

async function readReports(id) {
    const querySpec = {
        query: "SELECT * FROM c where c.id = @id AND c.type = 'visitreport'",
        parameters: [
            {
                name: "@id",
                value: id
            }
        ]
    };
    try {
        const { resources: results } = await client.database(databaseId).container(containerId).items.query(querySpec, { enableCrossPartitionQuery: true }).fetchAll();
        return results.length > 0 ? results[0] : null;
    } catch (error) {
        throw new Error(error.message);
    }
}

async function updateReports(id, report) {
    try {
        report.type = "visitreport";
        const { item } = await client.database(databaseId).container(containerId).item(report.id, 'visitreport').replace(report);
        return true;
    } catch (error) {
        throw new Error(error.message);
    }
}

async function deleteReports(id) {
    try {
        const { item } = await client.database(databaseId).container(containerId).item(id, 'visitreport').delete();
        return true;
    } catch (error) {
        if (error.code == 404) {
            return false;
        }
        throw new Error(error.message);
    }
}

module.exports = {
    listReports,
    createReports,
    deleteReports,
    readReports,
    updateReports
}