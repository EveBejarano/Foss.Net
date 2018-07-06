var mongoose = require('mongoose');

var emails = mongoose.model('email');
var publicity = mongoose.model('publicity');
var sendEmail = require('./sendEmailController.js');

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

