using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.DTO;
using Users.Services;

namespace Users.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> Login(LoginCredentialsDTO credentials)
        {
            var token = _userService.Authenticate(credentials);

            if (token == null)
                return StatusCode(401, "Wrong username or password");

            return Ok(token);
        }

        /*
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult RegisterUser(RegistrationDTO registrationData)
        {
            try
            {
                SuccessfulRegistrationDTO dto = _usersService.Register(registrationData);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }*/
    }
}
