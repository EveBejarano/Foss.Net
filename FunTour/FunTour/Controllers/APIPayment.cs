using System;
using MercadoPago.DataStructures.Payment;
using MercadoPago.Resources;

namespace PruebaUsers.Controllers
{
    public class APIPayment
    {
        public string DoPayment()
        {
            MercadoPago.SDK.SetAccessToken(TEST-2845946758815722-062022-863d1d2f7b9de36797605a09fff60f9d-207564866);

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