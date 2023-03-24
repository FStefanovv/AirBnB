using Flights.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Repository
{
    public class UsersRepository
    {
        private readonly IDbContext _context;
        private IMongoCollection<User> _users;
        public UsersRepository(IDbContext context)
        {
            _context = context;
            _users = _context.GetCollection<User>("users");
        }

        public List<User> GetAll()
        {
            return _users.Find(user => true).ToList();
        }
    }
}
