var express = require('express');
var app = express();
var port = process.env.PORT || 3400;
var mongoose = require('mongoose');
var Publicity = require('./api/models/publicityModel.js');
var bodyParser = require('body-parser');
var multer = require('multer');

/*var storage = multer.diskStorage({
	destination: function (req, file, callback) {
		callback(null, '../FunTour/Newsletter');
	},
	filename: function (req, file, callback) {
		callback(null, file.originalname);
	}
});

var upload = multer({ storage : storage}).single('Newsletter');

app.get('/', function(req, res) {
	res.sendFile(__dirname + "/index.html");
});

app.post('/api/file', function(req, res) {
	upload(req, res, function (err) {
		if (err)
			return res.send(err);
		res.end("FileDone");
	});
});
*/

mongoose.connect('mongodb://localhost/publicityDB');

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

var routes = require('./api/routes/publicityRoutes');
routes(app);

app.listen(port);
console.log('Server started');

