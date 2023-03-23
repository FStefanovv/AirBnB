using Flights.Model;
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
    }
}
