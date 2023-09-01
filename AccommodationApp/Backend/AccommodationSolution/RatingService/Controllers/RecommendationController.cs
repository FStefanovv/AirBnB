using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using RatingService.Model;
using RatingService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Controllers
{
    [Route("api/accommodation-recommendation")]
    [ApiController]
    public class RecommendationController : Controller
    {
        private readonly RecommendationService _recommendationService;

        public RecommendationController(RecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }


        [HttpGet]
        [Route("get-recommendations")]
        public async Task<ActionResult> GetRecommendations()
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);

            List<Recommendation> recommendations = await _recommendationService.GetRecommendationsFor(userId);



            return Ok(recommendations);
        }

       
    }
}
