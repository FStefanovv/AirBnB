using System;

namespace ReservationService.DTO
{
    public class ShowRequestDTO
    {
        public string RequestId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string UserId { get; set; }
        public string AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public string HostId { get; set; }
        public int NumberOfGuests { get; set; }

        public ShowRequestDTO()
        {
        }
    }
}