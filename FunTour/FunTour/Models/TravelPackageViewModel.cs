using System;
using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.BusCompany;
using FunTourDataLayer.EventCompany;
using FunTourDataLayer.FlightCompany;
using FunTourDataLayer.Hotel;
using FunTourDataLayer.Locality;

namespace FunTour.Models
{
    public class TravelPackageViewModel
    {
        public TravelPackageViewModel()
        {
            ReservationAmount =0;
        }
        public int Id_TravelPackage { get; set; }

        [Display(Name = "Nombre del Paquete")]
        public string PackageName { get; set; }

        [Display(Name = "Descripción del Paquete")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Día de Salida")]
        public DateTime FromDay { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Día de Llegada")]
        public DateTime ToDay { get; set; }

        public bool FlightOrBus { get; set; }

        public virtual Hotel Hotel { get; set; }
        public virtual Flight ToGoFlight { get; set; }
        public virtual Flight ToBackFlight { get; set; }
        public virtual Bus ToGoBus { get; set; }
        public virtual Bus ToBackBus { get; set; }
        public virtual Event Event { get; set; }

        [Display(Name = "Cantidad de Reservaciones Posibles")]
        public int ReservationAmount { get; set; }
        public virtual City FromPlace { get; set; }
        public virtual City ToPlace { get; set; }

        [Display(Name = "Precio Total del Paquete")]
        public float TotalPrice { get; internal set; }
    }
}