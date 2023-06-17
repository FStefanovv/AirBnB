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
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task CreateAsync(RatingDTO dto, StringValues username, StringValues userId)
        {

            bool canRate = await CheckIfUserCanRate(dto, userId);
            if (canRate)
            {
                User user = await _ratingRepository.GetUser(userId);
                if (user==null)
                    user = await _ratingRepository.CreateUser(userId, username);
                RatedEntity entity = await _ratingRepository.GetRatedEntity(dto.RatedEntityId);
                if (entity==null)
                {
                    Rating rating = new Rating(username, userId, dto.Grade, dto.RatedEntityId);
                    entity = new RatedEntity { Id = dto.RatedEntityId, AverageRating = dto.Grade, Type = dto.RatedEntityType };
                    await _ratingRepository.CreateRatedEntity(entity);
                    await _ratingRepository.MapRating(user, entity, rating);
                }
                else
                {
                    Rating rating = await _ratingRepository.GetRatingByParams(userId.ToString(), dto.RatedEntityId);
                    if (rating != null)
                    {
                        entity.AverageRating = await UpdateEntityRating(entity.Id, dto.Grade, rating.Id);
                        rating.Grade = dto.Grade;
                        await _ratingRepository.UpdateRating(rating);
                        await _ratingRepository.UpdateRatedEntity(entity);
                    }
                    else
                    {
                        rating = new Rating(username, userId, dto.Grade, entity.Id);
                        entity.AverageRating = await UpdateEntityRating(entity.Id, dto.Grade, null);
                        await _ratingRepository.MapRating(user, entity, rating);
                        await _ratingRepository.UpdateRatedEntity(entity);
                    }
                }
            }
            else throw new Exception("User cannot rate this entity");
        }

        
        public async Task DeleteRating(string id, StringValues userId)
        {
            Rating rating = await _ratingRepository.GetRatingById(id);
            
            
            RatedEntity entity = await _ratingRepository.GetRatedEntityByRating(rating.Id);
            entity.AverageRating = await UpdateEntityRatingPostRatingRemoval(entity.Id, rating.Id);

            await _ratingRepository.DeleteRating(rating.Id);
            await _ratingRepository.UpdateRatedEntity(entity);
        }

        private async Task<float> UpdateEntityRatingPostRatingRemoval(string entityId, string ratingId)
        {
            List<Rating> ratings = await GetAllEntityRatings(entityId);

            float sum = 0;

            foreach(Rating r in ratings)
            {
                if (r.Id == ratingId)
                    continue;
                sum += r.Grade;
            }

            return sum / (ratings.Count - 1);
        }

        public async Task<List<Rating>> GetAllEntityRatings(string id)
        {
            return await _ratingRepository.GetEntityRatings(id);
        }

        public async Task<RatedEntity> GetRatedEntity(string id)
        {
            RatedEntity entity = await _ratingRepository.GetRatedEntity(id);
            return entity;
        }

        private async Task<float> UpdateEntityRating(string id, int grade, string ratingId)
        {
            List<Rating> ratings = await _ratingRepository.GetEntityRatings(id);
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
