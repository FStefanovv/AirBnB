using Flights.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace Flights
{
    public class DbContext : IDbContext
    {
        private readonly IConfiguration _configuration;
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public DbContext(IConfiguration configuration)
        {
            // var _mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString);
            _mongoClient = new MongoClient(configuration.GetValue<string>("XWSDatabase:ConnectionString"));
            _db = _mongoClient.GetDatabase(configuration.GetValue<string>("XWSDatabase:DatabaseName"));
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }

    public interface IDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
