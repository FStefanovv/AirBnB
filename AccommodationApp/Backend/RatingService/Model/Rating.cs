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
        public int Grade { get; set; }
        public DateTime RatingDate { get; set; }
        public string UserId { get; set; }

        public Rating() { }

        public Rating(string username, string userId, int grade, string entityId)
        {
            Id = Guid.NewGuid().ToString();
            Grade = grade;
            RatingDate = DateTime.Now;
            UserId = userId;
        }
    }
}
