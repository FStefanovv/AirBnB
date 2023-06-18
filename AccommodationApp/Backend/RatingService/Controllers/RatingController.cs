using Jaeger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OpenTracing;
using RatingService.DTO;
using RatingService.Model;
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
        private readonly ITracer _tracer;

        public RatingController(RatingService.Service.RatingService ratingService, ITracer tracer)
        {
            _ratingService = ratingService;
            _tracer = tracer;
        }

        [HttpPost]
        [Route("rate")]
        public async Task<ActionResult> RateAsync(RatingDTO dto)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            Request.Headers.TryGetValue("Username", out StringValues username);
            Request.Headers.TryGetValue("UserId", out StringValues userId);

            try
            {
                await _ratingService.CreateAsync(dto, username, userId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

        [HttpGet]
        [Route("get-average-rating/{id}")]
        public async Task<ActionResult> GetAverageRating(string id)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            RatedEntity entity = _ratingService.GetRatedEntity(id);

            if (entity != null)
                return Ok(entity);

            return NotFound();   
        }

        
        [HttpGet]
        [Route("get-all-ratings/{id}")]
        public async Task<ActionResult> GetAllRatings(string id)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            
            List<Rating> ratings = _ratingService.GetAllEntityRatings(id);

            if (ratings != null && ratings.Count > 0)
                return Ok(ratings);

            return NotFound();
        }

        [HttpDelete]
        [Route("delete-rating/{id}")]
        public async Task<ActionResult> DeleteRating(string id)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            try
            {
                await _ratingService.DeleteRating(id, userId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
