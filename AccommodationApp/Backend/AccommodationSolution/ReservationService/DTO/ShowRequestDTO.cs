namespace ReservationService.DTO
{
    public class ShowRequestDTO
    {
        public string RequestId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string UserId { get; set; }
        public string AccommodationId { get; set; }
        public string HostId { get; set; }
        public int NumberOfGuests { get; set; }

        public ShowRequestDTO()
        {
        }
    }
}