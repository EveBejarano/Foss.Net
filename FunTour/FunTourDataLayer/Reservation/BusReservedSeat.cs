using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTourDataLayer.Models
{
        public partial class BusReservedSeat
        {
            [Key]   
            public int Id_BusReservedSeat { get; set; }

            public virtual Bus Bus { get; set; }
            public virtual Reservation Reservation { get; set; }
        }

}