const dotenv = require('dotenv');
const express = require('express');
const bodyParser = require('body-parser');

const app = express();
dotenv.config();
const port = process.env.SERVER_PORT;

// Dapr publishes messages with application/cloudevents+json content-type
app.use(bodyParser.json({ type: 'application/*+json' }));

// subscribe to topic 'mytopic'
app.route('/dapr/subscribe')
    .get((req, res) => {
        res.json(['mytopic']);
    });

// handle 'mytopc' messages
app.route('/mytopic')
    .post((req, res) => {        
        var message = req.body.data;
        console.log(message.Text);
        res.sendStatus(200);
    });

app.listen(port);

console.log('SubscriberNodeJs started on port:' + port);