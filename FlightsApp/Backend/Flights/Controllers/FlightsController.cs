using Flights.Model;
using Flights.Service;
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

        [HttpPost]
        public  ActionResult Post(Flight flight)
        {

            _flightsService.Create(flight);
            return CreatedAtAction("Post", flight);

        }
        
        [HttpDelete]
        public ActionResult Delete(String id)
        {
            Flight flight=_flightsService.GetById(id);
            _flightsService.Delete(flight);
            return Ok();
        }
    }

}
