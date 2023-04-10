using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accommodation.Model
{
    public class Accommodation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string[] Offers { get; set; }
        public int MinGuests { get; set; }
        public int MaxGuests { get; set; }
        public string Host { get; set; }
    
    }
}
