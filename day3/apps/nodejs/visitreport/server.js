if (process.env.SCM_ENV && process.env.SCM_ENV.toLowerCase() != 'production') var env = require('dotenv').config();
const fastify = require('fastify')({
    logger: true
});
const appInsights = require("applicationinsights");
if (process.env.APPINSIGHTS_KEY) {
    appInsights.setup(process.env.APPINSIGHTS_KEY)
        .setAutoDependencyCorrelation(true)
        .setAutoCollectRequests(true)
        .setAutoCollectPerformance(true)
        .setAutoCollectExceptions(true)
        .setAutoCollectDependencies(true)
        .setAutoCollectConsole(true)
        .setUseDiskRetryCaching(true);
    appInsights.start();
    appInsights.defaultClient.context.tags[appInsights.defaultClient.context.keys.cloudRole] = process.env.APPINSIGHTS_ROLENAME || "visitreport";
}
fastify.register(require('fastify-swagger'), {
    routePrefix: '/docs',
    exposeRoute: true
});
fastify.addHook('onRequest', async (request, reply) => {
    if (process.env.APPINSIGHTS_KEY != '') {
        appInsights.defaultClient.trackNodeHttpRequest({request: request.req, response: reply.res});
    }
    return;
});

fastify.register(require('./routes'));

const start = async () => {
    try {
        await fastify.listen(process.env.PORT || 3000, '0.0.0.0', (err, address) => {

            if (err) {
                fastify.log.error(err)
                process.exit(1)
            }
        });
        fastify.swagger();
    } catch (err) {
        fastify.log.error(err)
        process.exit(1)
    }
};

start();