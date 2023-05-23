using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using RatingService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class RatingController : ControllerBase
    {

        private readonly RatingService.Service.RatingService _ratingService;

        public RatingController(RatingService.Service.RatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        [Route("rate")]
        public ActionResult Rate(RatingDTO dto)
        {
            Request.Headers.TryGetValue("Username", out StringValues username);
            try
            {
                _ratingService.Create(dto, username);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
