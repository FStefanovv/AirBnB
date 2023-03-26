using Flights.DTOs;
using Flights.Model;
using Flights.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Login(LoginCredentialsDTO credentials)
        {
            var token = _usersService.Authenticate(credentials);

            if (token == null)
                return StatusCode(401, "Wrong username or password");

            return Ok(token);
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult RegisterUser(RegistrationDTO registrationData)
        {
            try
            {
                _usersService.Register(registrationData);
                return StatusCode(201, "Successful registration");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        /*[Authorize(Policy = "Admin")]
        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            return _usersService.GetAll();
        }*/
    }
}
