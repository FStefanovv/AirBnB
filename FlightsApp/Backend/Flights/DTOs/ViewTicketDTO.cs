namespace Flights.DTOs
{
    public class ViewTicketDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public float SummedPrice { get; set; }
        public string DeparturePoint { get; set; }
        public string ArrivalPoint { get; set; }
        public string DepartureTime { get; set; }
        public int Duration { get; set; }

        public ViewTicketDTO()
        {
        }

        public ViewTicketDTO(string id, string userId, int quantity, float summedPrice, string departurePoint, string arrivalPoint, string departureTime, int duration)
        {
            Id = id;
            UserId = userId;
            Quantity = quantity;
            SummedPrice = summedPrice;
            DeparturePoint = departurePoint;
            ArrivalPoint = arrivalPoint;
            DepartureTime = departureTime;
            Duration = duration;
        }
    }
}