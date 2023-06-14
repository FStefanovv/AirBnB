using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Model
{
    public class FlightRequirements
    {
        public string DeparturePoint { get; set; }
        public DateTime DepartureDate { get; set; }

        public FlightRequirements() { }

        public FlightRequirements(string departurePoint, DateTime departureDate)
        {
            DepartureDate = departureDate;
            DeparturePoint = departurePoint;
        }
    }
}
