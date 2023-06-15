using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.DTOs
{
    public class BuyWithApiKeyDTO
    {
        public string flightId { get; set; }
        public int numberOfTickets { get; set; }

        public BuyWithApiKeyDTO() { }
    }
}
