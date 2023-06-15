using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Model
{
    public class RatedEntity
    {
        public string Id { get; set; }
        public float AverageRating { get; set; }

        public RatedEntity(){}

        public RatedEntity(string id)
        {
            Id = id;
            AverageRating = 0;
        }
    }
}
