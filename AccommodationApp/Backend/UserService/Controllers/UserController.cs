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
using OpenTracing;

namespace Users.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITracer _tracer;

        public UserController(IUserService userService,ITracer tracer)
        {
            _userService = userService;
            _tracer = tracer;
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> Login(LoginCredentialsDTO credentials)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            var token = _userService.Authenticate(credentials);

            if (token == null)
                return StatusCode(401, "Wrong username or password");

  
            scope.Span.Log($"User is logged with: {credentials.Username}");

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
                var actionName = ControllerContext.ActionDescriptor.DisplayName;
                using var scope = _tracer.BuildSpan(actionName).StartActive(true);
                scope.Span.Log($"User is registered with username: {registrationData.Username}");
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
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log($"Hostid: {userId}");

            return Ok(user);
        }

        [HttpGet]
        [Route("get-regular")]
        public ActionResult GetRegular()
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            User user = _userService.GetUser(userId);
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log($"UserId : {userId}");

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
                var actionName = ControllerContext.ActionDescriptor.DisplayName;
                using var scope = _tracer.BuildSpan(actionName).StartActive(true);
                scope.Span.Log($"User {userId} is updated!");
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
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log($"Guest {userId} is deleting");
            return Ok(canBeDeleted);      
        }


        [HttpDelete]
        [AllowAnonymous]
        [Route("deleteAsHost")]
        public async Task<IActionResult> DeleteAsHost()
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);

            bool canBeDeleted = await _userService.DeleteAsHost(userId);

            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log($"Host {userId} is deleting");

            return Ok(canBeDeleted);
            
          
        }
    }
}
