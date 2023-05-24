using RatingService.DTO;
using RatingService.Model;
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
            //check if user has had any reservations in accommodation or host's accommodation based on rating type
            if (true)
            {
                Rating rating = new Rating(username, dto.Grade, dto.RatedEntityId);
                RatedEntity entity = _ratingRepository.GetRatedEntity(dto.RatedEntityId);

                if (entity != null)
                {
                    entity.AverageRating = UpdateEntityRating(entity.Id, dto.Grade);
                    _ratingRepository.CreateRatingAsync(rating, entity);
                }
                else {
                    entity = new RatedEntity { Id = dto.RatedEntityId, AverageRating = dto.Grade };
                    _ratingRepository.CreateEntity(rating, entity);   
                } 
            }
            else throw new Exception();
        }

        private float UpdateEntityRating(string id, int grade)
        {
            List<Rating> ratings = _ratingRepository.GetEntityRatings(id);
            float sum = 0;
            foreach(Rating rating in ratings)
            {
                sum += rating.Grade;
            }
            sum += grade;

            return sum / (ratings.Count+1); 
        }
    }
}
