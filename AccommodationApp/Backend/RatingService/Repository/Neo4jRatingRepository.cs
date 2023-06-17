using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using RatingService.Model;
using RatingService.Neo4J;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repository
{
    public class Neo4jRatingRepository : IRatingRepository
    {
        private readonly INeo4jDataAccess _neo4jDataAccess;

        public Neo4jRatingRepository(INeo4jDataAccess neo4jDataAccess)
        {
            _neo4jDataAccess = neo4jDataAccess;
        }

        public async Task<User> GetUser(string userId)
        {
            var query = @"
        MATCH (u:User { Id: $userId })
        RETURN u{ Id: u.Id, Username: u.Username}";

            var parameters = new Dictionary<string, object>
                {
                    { "userId", userId }
                };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "u" , parameters);

            if (result.Count == 0)
            {
                return null; // or throw an exception, depending on your application's requirements
            }

           

            var user = new User
            {
                Id = result[0]["Id"].ToString(),
                Username = result[0]["Username"].ToString()
            };

            return user;
        }

        public async Task<User> CreateUser(string userId, string username)
        {
          
            var query = @"
        CREATE (u:User { Id: $userId, Username: $username })
        RETURN u.Id AS Id, u.Username AS Username";

            var parameters = new Dictionary<string, object>
            {
                { "userId", userId },
                { "username", username }
            };

           await _neo4jDataAccess.ExecuteWriteTransactionAsync<object>(query, parameters);

            User user = await GetUser(userId);

           return user;
        }



        public async Task<RatedEntity> GetRatedEntity(string id)
        {
            var query = @"
            MATCH (re:RatedEntity { Id: $id })
            RETURN re.Id AS Id, re.AverageRating AS AverageRating, re.Type AS Type";

            var parameters = new Dictionary<string, object>
        {
            { "id", id }
        };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "Id", parameters);
            if (result.Count == 0)
            {
                return null; // or throw an exception, depending on your application's requirements
            }

            var ratedEntity = new RatedEntity
            {
                Id = result[0]["Id"].ToString(),
                AverageRating = float.Parse(result[0]["AverageRating"].ToString()),
                Type = int.Parse(result[0]["Type"].ToString())
            };

            return ratedEntity;
        }

        public async Task CreateRatedEntity(RatedEntity entity)
        {
            var query = @"
        CREATE (re:RatedEntity { Id: $id, AverageRating: $averageRating, Type: $type })
            RETURN re.Id AS Id, re.AverageRating AS AverageRating, re.Type as Type";

            var parameters = new Dictionary<string, object>
            {
                { "id", entity.Id },
                { "averageRating", entity.AverageRating },
                { "type", entity.Type }
            };

            await _neo4jDataAccess.ExecuteWriteTransactionAsync<object>(query, parameters);
        }

        public async Task MapRating(User user, RatedEntity entity, Rating rating)
        {
            var query = @"
        MATCH (u:User { Id: $userId })
        MATCH (re:RatedEntity { Id: $ratedEntityId })
        CREATE (u)-[:RATED { Id: $ratingId, UserId: $userId, Username: $username, Grade: $grade, RatingDate: $ratingDate }]->(re)
        RETURN u.Id AS UserId, re.Id AS RatedEntityId";

            var parameters = new Dictionary<string, object>
    {
        { "userId", user.Id },
        { "ratedEntityId", entity.Id },
        { "ratingId", rating.Id },
        { "username", rating.Username },
        { "grade", rating.Grade },
        { "ratingDate", rating.RatingDate }
    };

            await _neo4jDataAccess.ExecuteWriteTransactionAsync<object>(query, parameters);
        }

       

        public Rating GetRatingByParams(StringValues userId, string ratedEntityId)
        {
            throw new NotImplementedException();
        }

        public List<Rating> GetEntityRatings(string id)
        {
            throw new NotImplementedException();
        }

        public void UpdateRating(Rating rating, RatedEntity entity)
        {
            throw new NotImplementedException();
        }        
    }
}
