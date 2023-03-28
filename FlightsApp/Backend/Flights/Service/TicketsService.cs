using System;
using Flights.Adapters;
using Flights.DTOs;
using Flights.Model;
using Flights.Repository;
using Microsoft.JSInterop.Infrastructure;

namespace Flights.Service
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TicketsService
    {
        private readonly TicketsRepository _ticketsRepository;
        private readonly FlightsRepository _flightsRepository;
        private readonly TicketsAdapter _adapter = new TicketsAdapter();

        public TicketsService(TicketsRepository ticketsRepository,FlightsRepository flightsRepository)
        {
            _ticketsRepository = ticketsRepository;
            _flightsRepository = flightsRepository;
        }

        public void BuyTicket(BuyTicketDTO dto)
        {
            if (CheckIfThereAreAvailableTickets(dto.flightId, dto.numberOfTickets)==true)
            {
                _ticketsRepository.Create(_adapter.BuyTicketDtoToTicket(dto,
                    _flightsRepository.GetById(dto.flightId)));
                ReduceNumberOfTickets(_flightsRepository.GetById(dto.flightId),
                    dto.numberOfTickets);

            }
        }

        private void ReduceNumberOfTickets(Flight flight,int numberOfTickets)
        {
            int flag = flight.RemainingTickets;
            flight.RemainingTickets = flag - numberOfTickets;
            _flightsRepository.UpdateNumberOfTickets(flight);
        }
        
        private Boolean CheckIfThereAreAvailableTickets(string flightId, int numOfTickets)
        {
            Flight flight = _flightsRepository.GetById(flightId);
            if (flight.RemainingTickets - numOfTickets > 0)
            {
                return true;
            }

            return false;
        }
    }
}