'use strict';

const mongoose = require('mongoose');
var Payment = mongoose.model('Payments');
var CreditCard = mongoose.model('creditCard');

var searchCreditCard = function (creditcardnumber){
	CreditCard.findOne({ creditCardNumber: creditcardnumber }, function (err, credit) {
		if (err)
			return res.send ({error: error});
		return credit;
	});
};

exports.list_all_payments = function(req, res) {
	Payment.find({}, function(err, payment) {
		if (err)
			return res.send(err);
		res.json(payment);
	});
};

exports.create_a_payment = function(req, res, next) {
	var random_boolean = Math.random() >= 0.5;
	var creditCards = searchCreditCard(req.creditCardNumber);
	if (creditCards == null)
		return next( 'not found');
	var new_payment = new Payment(req.body);
	new_payment.save(function(err, payment) {
		if (err) 
			return next(err);
		var response = {
			state : random_boolean
		};
		res.json(response);
	});
};

exports.paymentDetails = function(req, res) {
	Payment.findById(req.params.paymentId, function(err, payment) {
		if (err)
			return res.send(err);
		res.json(payment);
	});
};

exports.addCreditCard = function(req, res) {
	var new_creditCard = new CreditCard(req.body);
	new_creditCard.save( function(err, creditcard) {
		if (err) 
			return next(err);
		res.json(creditcard);
	});
};
