using System;
using System.Collections.Generic;
using Flights.Model;
using MongoDB.Driver;

namespace Flights.Repository
{
    public class TicketsRepository
    {
        private readonly IDbContext _context;
        private IMongoCollection<Ticket> _tickets;

        public TicketsRepository(IDbContext context)
        {
            _context = context;
            _tickets = _context.GetCollection<Ticket>("tickets");
        }

        public List<Ticket> GetAll()
        {
            return _tickets.Find(ticket => true).ToList();
        }

        public void Create(Ticket ticket)
        {
            _tickets.InsertOneAsync(ticket);
        }

        public void InvalidateTickets(string id)
        {         
            var filter = Builders<Ticket>.Filter.Eq("FlightInfo.Id", id);
            var update = Builders<Ticket>.Update.Set("Valid", false);
            _tickets.UpdateMany(filter, update);
        }
    }
}