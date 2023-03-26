﻿using Flights.Model;
using MongoDB.Bson;
using MongoDB.Driver;
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
            var filter = Builders<Flight>.Filter.Eq(a =>a.Id, id);

            return _flights.Find(filter).FirstOrDefault();
        }
    }
}