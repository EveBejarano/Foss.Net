using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class GuestData
    {
        public int BookingID { get; set; }
        public string GuestName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string GuestEmail { get; set; }
    }
}