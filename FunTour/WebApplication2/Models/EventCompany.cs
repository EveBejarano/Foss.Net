using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Models
{
    public class EventCompany
    {
        [Key]
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public int FoundedYear { get; set; }
        public string Description { get; set; }
    }
}