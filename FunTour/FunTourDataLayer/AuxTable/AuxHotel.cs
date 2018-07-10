using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.Hotel;

namespace FunTourBusinessLayer.Service
{
    public class AuxHotel
    {
        [Key]
        public int Id_Hotel { get; set; }
        public string Name { get; set; }

        public float Price { get; set; }

        public string Description { get; set; }

        public int NotReservedRooms { get; set; }
        public virtual HotelCompany HotelCompany { get; set; }
    }




}