'use strict';

const mongoose = require('mongoose');
var Schema = mongoose.Schema;

var paymentSchema = new Schema({
	Name: String,
	creditCardNumber: {
		type: String,
		maxlength: 19,
		minlength: 12,
	},
	expirationDate: String,
	securityNumber: {
		type: String,
		maxlength: 3,
		minlength: 3
	}
});

var creditCardSchema = new Schema({
	creditCardNumber: {
		type: String,
		maxlength: 19,
		minlength: 12
	},
	securityNumber: {
		type: String,
		maxlength: 3,
		minlength: 3
	},
	expirationDate: String,
	propietary: {
		name: String,
		lastName: String
	}
});

module.exports = mongoose.model('creditCard', creditCardSchema);
module.exports = mongoose.model('Payments', paymentSchema);
