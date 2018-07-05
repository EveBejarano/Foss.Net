namespace FunTourBusinessLayer.Service
{

    //    Ejemplo para crear las reservas
    //el agentID siempre tiene que ser 1 para fantour
    //roomtypeID va a ser el elegido para reservar
    //las fechas se tienen que mandar de vuelta, se supone que van a ser lo mismo que la consulta de disponibilidad
    //roomcount es la cantidad de habitaciones a reservar
    //devuelve json con datos de las reservas hechas
    //    {	"AgentID": 1,
    //	"RoomTypeID": 2,
    //	"Date_start": "2018-06-07",
	
    //	"Date_end": "2018-06-08",
    //	"RoomCount": 2
    //}

//    {
//    "HotelID": 1,
//    "HotelName": "Hotel1",
//    "HotelAddress": "Calle Falsa 123",
//    "HotelCity": "Corrientes",
//    "HotelRegion": "Corrientes",
//    "HotelCountry": "Argentina",
//    "StandardRate": 1000,
//    "FreeRoomCount": 4
//},
    internal class GetHotelsResponse
    {
        public int HotelID { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string HotelCity { get; set; }
        public string HotelRegion { get; set; }
        public string HotelCountry { get; set; }
        public float StandardRate { get; set; }
        public int FreeRoomCount { get; set; }
    }


    //Devuelve asi. ID de las reservas creadas y el id del hotel para comprobar //que sea para ese que elegiste. 

//    [
//    {
//        "BookingID": 29,
//        "HotelID": 2
//    },
//    {
//    "BookingID": 30,
//    "HotelID": 2
//}
//]
    internal class SetReservationsToTravelPackageResponse
    {
        public int HotelID { get; set; }
        public int BookingID { get; set; }

    }


}