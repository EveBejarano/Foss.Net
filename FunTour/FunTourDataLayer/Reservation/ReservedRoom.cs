using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunTourDataLayer.Models
{
    public partial class ReservedRoom
    {

        [Key]
        public int Id_ReservedRoom { get; set; }

        public virtual Hotel Hotel { get; set; }
        public virtual Reservation Reservation { get; set; }
    }
}