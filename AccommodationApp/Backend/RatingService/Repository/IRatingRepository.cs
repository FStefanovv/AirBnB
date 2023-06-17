using Microsoft.Extensions.Primitives;
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
        Rating GetRatingByParams(StringValues userId, string ratedEntityId);
        List<Rating> GetEntityRatings(string id);
        void UpdateRating(Rating rating, RatedEntity entity);
        //Rating GetRatingById(string id);
        //void DeleteRating(RatedEntity entity, string id);

        //new methods
        Task<User> GetUser(string id);
        Task<User> CreateUser(string id, string username);
        Task<RatedEntity> GetRatedEntity(string id);
        Task CreateRatedEntity(RatedEntity entity);
        Task MapRating(User user, RatedEntity entity, Rating rating);
    }
}
