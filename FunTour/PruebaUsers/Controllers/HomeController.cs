using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FunTour.ActualModels;

namespace FunTour.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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