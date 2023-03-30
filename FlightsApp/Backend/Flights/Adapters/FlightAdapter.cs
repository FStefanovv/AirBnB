using Flights.DTOs;
using Flights.Model;
using System;

namespace Flights.Adapters
{
    public class FlightAdapter
    {
        public FlightAdapter() { }
        public static Flight NewFlightDTOToFlight(NewFlightDTO dto)
        {
            Flight flight = new Flight();
            flight.ArrivalPoint = dto.arrivalPoint;
            flight.DeparturePoint = dto.departurePoint;
            flight.DepartureTime = Convert.ToDateTime(dto.departureTime);
            flight.Duration = dto.duration;
            flight.TicketPrice= dto.ticketPrice;
            flight.RemainingTickets = dto.remainingTickets;
            flight.NumberOfPassengers = dto.numberOfPassengers;
            flight.Status = Enums.FlightStatus.SCHEDULED;


            return flight;

        }
    }
}
