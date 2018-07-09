using System;

namespace FunTourBusinessLayer.Service 
{
	public class GetPaymentRequest
	{
		public string Name {get; set; }
		public string creditCardNumber { get; set; }
		public string expirationDate { get; set; }
		public string securityNumber { get; set; }
	}
}
