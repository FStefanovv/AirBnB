using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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
        public ActionResult Create()
        {
            const string HeaderKeyName = "HostUsername";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues hostUsername);

            Console.WriteLine(hostUsername);
            

            return Ok();
        }
    }
}
