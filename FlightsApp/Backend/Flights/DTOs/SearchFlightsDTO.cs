using Microsoft.VisualBasic;
using System;

namespace Flights.DTOs
{
    public class SearchFlightsDTO
    {
        public DateTime DepartureTime { get; set; }
        public string DeparturePoint { get; set; }
        public string ArrivalPoint { get; set; }  
        public int NumberOfPassangers { get; set; }  
        
    }
}
