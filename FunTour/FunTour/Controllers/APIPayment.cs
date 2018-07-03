using System;
using MercadoPago.DataStructures.Payment;
using MercadoPago.Resources;

namespace MercadoPagoExample.Payments
{
    public class APIPayment
    {
        public string DoPayment()
        {
            MercadoPago.SDK.ClientId = "2845946758815722";
            MercadoPago.SDK.ClientSecret = "Joa4kK1zRAbmseZgrFhYGoPbjRV5aXSq";

            Payment payment = new Payment
            {
                TransactionAmount = (float)1000,
                Token = "card_token_id",
                Description = "Cats",
                ExternalReference = "YOUR_REFERENCE",
                PaymentMethodId = "visa",
                Installments = 1,
                Payer = new Payer
                {
                    Email = "some@mail.com",
                    FirstName = "Diana",
                    LastName = "Prince"
                }
            };

            payment.Save();

            return payment.Status.ToString();
        }
    }
}