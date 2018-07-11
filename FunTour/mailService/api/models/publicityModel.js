var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var emailAccountSchema = new Schema({
	url: String
});

var publicitySchema = new Schema({
	subject: String,
	description: String,
	emails: String,
	fileName: String
});

module.exports = mongoose.model('email', emailAccountSchema);
module.exports = mongoose.model('publicity', publicitySchema);


