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

        public async Task<List<string>> GetSimilarUsers(string userId)
        {
            List<RatedEntity> entities =  await GetEntitiesRatedBy(userId);
            List<string> similarUsers = new List<string>();

            foreach(RatedEntity entity in entities)
            {
                Rating userRating = await GetRatingByParams(userId, entity.Id);
                List<Rating> usersThatRated = await GetUsersThatRated(entity.Id);
                foreach(Rating r in usersThatRated)
                {
                    if(SimilarRating(r.Grade, userRating.Grade) && r.UserId!=userId && similarUsers.Where(u => u == r.UserId).FirstOrDefault()==null){
                        similarUsers.Add(r.UserId);
                    }
                }

            }

            return similarUsers;
        }



        public async Task<List<string>> GetAccommodationWithGoodRatingFrom(List<string> similarUsers)
        { 
            List<string> accommodationToRecommend = new List<string>();
            foreach(string userId in similarUsers)
            {
                List<string> goodRatingCurrent = await GetWithGoodRatingFrom(userId);
                foreach(string accommodationId in goodRatingCurrent)
                {
                    if (accommodationToRecommend.Where(a => a == accommodationId).FirstOrDefault() == null)
                        accommodationToRecommend.Add(accommodationId);

                }
            }
            return accommodationToRecommend;
        }

        private async Task<List<string>> GetWithGoodRatingFrom(string userId)
        {
            var query = @"MATCH (u:User { Id: $userId })-[r:RATED]->(re:RatedEntity {Type: 0})
                                WHERE r.Grade >= 4
                                RETURN re{Id: re.Id}";

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

            List<string> accommodation = new List<string>();

            for (int i = 0; i < result.Count; i++)
            {
                var accommodationId = result[i]["Id"].ToString();
                   

                accommodation.Add(accommodationId);
            }
            return accommodation;


        }

        private bool SimilarRating(int grade1, int grade2)
        {
            if ((grade1 == 5 || grade1 == 4) && (grade2 == 5 || grade2 == 4))
                return true;
            else if ((grade1 == 1 || grade1 == 2) && (grade2 == 1 || grade2 == 2))
                return true;
            else if (grade1 == 3 && grade2 == 3)
                return true;
            else return false;
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

        private async Task<List<Rating>> GetUsersThatRated(string entityId)
        {
            var getRatings = @"MATCH ()-[r:RATED]->(re:RatedEntity {Type: 0, Id: $entityId})
                                RETURN r{Grade: r.Grade, UserId: r.UserId}";

            var parameters = new Dictionary<string, object>
            {
                {
                    "entityId", entityId
                }
            };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(getRatings, "r", parameters);

            if (result.Count == 0)
            {
                return null; // or throw an exception, depending on your application's requirements
            }


            List<Rating> ratings = new List<Rating>();

            for (int i = 0; i < result.Count; i++)
            {
                var rating = new Rating
                {
                    UserId = result[i]["UserId"].ToString(),
                    Grade = Convert.ToInt32(result[i]["Grade"])
                };

                ratings.Add(rating);
            }
            return ratings;
        }

        public async Task<List<string>> FilterAccommodationByLatestRatingsAndSort(List<string> accommodation)
        {
            var query = @" MATCH (u1:User)-[r:RATED]->(re:RatedEntity)
                           WHERE re.Id IN $accommodationIds 
                            AND NOT EXISTS {
                              MATCH (re)<-[r2:RATED]-(u2:User)
                                WHERE r2.Grade < 3 AND r2.RatingDate >= (date() - duration('P3M'))
                              WITH re, COUNT(r2) AS ratingCount
                              WHERE ratingCount >= 5
                              RETURN re
                            }
                            RETURN DISTINCT re{Id: re.Id, AverageRating: re.AverageRating}
                            ORDER BY re.AverageRating DESC
                            LIMIT 10";
            

            var parameters = new Dictionary<string, object>
            {
                {
                    "accommodationIds", accommodation
                }
            };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "re", parameters);

            if (result.Count == 0)
            {
                return null;
            }


            List<string> accommodationIds = new List<string>();

            for (int i = 0; i < result.Count; i++)
            {
                var accommodationId = result[i]["Id"].ToString();
                accommodationIds.Add(accommodationId);
            }
            return accommodationIds;
        }

        /*
        public async Task<List<string>> GetSorted(List<string> accommodationFiltered)
        {
            var query = @"MATCH (re: RatedEntity)
                            WHERE re.Id IN $accommodationIds 
                            RETURN re{Id: re.Id}
                            ORDER BY re.AverageRating DESC
                            LIMIT 10";

            var parameters = new Dictionary<string, object>
            {
                {
                    "accommodationIds", accommodationFiltered
                }
            };

            var result = await _neo4jDataAccess.ExecuteReadDictionaryAsync(query, "re", parameters);

            if (result.Count == 0)
            {
                return null;
            }


            List<string> accommodationIds = new List<string>();

            for (int i = 0; i < result.Count; i++)
            {
                var accommodationId = result[i]["Id"].ToString();
                accommodationIds.Add(accommodationId);
            }
            return accommodationIds;


        }*/


    }
}
