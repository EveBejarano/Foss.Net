var express = require('express');
var app = express();
var port = process.env.PORT || 3400;
var mongoose = require('mongoose');
var Publicity = require('./api/models/publicityModel.js');
var bodyParser = require('body-parser');

mongoose.connect('mongodb://localhost/publicityDB');

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

var routes = require('./api/routes/publicityRoutes');
routes(app);

app.listen(port);
console.log('Server started');

