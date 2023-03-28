using Flights.DTOs;
using Flights.Model;

namespace Flights.Adapters
{
    public class TicketsAdapter
    {
        public TicketsAdapter()
        {
        }

        public Ticket BuyTicketDtoToTicket(BuyTicketDTO dto,Flight flight)
        {
            Ticket ticket = new Ticket();
            FlightInfo flightInfo = new FlightInfo();
            ticket.UserId = dto.userId;
            ticket.Quantity = dto.numberOfTickets;
            ticket.SummedPrice = dto.price * dto.numberOfTickets;
            flightInfo.TicketPrice = dto.price;
            flightInfo.Duration = flight.Duration;
            flightInfo.Id = flight.Id;
            flightInfo.DepartureTime = flight.DepartureTime;
            flightInfo.Status = flight.Status;
            flightInfo.ArrivalPoint = flight.ArrivalPoint;
            flightInfo.DeparturePoint = flight.DeparturePoint;
            ticket.FlightInfo = flightInfo;
            ticket.Valid = true;
            return ticket;
        }
    }
}