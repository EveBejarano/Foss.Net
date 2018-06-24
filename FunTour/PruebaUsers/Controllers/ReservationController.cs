using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.UnitOfWorks;
using FunTour.Models;
using FunTourDataLayer;
using FuntourBusinessLayer.Service;
using FunTourDataLayer.Models;
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
			var reservation = Service.UnitOfWork.ReservationRepository.Get(filter: p => p.Id_Reservation == id).FirstOrDefault();
			var reservationViewModel = new ReservationViewModel
			{
				Id_Reservation = reservation.Id_Reservation,
				UserName = reservation.Client.UserName,
				HotelName = reservation.ReservedRoom.Hotel.Name,
				RoomNumber = reservation.ReservedRoom.Id_ReservedRoom,
				SeatNumber = reservation.ReservedSeat.Id_ReservedSeat,
				EventName = reservation.ReservedTicket.Event.Name,
				
			};

			return View(reservationViewModel);
		}

		// POST: Reservation/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id_Reservation")] ReservationViewModel reservationViewModel)
		{
			var reservation = new Reservation
			{
				Id_Reservation = reservationViewModel.Id_Reservation,
				Id_TravelPackage = reservationViewModel.travelPackage.Id_TravelPackage,
				TravelPackage = reservationViewModel.travelPackage,
				Client = reservationViewModel.client,
				ReservedRoom = reservationViewModel.reservedRoom,
				ReservedSeat = reservationViewModel.reservedSeat,
				//BusReservedSeat = reservationViewModel.reservedSeat,
				ReservedTicket = reservationViewModel.ticket,
				paid = reservationViewModel.Pagado

			};

			if (ModelState.IsValid)
			{
				Service.UnitOfWork.ReservationRepository.Insert(reservation);
				Service.UnitOfWork.Save();
				return RedirectToAction("AddPlaces", routeValues: new {ReservationViewModel});
			}

			return View(ReservationViewModel);
		}
		
		//GET: Reservation
		public ActionResult Edit(int? id)
		{
			if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

			Reservation reservation = Service.UnitOfWork.ReservationRepository.GetByID(id);

			if (reservation == null) { return HttpNotFound(); }

		}	


	}
}
		
