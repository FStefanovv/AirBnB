using Microsoft.Extensions.Primitives;
using Neo4j.Driver;
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
            RETURN re{Id: re.Id, AverageRating: re.AverageRating, Type: re.Type}";

            var parameters = new Dictionary<string, object>
        {
            { "id", id }
        };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "re", parameters);
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
        CREATE (u)-[:RATED { Id: $ratingId, Grade: $grade, RatingDate: $ratingDate, UserId: $userId }]->(re)
        RETURN u.Id AS UserId, re.Id AS RatedEntityId";

            var parameters = new Dictionary<string, object>
            {
                { "userId", user.Id },
                { "ratedEntityId", entity.Id },
                { "ratingId", rating.Id },
                { "grade", rating.Grade },
                { "ratingDate", rating.RatingDate }
            };

            await _neo4jDataAccess.ExecuteWriteTransactionAsync<object>(query, parameters);
        }

       

        public async Task<Rating> GetRatingByParams(string userId, string ratedEntityId)
        {
            //        RETURN u{ Id: u.Id, Username: u.Username}";

            var query = @"
            MATCH (u:User { Id: $userId })-[r:RATED]->(re:RatedEntity { Id: $ratedEntityId })
            RETURN r{Id: r.Id, Grade: r.Grade, RatingDate: r.RatingDate}";

            var parameters = new Dictionary<string, object>
                {
                    { "userId", userId },
                    { "ratedEntityId", ratedEntityId }
                };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "r", parameters);
            if (result.Count == 0)
            {
                return null; // or throw an exception, depending on your application's requirements
            }

            var rating = new Rating
            {
                Id = result[0]["Id"].ToString(),
                Grade = Convert.ToInt32(result[0]["Grade"]),
                RatingDate = result[0]["RatingDate"].As<LocalDateTime>().ToDateTime()
            };

            return rating;
        }

        public async Task<List<Rating>> GetEntityRatings(string id)
        {
            var query = @"MATCH ()-[r:RATED]->(re:RatedEntity { Id: $id })
                          RETURN r{Id: r.Id, Grade: r.Grade, RatingDate: r.RatingDate}";

            var parameters = new Dictionary<string, object>
            {
                {
                    "id",  id
                }
            };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "r", parameters);

            if (result.Count == 0)
            {
                return null; // or throw an exception, depending on your application's requirements
            }

            List<Rating> ratings = new List<Rating>();

            for (int i=0; i<result.Count; i++)
            {
                var rating = new Rating
                {
                    Id = result[i]["Id"].ToString(),
                    Grade = Convert.ToInt32(result[i]["Grade"]),
                    RatingDate = result[i]["RatingDate"].As<LocalDateTime>().ToDateTime()
                };

                ratings.Add(rating);
            }
            return ratings;
        }

        public async Task UpdateRating(Rating rating)
        {
            var query = @"
                MATCH ()-[r:RATED { Id: $ratingId }]->()
                SET r.Grade = $newGrade
                RETURN r.Id AS Id";

            var parameters = new Dictionary<string, object>
            {
                { "ratingId", rating.Id },
                { "newGrade", rating.Grade }
            };

            await _neo4jDataAccess.ExecuteWriteTransactionAsync<object>(query, parameters);
        }

        public async Task UpdateRatedEntity(RatedEntity entity)
        {
            var query = @"
                MATCH (re:RatedEntity { Id: $id })
                SET re.AverageRating = $newAverage
                RETURN re.Id AS Id";

            var parameters = new Dictionary<string, object>
            {
                { "id", entity.Id },
                { "newAverage", entity.AverageRating }
            };

            await _neo4jDataAccess.ExecuteWriteTransactionAsync<object>(query, parameters);
        }

        public async Task<Rating> GetRatingById(string id)
        {
            var query = @"
            MATCH ()-[r: RATED { Id: $id }]->()
            RETURN r{Id: r.Id, Grade: r.Grade, RatingDate: r.RatingDate}";

            var parameters = new Dictionary<string, object>
                {
                    { "id", id }
                };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "r", parameters);
            if (result.Count == 0)
            {
                return null; // or throw an exception, depending on your application's requirements
            }

            var rating = new Rating
            {
                Id = result[0]["Id"].ToString(),
                Grade = Convert.ToInt32(result[0]["Grade"]),
                RatingDate = result[0]["RatingDate"].As<LocalDateTime>().ToDateTime()
            };

            return rating;
        }
        public async Task<RatedEntity> GetRatedEntityByRating(string ratingId)
        {
            var query = @"MATCH ()-[r:RATED {Id:$ratingId}]->(re:RatedEntity)
                          RETURN re{Id: re.Id, AverageRating: re.AverageRating, Type: re.Type}";


            var parameters = new Dictionary<string, object>
            {
                {
                    "ratingId",  ratingId
                }
            };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "re", parameters);

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

        public async Task DeleteRating(string ratingId)
        {
            var query = @"MATCH ()-[r:RATED {Id:$ratingId}]->(re: RatedEntity)
                          DELETE r
                          RETURN re.Id as Id";


            var parameters = new Dictionary<string, object>
            {
                {
                    "ratingId",  ratingId
                }
            };

            await _neo4jDataAccess.ExecuteWriteTransactionAsync<object>(query, parameters);
        }
    }
}
