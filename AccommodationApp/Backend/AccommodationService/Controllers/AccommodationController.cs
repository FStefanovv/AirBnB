using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accommodation.Controllers
{
    [Route("api/accommodation")]
    [ApiController]
    public class AccommodationController : Controller
    {


        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public ActionResult Create()
        {
            return Ok();
        }
    }
}
