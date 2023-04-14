using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ReservationService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.Controllers
{
    [Route("api/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public ReservationController(IRequestService requestService)
        {
            _requestService = requestService;
        }


        [HttpPut]
        [Route("cancel-reservation-request/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult CancelRequest(string id)
        {
            try
            {
                Request.Headers.TryGetValue("UserId", out StringValues userId);
                _requestService.CancelReservationRequest(id, userId);

                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }
        
    }
}
