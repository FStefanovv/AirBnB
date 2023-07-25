using RatingService.Model;
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

        
        public async Task GetRecommendationsFor(string id)
        {
            List<string> similarUsers = await _repository.GetSimilarUsers(id);
            List<string> accommodation = await _repository.GetAccommodationWithGoodRatingFrom(similarUsers, id);
            List<string> accommodationFiltered = await _repository.FilterAccommodationByLatestRatingsAndSort(accommodation);
            //List<string> accommodationSorted = await _repository.GetSorted(accommodationFiltered);
        }
    }
}
