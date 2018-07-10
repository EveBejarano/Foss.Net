using System;
using System.ComponentModel.DataAnnotations;
using FunTourDataLayer.EventCompany;

namespace FunTourBusinessLayer.Service
{
    public class AuxEvent
    {
        [Key]
        public int Id_Event { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int AvailableTickets { get; set; }

        public virtual EventCompany EventCompany { get; set; }
    }
}