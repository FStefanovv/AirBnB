namespace Flights.DTOs
{
    public class BuyTicketDTO
    {
        public string userId { get; set; }
        public string flightId { get; set; }
        public int numberOfTickets { get; set; }
        public float price { get; set; }

        public BuyTicketDTO()
        {
        }

        public BuyTicketDTO(string userId, string flightId, int numberOfTickets,float price)
        {
            this.userId = userId;
            this.flightId = flightId;
            this.numberOfTickets = numberOfTickets;
            this.price = price;
        }
    }
}