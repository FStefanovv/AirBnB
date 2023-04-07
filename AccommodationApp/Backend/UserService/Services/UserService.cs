using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Users.DTO;
using Users.Model;
using Users.Repository;

namespace Users.Services
{
    public class UserService
    {
        private readonly UserRepositoryMongo _userRepository;
        private readonly string key;

        public UserService(UserRepositoryMongo userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            key = configuration.GetSection("JwtKey").ToString();
        }

        public TokenDTO Authenticate(LoginCredentialsDTO credentials)
        {
            var user = _userRepository.GetUserWithCredentials(credentials);

            if (user == null) return null;

            DateTime expires = DateTime.Now.AddHours(1);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Id as string),
                    new Claim(ClaimTypes.Name, user.Email as string),
                    new Claim(ClaimTypes.Role, user.Role as string)
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

        /*
        public SuccessfulRegistrationDTO Register(RegistrationDTO registrationData)
        {
            try
            {
                Validate(registrationData);
                User user = new User(registrationData);
                _userRepository.Create(user);
                return new SuccessfulRegistrationDTO(registrationData.Username);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Validate(RegistrationDTO registrationData)
        {
            if (_userRepository.CheckIfUsernameInUse(registrationData.Username))
                throw new Exception("The entered username already in use");
            else if (_userRepository.CheckIfEMailInUse(registrationData.EMail))
                throw new Exception("The entered email already in use");
            else if (registrationData.Password != registrationData.ConfirmPassword)
                throw new Exception("Passwords don't match");
            else if (registrationData.Password.Length <= 8)
                throw new Exception("Password is too short");
        }*/
    }
}
