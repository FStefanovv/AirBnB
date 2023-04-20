﻿using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.DTO;

namespace Users.Services
{
    public interface IUserService
    {
        TokenDTO Authenticate(LoginCredentialsDTO credentials);
        SuccessfulRegistrationDTO Register(RegistrationDTO registrationData);
        void DeleteAsGuest(StringValues id);
        void DeleteAsHost(StringValues id);
    }
}
