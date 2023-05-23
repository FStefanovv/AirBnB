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

        public RatingRepository(IDbContext context)
        {
            _context = context;
            _entities = _context.GetCollection<RatedEntity>("rated_entities");
            _ratings = _context.GetCollection<Rating>("ratings");
        }
    }
}
