using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.DTOs
{
    public class PurchaseDTO
    {
        public string FlightId {get; set;}
        public string UserId { get; set; }
        public int NumberOfTickets { get; set; }

        public PurchaseDTO() { }
    }
}
