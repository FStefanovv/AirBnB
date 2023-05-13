using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.DTO;
using Users.Model;

namespace Users.Services
{
    public interface IUserService
    {
        TokenDTO Authenticate(LoginCredentialsDTO credentials);
        SuccessfulRegistrationDTO Register(RegistrationDTO registrationData);

        User GetUser(StringValues userId);
        User UpdateUser(StringValues userId,UserChangeInfoDTO changeData);
        void DeleteAsGuest(StringValues id);
        void DeleteAsHost(StringValues id);
    }
}
