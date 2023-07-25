using Microsoft.Extensions.Primitives;
using RatingService.DTO;
using RatingService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repository
{
    public interface IRatingRepository
    {
        



        //old methods 
        //Rating GetRatingById(string id);
        //void DeleteRating(RatedEntity entity, string id);

        //new methods
        Task<User> GetUser(string id);
        Task<User> CreateUser(string id, string username);
        Task<RatedEntity> GetRatedEntity(string id);
        Task CreateRatedEntity(RatedEntity entity);
        Task MapRating(User user, RatedEntity entity, Rating rating);
        Task<Rating> GetRatingByParams(string userId, string ratedEntityId);
        Task<List<Rating>> GetEntityRatings(string id);
        Task UpdateRating(Rating rating);
        Task UpdateRatedEntity(RatedEntity entity);
        Task DeleteRating(string ratingId);
        Task<Rating> GetRatingById(string id);
        Task<RatedEntity> GetRatedEntityByRating(string ratingId);
        Task<List<RatingWithUsernameDTO>> GetAllEntityRatingsWithUsername(string id);
        Task<bool> CheckIfRatingAbove(string userId);
    }
}
