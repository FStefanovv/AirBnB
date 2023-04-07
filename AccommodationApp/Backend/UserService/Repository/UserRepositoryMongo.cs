using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.DTO;
using Users.Model;

namespace Users.Repository
{
    public class UserRepositoryMongo
    {
        private readonly IDbContext _context;
        private IMongoCollection<User> _users;

        public UserRepositoryMongo(IDbContext context)
        {
            _context = context;
            _users = _context.GetCollection<User>("users");
        }
        public User GetUserWithCredentials(LoginCredentialsDTO credentials)
        {
            List<User> users = _users.Find(user => true).ToList();
            foreach(User user in users)
            {
                if (user.Email == credentials.Email && user.Password == credentials.Password)
                    return user;
            }
            return null;
            //return _users.Find(user => user.Email == credentials.Email && user.Password == credentials.Password).FirstOrDefault();
        }
    }
}
