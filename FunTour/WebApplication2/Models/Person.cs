using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Models
{
    public class Person
        {
            [Key]
            public string DNI { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }

           


        }
    }
