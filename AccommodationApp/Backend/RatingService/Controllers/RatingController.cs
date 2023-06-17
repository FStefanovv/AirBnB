using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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

        public RatingController(RatingService.Service.RatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        [Route("rate")]
        public async Task<ActionResult> RateAsync(RatingDTO dto)
        {
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
            RatedEntity entity = await _ratingService.GetRatedEntity(id);

            if (entity != null)
                return Ok(entity);

            return NotFound();   
        }

        
        [HttpGet]
        [Route("get-all-ratings/{id}")]
        public async Task<ActionResult> GetAllRatings(string id)
        {
            List<Rating> ratings = await _ratingService.GetAllEntityRatings(id);

            if (ratings != null && ratings.Count > 0)
                return Ok(ratings);

            return NotFound();
        }

        [HttpDelete]
        [Route("delete-rating/{id}")]
        public async Task<ActionResult> DeleteRating(string id)
        {
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
