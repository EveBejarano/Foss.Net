using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTourDataLayer.Models
{
    public partial class ReservedSeat
    {

        [Key]
        public int Id_ReservedSeat { get; set; }

        public virtual Reservation Reservation { get; set; }
        public virtual Flight Flight { get; set; }
    }
}