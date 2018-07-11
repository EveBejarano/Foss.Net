using FunTour.Models;
using FunTourBusinessLayer.Service;
using FunTourDataLayer.Locality;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FunTour.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataService Service = new DataService();

        public ActionResult Index()
        {
            IEnumerable<City> ListOfCityToGo = Service.UnitOfWork.CityRepository.Get(includeProperties: "Province");
            IEnumerable<City> ListOfCityToStay = Service.UnitOfWork.CityRepository.Get(includeProperties: "Province"); 
            //se cargan listas para elegir ciudad origen y destino
            ViewBag.SelectOrigen = ListOfCityToGo;

            ViewBag.SelectDestino = ListOfCityToStay;

            return View();
        }



        [UserAuthorization]
        public ActionResult About()
        {
            if (this.HasRole("Administrator"))
            {
                //Perform additional tasks and/or extract additional data from 
                //database into view model/viewbag due to administrative privileges...                
            }

            return View();
        }

        // solo ingresa a esta vista si posee el permiso para hacerlo.

        [UserAuthorization]
        public ActionResult Contact()
        {
            return View();
        }
    }
}