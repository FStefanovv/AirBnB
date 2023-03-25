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

        public List<User> GetAll()
        {
            return _usersRepository.GetAll();
        }

        public string Authenticate(LoginCredentialsDTO credentials)
        {
            var user = _usersRepository.GetAll().Find(x => x.Username == credentials.Username && x.Password == credentials.Password);

            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor( ){
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public void Register(User user)
        {
            _usersRepository.AddUser(user);
        }
    }
}
