using Microsoft.AspNetCore.Mvc;
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
        [Route("get-similar-users/{id}")]
        public async Task<ActionResult> GetSimilarUsers(string id)
        {
            await _recommendationService.GetSimilarUsers(id);

            return Ok();
        }
    }
}
