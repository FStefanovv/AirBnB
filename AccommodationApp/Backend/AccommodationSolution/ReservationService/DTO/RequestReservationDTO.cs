namespace ReservationService.DTO
{
    public class RequestReservationDTO
    {
        public string AccomodationId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string HostId { get; set; }
        public string UserId { get; set; }
        public string AccommodationName { get; set; }
        public string AccommodationLocation { get; set; }
        public double Price { get; set; }

        public RequestReservationDTO()
        {
        }
    }
}