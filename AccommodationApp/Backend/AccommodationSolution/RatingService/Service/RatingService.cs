using RatingService.DTO;
using RatingService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Service
{
    public class RatingService
    {
        private readonly RatingRepository _ratingRepository;

        public RatingService(RatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public void Create(RatingDTO dto, Microsoft.Extensions.Primitives.StringValues username)
        {
            
        }
    }
}
