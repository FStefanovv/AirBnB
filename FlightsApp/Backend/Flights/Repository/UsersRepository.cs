using Flights.DTOs;
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

        public void Create(User user)
        {
            _users.InsertOne(user);
        }

        public User GetUserWithCredentials(LoginCredentialsDTO credentials)
        {
            return _users.Find(user => user.Username == credentials.Username && user.Password == credentials.Password).FirstOrDefault();
        }

        public bool CheckIfUsernameInUse(string username)
        {
            return _users.Find(user => user.Username == username).FirstOrDefault()!=null? true : false;
        }

        public bool CheckIfEMailInUse(string email)
        {
            return _users.Find(user => user.EMail == email).FirstOrDefault() != null ? true : false;
        }
    }
}
