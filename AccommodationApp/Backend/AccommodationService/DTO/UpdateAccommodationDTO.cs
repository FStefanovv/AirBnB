namespace Accommodation.DTO
{
    public class UpdateAccommodationDTO
    {

        public string Id { get; set; }
        public string StartSeason { get; set; }
        public string EndSeason { get; set; }
        public double Price { get; set; }

        public UpdateAccommodationDTO()
        {
        }
    }
}
