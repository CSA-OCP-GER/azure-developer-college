const {
    listReportsSchema,
    createReportsSchema,
    readReportsSchema,
    deleteReportsSchema,
    updateReportsSchema
} = require('../schemas/reports');


const {
    listReportsHandler,
    createReportsHandler,
    deleteReportsHandler,
    readReportsHandler,
    updateReportsHandler
} = require('../handler/reports');

module.exports = async function (fastify, opts) {
    fastify.route({
        method: 'GET',
        url: '/',
        handler: async function(request, reply) {
            reply.code(200).send();
        }
    });

    fastify.route({
        method: 'GET',
        url: '/reports',
        schema: listReportsSchema,
        handler: listReportsHandler
    });

    fastify.route({
        method: 'POST',
        url: '/reports',
        schema: createReportsSchema,
        handler: createReportsHandler
    });

    fastify.route({
        method: 'GET',
        url: '/reports/:id',
        schema: readReportsSchema,
        handler: readReportsHandler
    });

    fastify.route({
        method: 'PUT',
        url: '/reports/:id',
        schema: updateReportsSchema,
        handler: updateReportsHandler
    });
    
    fastify.route({
        method: 'DELETE',
        url: '/reports/:id',
        schema: deleteReportsSchema,
        handler: deleteReportsHandler
    });
};