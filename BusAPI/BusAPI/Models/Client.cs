using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BusAPI.Models
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}