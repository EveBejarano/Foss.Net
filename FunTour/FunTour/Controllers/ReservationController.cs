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

		public ActionResult Index(int userId)
		{
			IEnumerable<Reservation> ListOfReservations = Service.UnitOfWork.ReservationRepository.Get(filter: p => p.Client.Id_UserDetails == userId);
			return View(ListOfReservations);

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
			ReservationViewModel reservationViewModel;
			reservationViewModel = new ReservationViewModel
			{
					Id_TravelPackage = travelpackage.Id_TravelPackage,
					UserName = User.Identity.Name
			};
			return View(reservationViewModel);
		}
		
		public ActionResult createPayment(int ReservationId, PaymentModel pay)
		{
			var reservation = Service.UnitOfWork.ReservationRepository.Get(filter: p => p.Id_Reservation == ReservationID).FirstOrDefault();
			PaymentModel payment = new PaymentModel 
			{
				Name = pay.Name,
				creditCardNumber = pay.creditCardNumber,
				expirationDate = pay.expirationDate,
				securityNumber = pay.securityNumber
			};
		
			var AuxPayment = Service.doPayment( payment.Name, payment.creditCardNumber, payment.creditCardNumber, payment.securityNumber );

			if (AuxPayment.state)
			{
				reservation.Paid = true;
				Service.UnitOfWork.ReservationRepository.Update(reservation);
				Service.UnitOfWork.Save();
			}
			return View(reservation);
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
		
