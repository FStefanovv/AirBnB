using Flights.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Model
{
    public class FlightInfo
    {
        public string Id { get; set; }
        public string DeparturePoint { get; set; }
        public string ArrivalPoint { get; set; }
        public DateTime DepartureTime { get; set; }
        public int Duration { get; set; }
        public float TicketPrice { get; set; }
        public FlightStatus Status { get; set; }

        public FlightInfo(string id, string departurePoint, string arrivalPoint, DateTime departureTime,
                        int duraiton, float ticketPrice)
        {
            Id = id;
            DeparturePoint = departurePoint;
            ArrivalPoint = arrivalPoint;
            DepartureTime = departureTime;
            Duration = duraiton;
            TicketPrice = ticketPrice;
            Status = FlightStatus.SCHEDULED;
        }
    }
}
