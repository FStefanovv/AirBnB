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
        [Route("rate/{hostId}/{entityName}")]
        public async Task<ActionResult> RateAsync(CreateRatingDTO dto, string hostId, string entityName)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            Request.Headers.TryGetValue("Username", out StringValues username);
            Request.Headers.TryGetValue("UserId", out StringValues userId);

            try
            {
                await _ratingService.CreateAsync(dto, username, userId, hostId, entityName);
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
            RatedEntity entity = await _ratingService.GetRatedEntity(id);

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
            
            List<RatingWithUsernameDTO> ratings = await _ratingService.GetAllEntityRatingsWithUsername(id);

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

        [HttpGet]
        [Route("get-user-rating/{id}")]
        public async Task<ActionResult<Rating>> GetUsersRating(string id)
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            try
            {
                Rating rating = await _ratingService.GetUsersRating(id, userId);
                return Ok(rating);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-ratings-for-page/{accommId}/{hostId}")]
        public async Task<ActionResult<List<RatingDTO>>> GetRatingsForPage(string accommId, string hostId)
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);

            List<RatingDTO> ratingDtos = await _ratingService.GetPageRatings(userId, accommId, hostId);

            return Ok(ratingDtos);
        }

    }
}
