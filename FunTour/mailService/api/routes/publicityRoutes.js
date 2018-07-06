'use strict';
module.exports = function(app) {
	var PublicityMail = require('../controllers/publicityController.js');


	app.route('/sendPublicity')
		.post(PublicityMail.sendPublicity);

	app.route('/addEmail')
		.post(PublicityMail.addEmail)
		.get(PublicityMail.listEmail);

	app.route('/deleteEmail/emailId')
		.delete(PublicityMail.deleteEmail);
};
