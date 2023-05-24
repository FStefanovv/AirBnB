using MongoDB.Driver;
using RatingService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repository
{
    public class RatingRepository
    {
        private readonly IDbContext _context;
        private IMongoCollection<RatedEntity> _entities;
        private IMongoCollection<Rating> _ratings;

        public float AverageRating { get; private set; }

        public RatingRepository(IDbContext context)
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

        public void CreateRatingAsync(Rating rating, RatedEntity entity)
        {        
            _ratings.InsertOne(rating);
            _entities.ReplaceOne(e => e.Id == entity.Id, entity);
        }

        public void CreateEntity(Rating rating, RatedEntity entity)
        {
            _ratings.InsertOne(rating);
            _entities.InsertOne(entity);
        }

        public List<Rating> GetEntityRatings(string id)
        {
            return _ratings.Find(rating => rating.RatedEntityId == id).ToList<Rating>();
        }
    }
}
