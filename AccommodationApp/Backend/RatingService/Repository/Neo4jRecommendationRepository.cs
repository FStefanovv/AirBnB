using Neo4j.Driver;
using RatingService.Model;
using RatingService.Neo4J;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repository
{
    public class Neo4jRecommendationRepository
    {
        private readonly INeo4jDataAccess _neo4jDataAccess;

        public Neo4jRecommendationRepository(INeo4jDataAccess neo4jDataAccess)
        {
            _neo4jDataAccess = neo4jDataAccess;
        }

        public async Task<List<User>> GetSimilarUsers(string userId)
        {
            List<RatedEntity> entities =  await GetEntitiesRatedBy(userId);
            List<User> similarUsers = new List<User>();

            foreach(RatedEntity entity in entities)
            {
                Rating usersRating = await GetRatingByParams(userId, entity.Id);
                List<User> usersThatRated = await GetUsersThatRated(entity.Id);


            }

            return null;
        }

        public async Task<List<RatedEntity>> GetEntitiesRatedBy(string userId)
        {

            var query = @"MATCH (u:User { Id: $userId })-[r:RATED]->(re:RatedEntity {Type: 0})
                                RETURN re{Id: re.Id, AverageRating: re.AverageRating, Type: re.Type}";

            var parameters = new Dictionary<string, object>
            {
                {
                    "userId",  userId
                }
            };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "re", parameters);

            if (result.Count == 0)
            {
                return null; // or throw an exception, depending on your application's requirements
            }


            List<RatedEntity> entities = new List<RatedEntity>();

            for (int i = 0; i < result.Count; i++)
            {
                var ratedEntity = new RatedEntity
                {
                    Id = result[0]["Id"].ToString(),
                    AverageRating = float.Parse(result[0]["AverageRating"].ToString()),
                    Type = int.Parse(result[0]["Type"].ToString())
                };

                entities.Add(ratedEntity);
            }
            return entities;
        }


        private async Task<Rating> GetRatingByParams(string userId, string ratedEntityId)
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

        private async Task<List<User>> GetUsersThatRated(string entityId)
        {
            var query = @"MATCH (u:User)-[r:RATED]->(re:RatedEntity {Type: 0, Id: $entityId})
                                RETURN u{Id: u.Id, Username: u.Username}";

            var parameters = new Dictionary<string, object>
            {
                {
                    "entityId", entityId
                }
            };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "u", parameters);

            if (result.Count == 0)
            {
                return null; // or throw an exception, depending on your application's requirements
            }

            List<User> users = new List<User>();

            for (int i = 0; i < result.Count; i++)
            {
                var user = new User
                {
                    Id = result[i]["Id"].ToString(),
                    Username = result[i]["Username"].ToString()
                };

                users.Add(user);
            }
            return users;
        }
    }
}
