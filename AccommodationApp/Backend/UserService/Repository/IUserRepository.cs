using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.DTO;
using Users.Model;

namespace Users.Repository
{
    public interface IUserRepository
    {
        void Create(User user);
        User GetUserWithCredentials(LoginCredentialsDTO credentials);
        bool CheckIfEMailInUse(string email);
        bool CheckIfUsernameInUse(string username);
        User GetUser(StringValues userId);
        void UpdateUser(User user);
        User GetById(StringValues id);
        void Delete(User user);
    }
}
