﻿using Flights.DTOs;
using Flights.Enums;
using Flights.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Repository
{
    public class FlightsRepository
    {
        private readonly IDbContext _context;
        private IMongoCollection<Flight> _flights;
       
        public FlightsRepository(IDbContext context)
        {
            _context = context;
            _flights = _context.GetCollection<Flight>("flights");
        }

        public List<Flight> GetAll()
        {
            return _flights.Find(flight => true).ToList();
        }

        public List<Flight> GetSearched(string departurePoint, string arrivalPoint,int numberOfPassenger,string dateOfDeparture)
        {
            string[] splited = dateOfDeparture.Split('-');
            int year,month, day;
            int.TryParse(splited[0], out year);
            int.TryParse(splited[1], out month);
            int.TryParse(splited[2], out day);
            return _flights.Find(filteredFlight => filteredFlight.DeparturePoint == departurePoint &&
                                                   filteredFlight.ArrivalPoint == arrivalPoint &&
                                                   filteredFlight.RemainingTickets > numberOfPassenger &&
                                                   filteredFlight.DepartureTime.Year == year &&
                                                   filteredFlight.DepartureTime.Month == month &&
                                                   filteredFlight.DepartureTime.Day == day).ToList();
            
        }

        public void UpdatePastFlights()
        {
            DateTime currentTime = DateTime.Now;
            List<Flight> pastFlights = _flights.Find(flight => currentTime > flight.DepartureTime.AddMinutes(flight.Duration) && flight.Status==FlightStatus.SCHEDULED).ToList();
            foreach(Flight flight in pastFlights)
            {
                var filter = Builders<Flight>.Filter.Eq("Id", flight.Id);
                var update = Builders<Flight>.Update.Set("Status", FlightStatus.PAST);
                _flights.UpdateOne(filter, update);
            }
        }

        public List<Flight> GetMatchingFlights(FlightRequirements requirements)
        {
            List<Flight> matching = new List<Flight>();
            if(requirements.Direction==1)
            {
                foreach(Flight flight in _flights.Find(Builders<Flight>.Filter.Empty).ToList()){
                 if(flight.DeparturePoint == requirements.AirportLocation && flight.ArrivalPoint==requirements.AccommodationLocation && (flight.DepartureTime.Day == requirements.DepartureDate.Day) && (flight.DepartureTime.Month == requirements.DepartureDate.Month)  && (flight.DepartureTime.Year == requirements.DepartureDate.Year) && flight.RemainingTickets!=0 && flight.Status==FlightStatus.SCHEDULED)
                    matching.Add(flight);
                }
            }
            else
            {
                foreach(Flight flight in _flights.Find(Builders<Flight>.Filter.Empty).ToList()){
                 if(flight.DeparturePoint == requirements.AccommodationLocation && flight.ArrivalPoint==requirements.AirportLocation && (flight.DepartureTime.Day == requirements.DepartureDate.Day) && (flight.DepartureTime.Month == requirements.DepartureDate.Month) && (flight.DepartureTime.Year == requirements.DepartureDate.Year) && flight.RemainingTickets != 0 && flight.Status == FlightStatus.SCHEDULED)
                    matching.Add(flight);
                }
            }
            return matching;
        }

        public void Create(Flight flight)
        {
           
            _flights.InsertOneAsync(flight);
        }

        public void Delete(Flight flight)
        {
            var filter = Builders<Flight>.Filter.Eq("Id", flight.Id) ;
            var update = Builders<Flight>.Update.Set("Status", Enums.FlightStatus.CANCELLED);

            _flights.UpdateOne(filter, update);

        }

        public Flight GetById(String id)
        {
            foreach(Flight f in _flights.Find(Builders<Flight>.Filter.Empty).ToList()){
                if(f.Id==id)
                    return f;
            }
            return null;
        }

        public void UpdateNumberOfTickets(Flight flight)
        {
            var filter = Builders<Flight>.Filter.Eq("Id", flight.Id);
            var update = Builders<Flight>.Update.Set("RemainingTickets",flight.RemainingTickets);
            _flights.UpdateOne(filter,update);
        }
    }
}