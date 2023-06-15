using System.Collections.Generic;
using Flights.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Flights.DTOs;
using Flights.ApiKeyAuth;
using System;
using Microsoft.Extensions.Primitives;

namespace Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        //ovo zameni
        private readonly TicketsService _ticketsService;
        private readonly FlightsService _flightsService;

        public TicketsController(TicketsService ticketsService,FlightsService flightsService)
        {
            _ticketsService = ticketsService;
            _flightsService = flightsService;
        }

        [Authorize(Policy = "LoggedInUser")]
        [HttpPost]
        public ActionResult BuyTicketRegular(BuyTicketDTO dto)
        {

            _ticketsService.BuyTicket(dto);
            return CreatedAtAction("Post", dto);
        }

        [ApiKey]
        [HttpPost]
        [Route("buy-with-api-key")]
        public ActionResult BuyWithApiKey(BuyWithApiKeyDTO dto)
        {   
            try
            {
                Request.Headers.TryGetValue("Api-Key", out StringValues apiKey);

                _ticketsService.BuyWithApiKey(dto, apiKey);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [Authorize(Policy = "LoggedInUser")]
        [HttpGet]
        public ActionResult<List<ViewTicketDTO>> GetTicketsForUser(string userId)
        {
            
            return _ticketsService.GetTicketsForUser(userId);
        }
    }

}
