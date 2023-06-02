using ReservationService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.DTO
{
    public class ReservationCostDTO
    {
      
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string UserId { get; set; }
        public string AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public int NumberOfGuests { get; set; }
        //public ReservationStatus Status { get; set; }

        public ReservationCostDTO() { }
    }
}
