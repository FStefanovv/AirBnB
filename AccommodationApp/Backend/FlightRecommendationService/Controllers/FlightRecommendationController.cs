using FlightRecommendationService.DTO;
using FlightRecommendationService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OpenTracing;
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
        private readonly ITracer _tracer;

        public FlightRecommendationController(FlightRecommendationService.Service.FlightRecommendationService service, ITracer tracer)
        {
            _service = service;
            _tracer = tracer;
        }

        [HttpPost]
        [Route("get-recommendations")]
        public async Task<ActionResult> GetFlightRecommendations(FlightRequirements requirements)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
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
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            Request.Headers.TryGetValue("Email", out StringValues email);

            try
            {
                await _service.PurchaseTicket(dto, email);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}
