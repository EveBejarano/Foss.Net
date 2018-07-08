'use strict';

module.exports = function(app) {
	var payment = require('../controllers/payments.js');
	
	app.route('/payments')
		.get(payment.list_all_payments)
		.post(payment.create_a_payment);
	
	app.route('/payments/:paymentId')
		.get(payment.paymentDetails);

	app.route('/creditcard')
		.get(payment.addCreditCard);
};
