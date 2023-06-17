using Microsoft.Extensions.Primitives;
using MongoDB.Driver;
using RatingService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repository
{
    public class MongoRatingRepository 
    {
        private readonly IDbContext _context;
        private IMongoCollection<RatedEntity> _entities;
        private IMongoCollection<Rating> _ratings;

        public float AverageRating { get; private set; }

        public MongoRatingRepository(IDbContext context)
        {
            _context = context;
            _entities = _context.GetCollection<RatedEntity>("rated_entities");
            _ratings = _context.GetCollection<Rating>("ratings");
        }

       

        public RatedEntity GetRatedEntity(string id)
        {
            RatedEntity entity = _entities.Find(entity => entity.Id == id).FirstOrDefault();
            return entity;
        }

        public Rating GetRatingByParams(StringValues userId, string ratedEntityId)
        {
            //Rating rating = _ratings.Find(rating => rating.UserId == userId && rating.RatedEntityId == ratedEntityId).FirstOrDefault();
            //return rating;
            
            foreach(Rating r in _ratings.Find(res => true).ToList<Rating>())
            {
                /*
                if(r.UserId==userId && r.RatedEntityId == ratedEntityId)
                {
                    return r;
                }*/
            }
            return null;
        }

        

        public void CreateRating(Rating rating, RatedEntity entity)
        {        
            _ratings.InsertOne(rating);
            _entities.ReplaceOne(e => e.Id == entity.Id, entity);
        }

        public void CreateEntity(Rating rating, RatedEntity entity)
        {
            _ratings.InsertOne(rating);
            _entities.InsertOne(entity);
        }

       public void UpdateRating(Rating rating, RatedEntity entity)
        {
            _ratings.ReplaceOne(r => r.Id == rating.Id, rating);
            _entities.ReplaceOne(e => e.Id == entity.Id, entity);
        }

        public List<Rating> GetEntityRatings(string id)
        {
            return _ratings.Find(rating => "1" == id).ToList<Rating>();
        }

        public Rating GetRatingById(string id)
        {
            return _ratings.Find(rating => rating.Id == id).FirstOrDefault();
        }

        public void DeleteRating(RatedEntity entity, string id)
        {
            _ratings.DeleteOne(r => r.Id == id);
            _entities.ReplaceOne(e => e.Id == entity.Id, entity);
        }
    }
}
