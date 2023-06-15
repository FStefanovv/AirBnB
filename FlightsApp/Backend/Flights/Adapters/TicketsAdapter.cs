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

        public static Ticket BuyTicketApiKeyDtoToTicket(string userId, BuyWithApiKeyDTO dto, Flight flight)
        {
            Ticket ticket = new Ticket();

            FlightInfo flightInfo = new FlightInfo();

            ticket.UserId = userId;
            ticket.Quantity = dto.numberOfTickets;
            ticket.SummedPrice = flight.TicketPrice * dto.numberOfTickets;
            flightInfo.TicketPrice = flight.TicketPrice;
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
            dto.SummedPrice = ticket.FlightInfo.TicketPrice * ticket.Quantity;
            dto.DeparturePoint = ticket.FlightInfo.DeparturePoint;
            dto.ArrivalPoint = ticket.FlightInfo.ArrivalPoint;
            dto.DepartureTime = ticket.FlightInfo.DepartureTime.ToString();
            dto.Duration = ticket.FlightInfo.Duration;
            dto.Valid = ticket.Valid;
            return dto;
        }
    }
}