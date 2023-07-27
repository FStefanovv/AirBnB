using System;
using System.Collections;
using System.Collections.Generic;
using Flights.Adapters;
using Flights.ApiKeyAuth;
using Flights.DTOs;
using Flights.Model;
using Flights.Repository;
using Microsoft.JSInterop.Infrastructure;
using MongoDB.Bson;

namespace Flights.Service
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TicketsService
    {
        private readonly TicketsRepository _ticketsRepository;
        private readonly FlightsRepository _flightsRepository;
        private readonly UsersRepository _userRepository;
        private readonly ApiKeyRepository _apiKeyRepository;

        private readonly TicketsAdapter _adapter = new TicketsAdapter();

        public TicketsService(TicketsRepository ticketsRepository,FlightsRepository flightsRepository, UsersRepository userRepository, ApiKeyRepository apiKeyRepository)
        {
            _ticketsRepository = ticketsRepository;
            _flightsRepository = flightsRepository;
            _userRepository = userRepository;
            _apiKeyRepository = apiKeyRepository;
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

        public ApiKey GenerateApiKey(KeyValidUntilDTO dto)
        {
            try
            {
                ApiKey key = new ApiKey { Id = ObjectId.GenerateNewId().ToString(), UserId = dto.UserId, ValidUntil = dto.ValidUntil };
                _apiKeyRepository.Create(key);
                   
                return key;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not generate api key.");
            }
        }

        private Boolean CheckIfThereAreAvailableTickets(string flightId, int numOfTickets)
        {
            Flight flight = _flightsRepository.GetById(flightId);
            if (flight.RemainingTickets - numOfTickets >= 0)
            {
                return true;
            }

            return false;
        }

        public List<ViewTicketDTO> GetTicketsForUser(string userId)
        {
            List<ViewTicketDTO> tickets = new List<ViewTicketDTO>();
            foreach (var ticket in _ticketsRepository.GetAll())
            {
                if (ticket.UserId == userId)
                {
                    tickets.Add(_adapter.TicketToViewTicketDTO(ticket));
                }
            }

            return tickets;
        }


        public void PurchaseTicketsGrpc(TicketInfo ticketInfo)
        {
            User user = _userRepository.GetByEmail(ticketInfo.Email);
            if (user == null)
                throw new Exception("User does not have an account. Cannot purchase.");

            Flight flight = _flightsRepository.GetById(ticketInfo.FlightId);

            if(flight.Status == Enums.FlightStatus.CANCELLED)
                throw new Exception("Flight cancelled");

            if (flight.RemainingTickets < ticketInfo.NumberOfTickets)
                throw new Exception("Not enough tickets");

            BuyTicketDTO dto = new BuyTicketDTO
            {
                userId = user.Id,
                flightId = flight.Id,
                numberOfTickets = ticketInfo.NumberOfTickets,
                price = flight.TicketPrice
            };

            BuyTicket(dto);    
        }

        public void BuyWithApiKey(BuyWithApiKeyDTO dto, string apiKey)
        {
            string userId = GetUserIdFromApiKey(apiKey);
            Flight flight = _flightsRepository.GetById(dto.flightId);
            if(flight.RemainingTickets >= dto.numberOfTickets)
            {
                Ticket ticket = TicketsAdapter.BuyTicketApiKeyDtoToTicket(userId, dto, flight);
                _ticketsRepository.Create(ticket);
                ReduceNumberOfTickets(flight, dto.numberOfTickets);
            }
            else
            {
                throw new Exception("Not enough tickets");
            }




        }

        private string GetUserIdFromApiKey(string apiKey)
        {
            ApiKey key = _apiKeyRepository.GetApiKey(apiKey);

            return key.UserId;
        }
    }
}