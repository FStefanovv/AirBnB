using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.DTO;
using Users.Model;
using Users.Services;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Primitives;
using Users.RabbitMQ;
using MassTransit;
using MassTransit.Transports;

namespace Users.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService, ISendEndpointProvider sendEndpointProvider)
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


        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult RegisterUser(RegistrationDTO registrationData)
        {
            try
            {
                SuccessfulRegistrationDTO dto = _userService.Register(registrationData);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet]
        [Route("get-host")]
        public ActionResult GetHost()
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            User user = _userService.GetUser(userId);


            return Ok(user);
        }

        [HttpGet]
        [Route("get-regular")]
        public ActionResult GetRegular()
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            User user = _userService.GetUser(userId);


            return Ok(user);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("update-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateUser(UserChangeInfoDTO changeData)
        {
            try
            {
                Request.Headers.TryGetValue("UserId", out StringValues userId);
                User user = _userService.UpdateUser(userId,changeData);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpDelete]
        [AllowAnonymous]
        [Route("deleteAsGuest")]
        public async Task<IActionResult> DeleteAsGuest()
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            bool canBeDeleted = await _userService.DeleteAsGuest(userId);
            return Ok(canBeDeleted);      
        }


        [HttpDelete]
        [AllowAnonymous]
        [Route("deleteAsHost")]
        public async Task<IActionResult> DeleteAsHost(string id)
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);

            // bool canBeDeleted = await _userService.DeleteAsHost(userId);

            //return Ok(canBeDeleted);
           bool notReal=await _userService.DeleteAsHostSaga(id);
                                                        

            return Ok("Success");
        }


    
    }
}
