using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FunTourServiceLayer.Service;
using FunTourDataLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Core.Models;

namespace Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataService Service = new DataService();

        public IActionResult Index()
        {
            DateTime FromDay = System.DateTime.Now;
            DateTime toDay = System.DateTime.Now;

            string FromPlace = "Resistencia";
            string ToPlace = "Corrientes";

            IEnumerable<Bus> ListOfBusesToGo = Service.GetBuses(FromDay, FromPlace, ToPlace);
            IEnumerable<Bus> ListOfBusesToBack = Service.GetBuses(FromDay, FromPlace, ToPlace);

            ViewBag.BusesToGo = new SelectList(ListOfBusesToGo, "IdAPI_Bus", "IdAPI_Bus");
            ViewBag.BusesToBack = new SelectList(ListOfBusesToBack, "IdAPI_Bus", "IdAPI_Bus");
            ViewBag.BusesToGoToShow = ListOfBusesToGo;
            ViewBag.BusesToBackToShow = ListOfBusesToBack;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
