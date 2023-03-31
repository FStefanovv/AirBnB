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
        [Route("[action]")]
        public ActionResult<List<Flight>> GetSearchedFlights(string departurePoint,string arrivalPoint,int numberOfPassenger,string dateOfDeparture)
        {
            return _flightsService.GetSearchedFlights(departurePoint,arrivalPoint,numberOfPassenger,dateOfDeparture);
        }


        [HttpPost]
        [Authorize(Policy = "Admin")]
        public  ActionResult Post(NewFlightDTO dto)
        {
            _flightsService.Create(dto);
            return CreatedAtAction("Post", dto);
        }

        
        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(String id)
        {
            Flight flight = _flightsService.GetById(id);
            _flightsService.Delete(flight);
            return Ok();
        }
    }

}
