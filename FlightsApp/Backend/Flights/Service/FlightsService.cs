
﻿using Flights.DTOs;
﻿using Flights.Adapters;
using Flights.DTOs;
using Flights.Model;
using Flights.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

namespace Flights.Service
{
    public class FlightsService : FlightGRPCService.FlightGRPCServiceBase
    {
        private readonly FlightsRepository _flightsRepository;
        private readonly TicketsRepository _ticketsRepository;
        private readonly TicketsService _ticketsService;

        public FlightsService(FlightsRepository flightsRepository, TicketsRepository ticketsRepository, TicketsService ticketsService)
        {
            _flightsRepository = flightsRepository;
            _ticketsRepository = ticketsRepository;
            _ticketsService = ticketsService;
        }

        public List<Flight> GetAll()
        {
            return _flightsRepository.GetAll();
        }

        public void Create(NewFlightDTO dto)
        {
            Flight flight = FlightAdapter.NewFlightDTOToFlight(dto);
            _flightsRepository.Create(flight);
        }

        public void Delete(Flight flight)
        {
            _ticketsRepository.InvalidateTickets(flight.Id);
            _flightsRepository.Delete(flight);
        }
        public Flight GetById(String id)
        {
          return  _flightsRepository.GetById(id);
        }

        public List<Flight> GetSearchedFlights(string departurePoint, string arrivalPoint, int numberOfPassenger, string dateOfDeparture)
        {
            return _flightsRepository.GetSearched(departurePoint,arrivalPoint,numberOfPassenger,dateOfDeparture);
        }

        public void UpdatePastFlights()
        {
            _flightsRepository.UpdatePastFlights();
        }

        public override Task<Recommendations> GetRecommendations(FlightRequirementsGrpc requirementsGrpc, ServerCallContext context)
        {
            
            FlightRequirements requirements = new FlightRequirements(requirementsGrpc.AirportLocation, requirementsGrpc.DepartureDate.ToDateTime(), requirementsGrpc.AccommodationLocation, requirementsGrpc.Direction);
            List<Flight> matchingFlights = _flightsRepository.GetMatchingFlights(requirements);

            List<Recommendation> recommendationsList = new List<Recommendation>();

            foreach(Flight flight in matchingFlights)
            {
                Recommendation temp = new Recommendation
                {
                    FlightId = flight.Id,
                    DepartureTime = Timestamp.FromDateTime(flight.DepartureTime),
                    Duration = flight.Duration,
                    TicketPrice = flight.TicketPrice
                };
                recommendationsList.Add(temp);
            }
            Recommendations response = new Recommendations();

            response.Recommendations_.AddRange(recommendationsList);

            return Task.FromResult(response);
        }

        public override Task<Purchased> PurchaseTickets(TicketInfo ticketInfo, ServerCallContext context)
        {
            string purchaseMessage;
            bool ticketPurchased = false;

            try
            {
                _ticketsService.PurchaseTicketsGrpc(ticketInfo);
                purchaseMessage = "Tickets purchased successfully";
                ticketPurchased = true;
            }
            catch(Exception ex)
            {
                purchaseMessage = ex.Message;
            }
  
            Purchased response = new Purchased
            {
                Successful = ticketPurchased,
                PurchaseMessage = purchaseMessage
            };

            return Task.FromResult(response);
        }
    }
}
