using System.Collections.Generic;
using Flights.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Flights.DTOs;

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
        public ActionResult Post(BuyTicketDTO dto)
        {

            _ticketsService.BuyTicket(dto);
            return CreatedAtAction("Post", dto);

        }
        
        [Authorize(Policy = "LoggedInUser")]
        [HttpGet]
        public ActionResult<List<ViewTicketDTO>> GetTicketsForUser(string userId)
        {
            
            return _ticketsService.GetTicketsForUser(userId);
        }
    }

}
