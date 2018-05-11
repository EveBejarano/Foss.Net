using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Models
{
    public class Country
    {

        public int CountryID { get; set; }

        public string Name { get; set; }
        public string Currency { get; set; }
        public double Change { get; set; }

    }
}