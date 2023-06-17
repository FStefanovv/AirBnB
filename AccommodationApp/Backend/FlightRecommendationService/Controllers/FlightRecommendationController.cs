using FlightRecommendationService.DTO;
using FlightRecommendationService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightRecommendationService.Controllers
{
    [Route("api/flight-recommendation")]
    [ApiController]
    public class FlightRecommendationController : Controller
    {
        private readonly FlightRecommendationService.Service.FlightRecommendationService _service;

        public FlightRecommendationController(FlightRecommendationService.Service.FlightRecommendationService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("get-recommendations")]    
        public async Task<ActionResult> GetFlightRecommendations(FlightRequirements requirements)
        {
            try
            {
                List<FlightRecommendation> recommendations = await _service.GetRecommendationsFor(requirements);

                return Ok(recommendations);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("purchase-tickets")]
        public async Task<ActionResult> PurchaseTicket(TicketPurchaseDTO dto)
        {
            Request.Headers.TryGetValue("Email", out StringValues email);

            try
            {
                await _service.PurchaseTicket(dto, email);
                return Ok(dto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
