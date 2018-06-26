using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Models
{
    public class Ticket //ESTO ES UNA RESERVA
    {
        [Key]
        public int ReservaID { get; set; }

        [ForeignKey("Tick")]
        public int TicketID { get; set; }
        public virtual Tick Tick { get; set; }

        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }

    }
}