using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTourDataLayer.Models
{
    public partial class ReservedTicket
    {

        [Key]
        public int Id_ReservedTicket { get; set; }

        public virtual Reservation Reservation { get; set; }
        public virtual Event Event { get; set; }
    }
}