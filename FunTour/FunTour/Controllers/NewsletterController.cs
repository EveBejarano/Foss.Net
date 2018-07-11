using System;
using System.Collections.Generic;
using System.Net.Http;
using FunTour.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FunTourBusinessLayer.Service;
using FunTourDataLayer.Reservation;
using FunTourDataLayer.AccountManagement;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace FunTour.Controllers
{
    public class NewsletterController : Controller
    {
        private static DataService Service = new DataService();
        static HttpClient client = new HttpClient();

        static async Task<Uri> CreateNewsletterAsync(NewsletterModel news)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "localhost:3400/sendEmail", news);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }


        static String getAllEmails()
        {
            string emails = "";
            IEnumerable<IdentityUser> identityUsers = Service.UnitOfWork.UserRepository.GetUsers();
            foreach (var a in identityUsers)
            {
                emails = emails + a.Email.ToString() + " , ";
            };
            emails = emails.Substring(0, emails.Length - 3);

            return emails;
        }

        // GET: sendEmails
        public ActionResult Index(NewsletterModel newslettermodel)
        {
            var news = new NewsletterModel
            {
                subject = newslettermodel.subject,
                description = newslettermodel.description,
                emails = getAllEmails(),
                fileName = newslettermodel.fileName
            };

            return View(news);
        }

    }

}