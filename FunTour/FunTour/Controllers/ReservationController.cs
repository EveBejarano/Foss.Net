using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FunTourBusinessLayer.UnitOfWorks;
using FunTourDataLayer;
using FunTourBusinessLayer.Service;
using FunTourDataLayer.Reservation;
using FunTour.Models; 
namespace PruebaUsers.Controllers 
{
	public class ReservationController : Controller
	{
		private readonly DataService Service = new DataService();

		//GET: Reservations
		public ActionResult Index()
		{
			return View(Service.UnitOfWork.ReservationRepository.Get());
		}

		//GET: Reservation/Details
		public ActionResult Details(int? id) 
		{
			if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
			
			Reservation reservation = Service.UnitOfWork.ReservationRepository.GetByID(id);
			if (reservation == null) { return HttpNotFound(); }
			return View(reservation);
		}

		//GET: Reservation/Create
		public ActionResult Create() { return View(); }

		[HttpGet]
		public ActionResult Create(int id)
		{
			var travelpackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == id).FirstOrDefault();
			if (travelpackage.FlightOrBus)
			{
				var reservationViewModel = new ReservationViewModel
				{
					Id_Reservation = travelpackage.Id_TravelPackage,
					UserName = User.Identity.Name
				};
			} else {
				var reservationViewModel = new ReservationViewModel
				{
					Id_Reservation = travelpackage.Id_TravelPackage,
					UserName = User.Identity.Name,
				};
			};
			return View(reservationViewModel);
		}
		
		public ActionResult PayReservation(int ReservationID)
		{
			bool state;
			var reservation = Service.UnitOfWork.ReservationRepository.Get(filter: p => p.Id_Reservation == ReservationID).FirstOrDefault();
			reservation.Paid = state;
			Service.UnitOfWork.ReservationRepository.Update(reservation);
			Service.UnitOfWork.Save();
        		return View(reservation);
        		// No tengo idea como se Pagaria
        		}


		// POST: Reservation/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id_Reservation")] ReservationViewModel reservationViewModel)
		{
			var reservation = new Reservation
			{
				Id_TravelPackage = reservationViewModel.travelPackage.Id_TravelPackage,
				TravelPackage = reservationViewModel.travelPackage,
				Client = reservationViewModel.client,
				Paid = reservationViewModel.Pagado,

			};

			if (ModelState.IsValid)
			{
				Service.UnitOfWork.ReservationRepository.Insert(reservation);
				Service.UnitOfWork.Save();
			}

			return View(reservation);
		}

         public ActionResult CancelReservation(int ReservationId)
        {
            var reservation = Service.UnitOfWork.ReservationRepository.GetByID(ReservationId);
            if (reservation.Paid)
            {
                return RedirectToAction("index");
            }
            Service.UnitOfWork.ReservationRepository.Delete(reservation);
            Service.UnitOfWork.Save();
            return RedirectToAction("Index");
        }
		
			
		

	}
}
		
