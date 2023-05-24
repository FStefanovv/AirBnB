using RatingService.DTO;
using RatingService.Model;
using RatingService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using Grpc.Net.Client;

namespace RatingService.Service
{
    public class RatingService
    {
        private readonly RatingRepository _ratingRepository;

        public RatingService(RatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task CreateAsync(RatingDTO dto, StringValues username, StringValues userId)
        {

            bool canRate = await CheckIfUserCanRate(dto, userId);
            if (canRate)
            {
                RatedEntity entity = _ratingRepository.GetRatedEntity(dto.RatedEntityId);
                if (entity==null)
                {
                    Rating rating = new Rating(username, userId, dto.Grade, dto.RatedEntityId);
                    entity = new RatedEntity { Id = dto.RatedEntityId, AverageRating = dto.Grade };
                    _ratingRepository.CreateEntity(rating, entity);
                }
                else
                {
                    Rating rating = _ratingRepository.GetRatingByParams(userId, dto.RatedEntityId);
                    if (rating != null)
                    {
                        entity.AverageRating = UpdateEntityRating(entity.Id, dto.Grade, rating.Id);
                        rating.Grade = dto.Grade;
                        _ratingRepository.UpdateRating(rating, entity);
                    }
                    else
                    {
                        rating = new Rating(username, userId, dto.Grade, entity.Id);
                        entity.AverageRating = UpdateEntityRating(entity.Id, dto.Grade, null);
                        _ratingRepository.CreateRating(rating, entity);
                    }
                }
            }
            else throw new Exception("User cannot rate this entity");
        }

        public bool DeleteRating(string id, StringValues userId)
        {
            Rating rating = _ratingRepository.GetRatingById(id);
            if (rating.UserId == userId)
            {
                RatedEntity entity = _ratingRepository.GetRatedEntity(rating.RatedEntityId);
                entity.AverageRating = UpdateEntityRatingPostRatingRemoval(entity.Id, rating.Id);

                _ratingRepository.DeleteRating(entity, rating.Id);

                return true;
            }
            else return false;
        }

        private float UpdateEntityRatingPostRatingRemoval(string entityId, string ratingId)
        {
            List<Rating> ratings = GetAllEntityRatings(entityId);

            float sum = 0;

            foreach(Rating r in ratings)
            {
                if (r.Id == ratingId)
                    continue;
                sum += r.Grade;
            }

            return sum / (ratings.Count - 1);
        }

        public List<Rating> GetAllEntityRatings(string id)
        {
            return _ratingRepository.GetEntityRatings(id);
        }

        public RatedEntity GetRatedEntity(string id)
        {
            return _ratingRepository.GetRatedEntity(id);
        }

        private float UpdateEntityRating(string id, int grade, string ratingId)
        {
            List<Rating> ratings = _ratingRepository.GetEntityRatings(id);
            float sum = 0;
            foreach(Rating rating in ratings)
            {
                if (ratingId != null && ratingId == rating.Id)
                    sum += grade;
                else
                    sum += rating.Grade;
            }
            if (ratingId == null)
            {
                sum += grade;
                return sum / (ratings.Count + 1);
            }
            else return sum / ratings.Count;      
        }

        private async Task<bool> CheckIfUserCanRate(RatingDTO dto, StringValues userId)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("https://localhost:5003",
                new GrpcChannelOptions { HttpHandler = handler });
            var client = new ReservationGRPCService.ReservationGRPCServiceClient(channel);
            var reply = await client.CheckIfUserCanRateAsync(new RatingData
            {
                UserId = userId,
                RatedEntityId = dto.RatedEntityId,
                RatedEntityType = -1
            }) ;

            return reply.RatingAllowed;
        }


    }
}
