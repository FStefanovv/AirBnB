using System;
using System.Collections.Generic;
using Flights.Model;
using MongoDB.Driver;

namespace Flights.Repository
{
    // ReSharper disable once ClassNeverInstantiated.Global
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
            List<Ticket> flightsTickets = _tickets.Find(ticket => ticket.FlightInfo.Id == id).ToList();
            foreach(Ticket t in flightsTickets)
            {
                var filter = Builders<Ticket>.Filter.Eq("FlightInfo.Id", id);
                var update = Builders<Ticket>.Update.Set("Valid", false);
                _tickets.UpdateOne(filter, update);
            }
           
        }
    }
}