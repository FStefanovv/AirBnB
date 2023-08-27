using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationsService
{
    public class MongoDbContext : IDbContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public MongoDbContext(IConfiguration configuration)
        {
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
