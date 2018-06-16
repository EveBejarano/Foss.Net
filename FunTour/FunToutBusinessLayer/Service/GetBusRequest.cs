using System;

namespace FuntourBusinessLayer.Service
{
    internal class GetBusRequest
    {
        public string fromPlace { get; set; }
        public string toPlace { get; set; }
        public DateTime toDay { get; set; }
    }
}