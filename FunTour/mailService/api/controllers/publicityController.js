var mongoose = require('mongoose');

var emails = mongoose.model('email');
var publicity = mongoose.model('publicity');
var sendEmail = require('./sendEmailController.js');

const multer = require('multer');

var storage = multer.diskStorage({
	destination : function (req, file, callback) {
		callback(null, '../FunTour/Newsletter');
	},
	filename : function (req, file, callback) {
		callback(null, file.originalname);
	}
});

var upload = multer({ storage : storage }).single('Newsletter');


exports.addEmail = function(req, res, next) {
	var new_email = new emails(req.body);
	new_email.save(function(err, email) {
		if (err) 
			next(err);
		res.json(email);
	});
}

exports.listEmail = function(req, res, next) {
	emails.find({}, function(err,email) {
		if (err)
			next(err);
		res.json(email);
	})
};

exports.deleteEmail = function(req, res, next) {
	emails.remove({
		_id: req.params.emailId
	}, function (err, email) {
		if (err)
			next(err);
		res.json({ message: 'DONE' });
	});
};


exports.sendPublicity = function(req, res) {
	var publicityBody = new publicity(req.body);
	sendEmail.sendPublicity(publicityBody);
	res.json(publicityBody);
};


exports.renderFileUploader = function(req, res) {
	res.sendFile(__dirname + "/index.html");
};

exports.uploadFile = function(req, res) {
	upload(req, res, function (err) {
		if (err) 
			return res.send(err);
		res.end("File Uploaded");
	});
};
