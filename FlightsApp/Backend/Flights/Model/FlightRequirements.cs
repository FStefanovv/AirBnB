using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Model
{
    public class FlightRequirements
    {
        public string AirportLocation { get; set; }
        public DateTime DepartureDate { get; set; }
        public string AccommodationLocation { get; set; }
        public int Direction { get; set; }


        public FlightRequirements() { }

        public FlightRequirements(string airportLocation, DateTime departureDate, string accommodationLocaiton, int direction)
        {
            DepartureDate = departureDate;
            AirportLocation = airportLocation;
            AccommodationLocation = accommodationLocaiton;
            Direction = direction;
        }
    }
}
