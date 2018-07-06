using FunTourDataLayer.Reservation;
using FunTourDataLayer.AccountManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace FunTour.Models {
	public class ReservationViewModel {
		//public ReservationViewModel() {
		//	ReservationAmount = 0;
		//}

		public int Id_Reservation { get; set; }
		
		[Display(Name = "Nombre del cliente")]
		public string UserName { get; set; }

		[Display(Name = "Nombre del paquete")]
		public string PackageName { get; set; }

		[Display(Name = "Nombre del hotel")]
		public string HotelName { get; set; }

		[Display(Name = "Habitacion numero")]
		public int RoomNumber { get; set; }
		
		[Display(Name = "Asiento nuemro")]
		public int SeatNumber { get; set; }
		
		[Display(Name = "Ticket reservado")]
		public string EventName { get; set; }
		
		[Display(Name = "Pagado")]
		public bool Pagado { get; set; }	
		public virtual TravelPackage travelPackage { get; set; }
		
		public virtual UserDetails client { get; set; } 
		public virtual ReservedRoom reservedRoom { get; set; }
		public virtual ReservedSeat reservedSeat { get; set; } 
		public virtual BusReservedSeat busReservedSeat { get; set; }
		public virtual ReservedTicket ticket { get; set; }
		

	}
}


		

	
			

