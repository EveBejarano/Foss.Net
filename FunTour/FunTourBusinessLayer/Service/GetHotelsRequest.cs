using System;

namespace FunTourBusinessLayer.Service
{

        //    {
        //	"City": "Corrientes",
	
        //	"Region": "Corrientes",
	
        //	"Country": "Argentina",
	
        //	"Date_start": "2018-06-07",
	
        //	"Date_end": "2018-06-08"

        //}
    internal class GetHotelsRequest
        {
            public string City { get; set; }
            public string Region { get; set; }
            public string Country { get; set; }
            public DateTime Date_start { get; set; }
            public DateTime Date_end { get; set; }
    }

    //Para crear las reservas para ese hotel, tantas como se quieran
//    {

//    "HotelID": "2",
//    "Date_start": "2018-06-07",
//    "Date_end": "2018-06-08",
//    "RoomCount": 2
//}
internal class ReservationsToTravelPackageRequest
    {
        public int HotelID { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_end { get; set; }
        public int RoomCount { get; set; }
    }

//    //Para actualizar la reserva cuando se paga
//    {
//    "BookingID": 9,
//    "GuestName": "Greg Velazquez",
//    "GuestEmail": "greg@baconpancakes.com"
//}

internal class UpdateReservationWithClientDataRequest
    {
        public int BookingID { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
    }
}