using Flights.Model;
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
        private IMongoCollection<Flight> _flights;

        public FlightsRepository(IXWSDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _flights = database.GetCollection<Flight>(settings.FlightsCollectionName);
        }

        public List<Flight> GetAll()
        {
            return _flights.Find(book => true).ToList();
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