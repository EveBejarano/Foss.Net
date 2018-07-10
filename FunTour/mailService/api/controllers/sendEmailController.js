var mongoose = require('mongoose');
var emails = mongoose.model('email');
var publicity = mongoose.model('publicity');
var deasync = require('deasync');
var nodemailer = require ('nodemailer');

var userAccount = 'fantouroficial@gmail.com';

var transporter = nodemailer.createTransport({
	service: 'gmail',
	auth: {
		user: userAccount,
		pass: 'spamdacs2018'
	}
});

var fs = require('fs');

exports.sendPublicity = function(req) {
	function send (req) {
		var emailsubject = req.subject;
		var publicitybody = req.description;
		var emaillist = req.emails;
		var filenames = req.fileName;
		while (emaillist === undefined || publicitybody === undefined || filenames === undefined || emailsubject === undefined) {
			require('deasync').runLoopOnce();
		}
		var fileDirectory = '../FunTour/Newsletter/' + filenames;
		var text = fs.readFileSync(fileDirectory).toString('utf-8');
		var mailOptions = {
			from : userAccount,
			to : emaillist,
			subject: emailsubject,
			html: text,
		}
		transporter.sendMail(mailOptions, function(err, info){
			if (err) {
				console.log(err);
			} else { 
				console.log('Email sent');
			};
		});
	};


	send(req);
}
