using System;

namespace FunTourBusinessLayer.Service 
{
	internal class GetPaymentRequest
	{
		public string Name {get; set; }
		public string CreditCardNumber { get; set; }
		public string ExpirationDate { get; set; }
		public string SecurityNumber { get; set; }
	}
}
