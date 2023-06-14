
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

        public FlightsService(FlightsRepository flightsRepository, TicketsRepository ticketsRepository)
        {
            _flightsRepository = flightsRepository;
            _ticketsRepository = ticketsRepository;
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
            
            FlightRequirements requirements = new FlightRequirements(requirementsGrpc.DeparturePoint, requirementsGrpc.DepartureDate.ToDateTime());
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
    }
}
