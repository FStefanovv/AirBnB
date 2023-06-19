using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightRecommendationService.DTO
{
    public class TicketPurchaseDTO
    {
        public string FlightId { get; set; }
        public int NumberOfTickets { get; set; }

        public TicketPurchaseDTO() { }
    }
}
