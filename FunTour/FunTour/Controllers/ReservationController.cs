 using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FunTourDataLayer;
using FunTourBusinessLayer.Service;
using FunTourDataLayer.Reservation;
using FunTour.Models;
using FunTourDataLayer.AccountManagement;
using Microsoft.AspNet.Identity;

namespace PruebaUsers.Controllers 
{
	public class ReservationController : Controller
	{
		private readonly DataService Service = new DataService();

		//GET: Reservations
		public ActionResult Index()
		{
		    IEnumerable<Reservation> listOfReservations = new List<Reservation>();
		    if (this.HasRole("Administrator") | this.IsSysAdmin())
		    {
		        listOfReservations = Service.UnitOfWork.ReservationRepository.Get();

		    }
		    else
		    {
		        var userId = this.HttpContext.User.Identity.GetUserId();
		        string username = Service.UnitOfWork.UserRepository.GetUserByID(userId).UserName;
		        UserDetails userdetails = Service.UnitOfWork.UserRepository.GetUserDetailByUserName(username);
		        listOfReservations = Service.UnitOfWork.ReservationRepository.Get(filter: p => p.Client.Id_UserDetails == userdetails.Id_UserDetails, includeProperties: "TravelPackage"); // mil consultas despues -> pum suicidio 

		    }
            return View(listOfReservations);
		}
        

		//GET: Reservation/Details
		public ActionResult Details(int? id) 
		{
		    if (id == null)
		    {
		        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		    }
	
			Reservation reservation = Service.UnitOfWork.ReservationRepository.GetByID(id);
		    if (reservation == null)
		    {
		        return HttpNotFound();
		    }
			return View(reservation);
		}
        [HttpGet]
		//GET: Reservation/Create
	    public ActionResult CreateReservation(int TravelPackageId)
	    {
	        return View();
	    }

        // POST: Reservation/Create

	    [HttpPost, ActionName("Create")]
	    [ValidateAntiForgeryToken]

        public ActionResult CreateConfirmed(int id)
			//Creo que lo ideal seria enrealidad que se le pase el package
		{
    	    var travelpackage = Service.UnitOfWork.TravelPackageRepository.Get(filter: p => p.Id_TravelPackage == id).FirstOrDefault();
		    var room = Service.UnitOfWork.ReservedRoomRepository.Get(filter: p => p.TravelPackage.Id_TravelPackage == id && p.Available == true).FirstOrDefault();

		    ReservedSeat seat;

		    if (travelpackage.FlightOrBus)
		    {
		        FlightReservedSeat flightSeat = Service.UnitOfWork.ReservedSeatRepository.Get(filter: p => p.TravelPackage.Id_TravelPackage == id && p.Available == true).FirstOrDefault();
		        flightSeat.Available = false;
		        seat = flightSeat;
		        Service.UnitOfWork.ReservedSeatRepository.Update(flightSeat);
		    }
		    else
		    {
		        BusReservedSeat busSeat = Service.UnitOfWork.BusReservedSeatRepository.Get(filter: p => p.TravelPackage.Id_TravelPackage == id && p.Available == true).FirstOrDefault();
		        busSeat.Available = false;
		        seat = busSeat;
		        Service.UnitOfWork.BusReservedSeatRepository.Update(busSeat);
		    };

            var ticket = Service.UnitOfWork.ReservedTicketRepository.Get(filter: p => p.TravelPackage.Id_TravelPackage == id && p.Available == true).FirstOrDefault();

		    Reservation reservation = new Reservation
		    {
		        Id_TravelPackage = travelpackage.Id_TravelPackage,
		        Client = Service.UnitOfWork.UserRepository.GetUserDetailByUserName(ControllerContext.HttpContext.User.Identity.Name),
                TravelPackage = travelpackage,
		        ReservedRoom = room,
		        ReservedSeat = seat,
		        ReservedTicket = ticket,
		        Paid = false
		    };
		    room.Available = false;
            ticket.Available = false;
            
		    Service.UnitOfWork.ReservationRepository.Insert(reservation);
		    Service.UnitOfWork.ReservedRoomRepository.Update(room);
		    Service.UnitOfWork.ReservedTicketRepository.Update(ticket);
            Service.UnitOfWork.Save();

		    return RedirectToAction("Index");
		}
		public ActionResult createPayment(int ReservationId, PaymentModel pay)
		{
			var reservation = Service.UnitOfWork.ReservationRepository.GetByID(ReservationId);
			PaymentModel payment = new PaymentModel 
			{
				Name = pay.Name,
				creditCardNumber = pay.creditCardNumber,
				expirationDate = pay.expirationDate,
				securityNumber = pay.securityNumber
			};

			bool state = Service.DoPayment(payment.Name, payment.creditCardNumber, payment.expirationDate, payment.securityNumber);
		
			if (state)
			{
			reservation.Paid = true;
				Service.UnitOfWork.ReservationRepository.Update(reservation);
				Service.UnitOfWork.Save();
			};

			//Deberiamos devolver una error
			
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
		
