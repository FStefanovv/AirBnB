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
        User GetUserWithCredentials(LoginCredentialsDTO credentials);
    }
}
