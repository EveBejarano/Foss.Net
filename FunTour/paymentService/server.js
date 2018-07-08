'use strict';
const express = require('express');
const mongoose = require('mongoose');
const bodyParser = require('body-parser');

const port = process.env.PORT || 3040;
const app = express();

var Payment = require('./api/models/paymentModel.js')

// Database connection and preparation
mongoose.Promise = global.Promise;

mongoose.connection.on("open", function(ref) {
	return console.log("Connected to mongo server!");
});

mongoose.connection.on("error", function(ref) {
	console.log("Could not connect to mongo server!");
	return console.log (err.message);
});

try { 
	mongoose.connect('mongodb://localhost:27017/Payments', { useNewUrlParser: true });
} catch (err) {
	console.log ('FAIL');
}

app.use(bodyParser.urlencoded( { extended: true } ));
app.use(bodyParser.json());

var routes = require('./api/routes/routes.js');

routes(app);

app.listen(port);

console.log('Server started');

