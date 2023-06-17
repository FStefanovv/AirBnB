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
        public string Id { get; set; }
        public string RatedEntityId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public int Grade { get; set; }
        public DateTime RatingDate { get; set; }

        public Rating() { }

        public Rating(string username, string userId, int grade, string entityId)
        {
            Username = username;
            UserId = userId;
            Grade = grade;
            RatedEntityId = entityId;
            RatingDate = DateTime.Now;
        }
    }
}
