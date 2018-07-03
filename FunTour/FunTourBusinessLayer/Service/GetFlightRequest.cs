namespace FunTourBusinessLayer.Service
{
    internal class GetFlightRequest
    {
        public string fromPlace { get; set; }
        public string toPlace { get; set; }
        public object toDay { get; set; }
    }
}