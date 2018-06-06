using FunTourDataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTour.Models
{
    public class TravelPackageViewModel
    {
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
        public virtual Flight Flight { get; set; }
        public virtual Bus Bus { get; set; }
        public virtual Event Event { get; set; }
    }
}