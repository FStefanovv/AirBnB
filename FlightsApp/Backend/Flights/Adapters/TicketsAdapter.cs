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

        public ViewTicketDTO TicketToViewTicketDTO(Ticket ticket)
        {
            ViewTicketDTO dto = new ViewTicketDTO();
            dto.Id = ticket.Id;
            dto.UserId = ticket.UserId;
            dto.Quantity = ticket.Quantity;
            dto.SummedPrice = ticket.SummedPrice;
            dto.DeparturePoint = ticket.FlightInfo.DeparturePoint;
            dto.ArrivalPoint = ticket.FlightInfo.ArrivalPoint;
            dto.DepartureTime = ticket.FlightInfo.DepartureTime.ToString();
            dto.Duration = ticket.FlightInfo.Duration;
            return dto;
        }
    }
}