using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Model
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRequired]
        public string UserId { get; set; }
        [BsonRequired]
        public FlightInfo FlightInfo { get; set; }
        [BsonRequired]
        public int Quantity { get; set; }
        [BsonRequired]
        public bool Valid { get; set; }
        [BsonIgnore]
        public float SummedPrice { get { return Quantity * FlightInfo.TicketPrice;  } }
        
        public Ticket(FlightInfo info, int quantity)
        {   
            FlightInfo = info;
            Quantity = quantity;
            Valid = true;
        }
    }
}
