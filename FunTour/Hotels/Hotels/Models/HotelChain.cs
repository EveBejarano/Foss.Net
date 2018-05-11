using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotels.Models
{
    public class HotelChain
    {
        [Key]
        public string ChainID { get; set; }

        [Required]
        public string ChainName { get; set; }

        public byte[] ChainLogo { get; set; }

        public ICollection<Hotel> Hotels { get; set; }
    }
}