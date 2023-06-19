using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.DTO;
using Users.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

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
                Username = dto.Username,
                Role = dto.Role,
                Address = dto.Address,
                State=RabbitMQ.SagaState.NOT_DELETED
            };
            user.Password = hasher.HashPassword(user, dto.Password);

            return user;
        }

        public static User UpdateUserDTOToUser(User user,UserChangeInfoDTO dto)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Username = dto.Username;
            user.Role = dto.Role;
            user.Address = dto.Address;
             
            if (dto.Password != "")
            {
                if (dto.Password != dto.ConfirmPassword)
                    throw new Exception("Password and confirmation password need to be the same!");
                else if (dto.Password.Length <= 6)
                    throw new Exception("Password is too short");
                user.Password = hasher.HashPassword(user, dto.Password);
            }

            return user;
        }


    }
}
