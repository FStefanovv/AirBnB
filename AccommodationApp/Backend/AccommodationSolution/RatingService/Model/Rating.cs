using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Model
{
    public class Rating
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string RatedEntityId { get; set; }
        public string Username { get; set; }
        public int Grade { get; set; }
        public DateTime RatingDate { get; set; }

        public Rating() { }

        public Rating(string username, int grade, string entityId)
        {
            Username = Username;
            Grade = grade;
            RatedEntityId = entityId;
            RatingDate = DateTime.Now;
        }
    }
}
