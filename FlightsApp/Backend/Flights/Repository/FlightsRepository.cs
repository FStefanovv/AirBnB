﻿using Flights.Model;
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
            return _flights.Find(book => true).ToList();
        }
    }
}
