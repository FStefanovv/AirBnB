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

        public async Task GetSimilarUsers(string id)
        {
            await _repository.GetSimilarUsers(id);

        }
    }
}
