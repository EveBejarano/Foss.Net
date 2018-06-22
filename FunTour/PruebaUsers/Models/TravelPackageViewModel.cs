using FunTourDataLayer.Models;
using FunTourDataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

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

        [Display(Name = "Día de Salida")]
        public DateTime FromDay { get; set; }

        [Display(Name = "Día de Llegada")]
        public DateTime ToDay { get; set; }

        [Display(Name = "Tildar si desea que el transporte sea aéreo, de lo contrario, dejar sin tildar.")]
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