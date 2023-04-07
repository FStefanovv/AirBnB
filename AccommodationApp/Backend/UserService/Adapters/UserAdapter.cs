using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.DTO;
using Users.Model;
using Microsoft.AspNetCore.Identity;

namespace Users.Adapters
{
    public static class UserAdapter
    {
        public static User RegistrationDtoToUser(RegistrationDTO dto)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();

            User user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Role = dto.Role
            };
            user.Password = hasher.HashPassword(user, dto.Password);

            return user;
        }


    }
}
