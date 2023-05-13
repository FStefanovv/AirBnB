namespace ReservationService.DTO
{
    public class GetBusyDateForAccommodationDTO
    {
        public string AccommodationId { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public GetBusyDateForAccommodationDTO()
        {
        }
    }
}