using Flights.Model;
using Flights.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Login([FromBody] User user)
        {
            var token = _usersService.Authenticate(user.Username, user.Password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            return _usersService.GetAll();
        }
    }
}
