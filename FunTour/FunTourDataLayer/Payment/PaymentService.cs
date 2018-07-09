using System.ComponentModel.DataAnnotations;

namespace FunTourDataLayer.Payment
{
	public partial class PaymentService
	{
		public string Name { get; set; }
		public string APIURLToPay { get; set; }
	}
}
