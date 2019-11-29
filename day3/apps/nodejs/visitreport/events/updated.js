var constants = require('../constants/constants');
var messageBus, logger;

function handle(project) {
    var p = project.constructor.name == 'model' ? project.toJSON() : project;
    var message = {
        body: p,
        contentType: 'application/json',
        userProperties: {
            type: 'project',
            subtype: constants.events.updated,
            appId: 'getfdback',
            'x-fdback-context': p.tenant.toString()
        }
    };
    messageBus.topic.send(message).then(() => { }).catch(err => {
        console.log(err);
    });
    logger.info('updated_event');
}

function init(mb, log) {
    messageBus = mb;
    logger = log;
}

module.exports = {
    handler: handle,
    initialize: init
}