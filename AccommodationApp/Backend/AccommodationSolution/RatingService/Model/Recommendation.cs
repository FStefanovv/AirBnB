using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Model
{
    public class Recommendation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Rating { get; set; }
    }
}
