using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.DTO
{
    public class RatingDTO
    {
        public string RatedEntityId { get; set; }
        public int Grade { get; set; }
        public int RatedEntityType { get; set; }
        public RatingDTO() { }
    }

}
