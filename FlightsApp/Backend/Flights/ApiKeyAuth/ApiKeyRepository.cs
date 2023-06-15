using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.ApiKeyAuth
{
    public class ApiKeyRepository
    {
        private readonly IDbContext _context;
        private IMongoCollection<ApiKey> _keys;
        private IMongoCollection<BsonDocument> _documents;


        public ApiKeyRepository(IDbContext context)
        {
            _context = context;
            _keys = _context.GetCollection<ApiKey>("api_keys");
        }

      
        public ApiKey GetApiKey(string apiKey)
        {
            return _keys.Find(key => key.Id == apiKey).FirstOrDefault();
        }




    }
}
