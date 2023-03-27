using Flights.DTOs;
using Flights.Model;
using Flights.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Flights.Service
{
    public class UsersService
    {
        private readonly UsersRepository _usersRepository;
        private readonly string key;

        public UsersService(UsersRepository usersRepository, IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            key = configuration.GetSection("JwtKey").ToString();
        }

        public TokenDTO Authenticate(LoginCredentialsDTO credentials)
        {
            var user = _usersRepository.GetUserWithCredentials(credentials);

            if (user == null) return null;

            DateTime expires = DateTime.Now.AddHours(1);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor( ){
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenDTO(tokenHandler.WriteToken(token));
        }

        public void Register(RegistrationDTO registrationData) 
        {
            try
            {
                Validate(registrationData);
                User user = new User(registrationData);
                _usersRepository.Create(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Validate(RegistrationDTO registrationData)
        {
            if (_usersRepository.CheckIfUsernameInUse(registrationData.Username))
                throw new Exception("The entered username already in use");
            else if (_usersRepository.CheckIfEMailInUse(registrationData.EMail))
                throw new Exception("The entered email already in use");
            else if (registrationData.Password != registrationData.ConfirmPassword)
                throw new Exception("Passwords don't match");
            else if (registrationData.Password.Length <= 8)
                throw new Exception("Password is too short");     
        }
    }
}
