using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationsService.Model;
using Microsoft.Extensions.Primitives;

namespace NotificationsService.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private NotificationsService.Service.NotificationService _service;
        public NotificationsController(NotificationsService.Service.NotificationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("all-by-user")]
        public ActionResult GetNotifications()
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            List<Notification> userNotifications = _service.GetNotifications(userId);


            return Ok(userNotifications);
        }


    }
}
