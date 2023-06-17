using RatingService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Service
{
    public class RecommendationService {

        private readonly Neo4jRecommendationRepository _repository;

        public RecommendationService(Neo4jRecommendationRepository repository)
        {
            _repository = repository;
        }

       

        //function will be renamed
        public async Task GetSimilarUsers(string id)
        {
            List<string> similarUsers = await _repository.GetSimilarUsers(id);
            List<string> accommodation = await _repository.GetAccommodationWithGoodRatingFrom(similarUsers);

        }
    }
}
