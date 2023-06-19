using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightRecommendationService.Model
{
    public class FlightRecommendation
    {
        public string FlightId { get; set; }
        public DateTime DepartureTime { get; set; }
        public int Duration { get; set; }
        public float TicketPrice { get; set; }


        public FlightRecommendation()
        {

        }

        public FlightRecommendation(string flightId, DateTime departureTime, int duration, float ticketPrice)
        {
            FlightId = flightId;
            DepartureTime = departureTime;
            Duration = duration;
            TicketPrice = ticketPrice;
        }
    }
}
