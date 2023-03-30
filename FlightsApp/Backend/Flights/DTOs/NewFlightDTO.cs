namespace Flights.DTOs
{
    public class NewFlightDTO
    {

        public string departurePoint { get; set; }
        public string arrivalPoint { get; set; }
        public string departureTime { get; set; }
        public int duration { get; set; }
        public  float ticketPrice { get; set; }
        public int numberOfPassengers { get; set; }
        public int remainingTickets { get; set; }

        public NewFlightDTO()
        {
        }

        public NewFlightDTO(string departurePoint, string arrivalPoint, string departureTime, int duration, float ticketPrice, int numberOfPassengers, int remainingTickets)
        {
            this.departurePoint = departurePoint;
           this.arrivalPoint = arrivalPoint;
            this.departureTime = departureTime;
            this.duration = duration;
           this.ticketPrice = ticketPrice;
            this.numberOfPassengers = numberOfPassengers;
            this.remainingTickets = remainingTickets;
        }
    }
}
