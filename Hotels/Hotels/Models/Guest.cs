using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class Guest
    {
        [Key]
        public int GuestID { get; set; }
        
        [Required]
        public string GuestName { get; set; }

        public string GuestAddress { get; set; }

        [DataType(DataType.EmailAddress)]
        public string GuestEmail { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}