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
	var emailList = '';
	var emailsubject = req.subject;
	var publicitybody = req.description;
	emails.find({}, function(err, mail) {
		mail.forEach(function(email) {
			emailList = emailList + email.url + ' , ';
		});
		emailList = emailList.substring(0, emailList.length -3);
		while (emailList === undefined || publicitybody === undefined || emailsubject === undefined ) {
			require('deasync').runLoopOnce();
		}
		
		var text = fs.readFileSync('../FunTour/Newsletter/publicidad.html').toString('utf-8');
		
		var mailOptions = {
			from: userAccount,
			to: emailList,
			subject: emailsubject,
			html: text
		}

		transporter.sendMail(mailOptions, function(err, info){
			if (err) {
				console.log(err);
			} else {
				console.log('Email sent');
			}
		});
	});
};
