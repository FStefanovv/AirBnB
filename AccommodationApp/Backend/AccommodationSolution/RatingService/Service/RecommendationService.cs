using Grpc.Net.Client;
using RatingService.Model;
using RatingService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RatingService.Service
{
    public class RecommendationService {

        private readonly Neo4jRecommendationRepository _repository;
        private readonly IRatingRepository _ratingRepository;

        public RecommendationService(Neo4jRecommendationRepository repository, IRatingRepository ratingRepository)
        {
            _repository = repository;
            _ratingRepository = ratingRepository;
        }

        
        public async Task<List<Recommendation>> GetRecommendationsFor(string id)
        {
            List<string> similarUsers = await _repository.GetSimilarUsers(id);
            if (similarUsers == null)
                return new List<Recommendation>();
            else if (similarUsers.Count == 0)
                return new List<Recommendation>();

            List<string> accommodation = await _repository.GetAccommodationWithGoodRatingFrom(similarUsers, id);
            if (accommodation == null)
                return new List<Recommendation>();
            else if (accommodation.Count == 0)
                return new List<Recommendation>();

            List<string> accommodationFiltered = await _repository.FilterAccommodationByLatestRatingsAndSort(accommodation);
            if(accommodationFiltered==null)
                return new List<Recommendation>();
            else if (accommodationFiltered.Count == 0)
                return new List<Recommendation>();

            List<string> accommNames = await GetAccommodationNames(accommodationFiltered);
            
            List<Recommendation> recommendations = await GenerateRecommendations(accommodationFiltered, accommNames);

            return recommendations;
        }

        private async Task<List<Recommendation>> GenerateRecommendations(List<string> accommodationIds, List<string> accommNames)
        {
            List<Recommendation> recommendations = new List<Recommendation>();

            for(int i=0; i<accommodationIds.Count; i++)
            {
                RatedEntity accommodation = await _ratingRepository.GetRatedEntity(accommodationIds[i]);
                Recommendation recomm = new Recommendation { Id = accommodationIds[i], Name = accommNames[i], Rating = accommodation.AverageRating };
                recommendations.Add(recomm);
            }

            return recommendations;
        }

        private async Task<List<string>> GetAccommodationNames(List<string> accommIds)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://accommodation-service:443",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new AccommodationGRPCService.AccommodationGRPCServiceClient(channel);

            AccommodationIds message = new AccommodationIds();
            message.Ids.AddRange(accommIds);

            var reply = await client.GetAccommodationNamesAsync(message);

            return new List<string>(reply.Names);
        }

    }
}
