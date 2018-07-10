//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using FunTourBusinessLayer.UnitOfWorks;
//using FunTourDataLayer;
//using FunTourBusinessLayer.Service;
//using FunTourDataLayer.Reservation;
//using FunTour.Models; 
//namespace PruebaUsers.Controllers 
//{
//	public class ReservationController : Controller
//	{
//		private readonly DataService Service = new DataService();

//		//GET: Reservations
//		public ActionResult Index()
//		{
//			return View(Service.UnitOfWork.ReservationRepository.Get());
//		}

//		public ActionResult Index(int userId)
//		{
//			IEnumerable<Reservation> ListOfReservations = Service.UnitOfWork.ReservationRepository.Get(filter: p => p.Client.Id_UserDetails == userId);
//			return View(ListOfReservations);

//		}


//		//GET: Reservation/Details
//		public ActionResult Details(int? id) 
//		{
//			if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
			
//			Reservation reservation = Service.UnitOfWork.ReservationRepository.GetByID(id);
//			if (reservation == null) { return HttpNotFound(); }
//			return View(reservation);
//		}

//		//GET: Reservation/Create
//	    public ActionResult Create()
//	    {
//	        return View();
//	    }
		
//		// POST: Reservation/Create
//		[ValidateAntiForgeryToken]
//		[HttpPost]
//		public ActionResult Create(int id)
//			//Creo que lo ideal seria enrealidad que se le pase el package
//		{
//			var travelpackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == id).FirstOrDefault();
//			var room = Service.UnitOfWork.ReservedRoomRepository.Get(filter: p => p.TravelPackage.Id_TravelPackage == id && p.Available == true).FirstOrDefault();
//			ReservedSeat seat;
//			if (travelpackage.FlightOrBus) 
//			{
//				seat = Service.UnitOfWork.ReservedSeatRepository.Get(filter: p => p.TravelPackage.Id_TravelPackage == id && p.Available == true).FirstOrDefault();
//			} else {
//				seat = Service.UnitOfWork.BusReservedSeatRepository.Get(filter: p => p.TravelPackage.Id_TravelPackage == id && p.Available == true).FirstOrDefault();
//			};
//			var ticket = Service.UnitOfWork.ReservedTicketRepository.Get(filter: p => p.TravelPackage.Id_TravelPackage == id && p.Available == true).FirstOrDefault();

//			Reservation reservation = new Reservation
//			{
//					Id_TravelPackage = travelpackage.Id_TravelPackage,
//					//UserDetails = 
//					TravelPackage = travelpackage,
//					ReservedRoom = room,
//					ReservedSeat = seat,
//					ReservedTicket = ticket,
//					Paid = false
//			};
//			// If model state is valid, update the available field to set it false
//			if (ModelState.IsValid) 
//			{
//				Service.UnitOfWork.ReservationRepository.Insert(reservation);
//				room.Available = false;
//				Service.UnitOfWork.ReservedRoomRepository.Update(room);
//				ticket.Available = false;
//				Service.UnitOfWork.ReservedTicketRepository.Update(ticket);

//				if (travelpackage.FlightOrBus) 
//				{
//					FlightReservedSeat flightSeat = Service.UnitOfWork.ReservedSeatRepository.Get(filter: p => p.Id_ReservedSeat == seat.Id_ReservedSeat).FirstOrDefault();
//					flightSeat.Available = false;
//					Service.UnitOfWork.ReservedSeatRepository.Update(flightSeat);
//				} else {
//					BusReservedSeat busSeat = Service.UnitOfWork.BusReservedSeatRepository.Get(filter: p => p.Id_ReservedSeat == seat.Id_ReservedSeat).FirstOrDefault();
//					busSeat.Available = false;
//					Service.UnitOfWork.BusReservedSeatRepository.Update(busSeat);
//				};

//				Service.UnitOfWork.Save();

//			}
//		}
//		public ActionResult createPayment(int ReservationId, PaymentModel pay)
//		{
//			var reservation = Service.UnitOfWork.ReservationRepository.GetByID(ReservationId);
//			PaymentModel payment = new PaymentModel 
//			{
//				Name = pay.Name,
//				creditCardNumber = pay.creditCardNumber,
//				expirationDate = pay.expirationDate,
//				securityNumber = pay.securityNumber
//			};
		
//			bool state = Service.DoPayment(payment);
				
//			if (state)
//			{
//				reservation.Paid = true;
//				Service.UnitOfWork.ReservationRepository.Update(reservation);
//				Service.UnitOfWork.Save();
//			};

//			//Deberiamos devolver una error
			
//			return View(reservation);
//		}
//         public ActionResult CancelReservation(int ReservationId)
//        {
//            var reservation = Service.UnitOfWork.ReservationRepository.GetByID(ReservationId);
//            if (reservation.Paid)
//            {
//                return RedirectToAction("index");
//            }
//            Service.UnitOfWork.ReservationRepository.Delete(reservation);
//            Service.UnitOfWork.Save();
//            return RedirectToAction("Index");
//        }
    	
    		
    	

//	}
//}
		
