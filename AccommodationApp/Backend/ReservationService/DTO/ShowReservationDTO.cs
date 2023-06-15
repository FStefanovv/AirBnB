using ReservationService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.DTO
{
    public class ShowReservationDTO
    {
        public string Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string AccommodationName { get; set; }
        public string AccommodationLocation { get; set; }
        public int NumberOfGuests { get; set; }
        public ReservationStatus Status { get; set; }
        public double Price { get; set; }

        public ShowReservationDTO() { }
    }
}
