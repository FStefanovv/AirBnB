using Flights.DTOs;
using Flights.Model;
using Flights.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly FlightsService _flightsService;

        public FlightsController(FlightsService flightsService)
        {
            _flightsService = flightsService;
        }

        [HttpGet]
        public ActionResult<List<Flight>> GetAll()
        {
            return _flightsService.GetAll();
        }

        [HttpGet]
        public ActionResult<List<Flight>> GetSearchedFlights(SearchFlightsDTO flightDTO)
        {
            return _flightsService.GetSearchedFlights(flightDTO);
        }


        [Authorize(Policy = "ADMIN")]
        [HttpPost]
        public  ActionResult Post(Flight flight)
        {

            _flightsService.Create(flight);
            return CreatedAtAction("Post", flight);

        }

        [Authorize(Policy = "ADMIN")]
        [HttpDelete]
        public ActionResult Delete(String id)
        {
            Flight flight=_flightsService.GetById(id);
            _flightsService.Delete(flight);
            return Ok();
        }
    }

}
