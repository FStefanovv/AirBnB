using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ReservationService.Adapter;
using ReservationService.DTO;
using ReservationService.Model;
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
        private readonly IReservationService _service;

        public ReservationController(IReservationService service)
        {
            _service = service;
        }

        [HttpPut]
        [Route("cancel-reservation/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult CancelReservation(string id)
        {
            try
            {
                Request.Headers.TryGetValue("UserId", out StringValues userId);
                _service.CancelReservation(id, userId);

                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
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
                _service.CancelReservationRequest(id, userId);

                return StatusCode(200);
            }
            catch
            {
                return StatusCode(400);
            }
        }

        [HttpGet]
        [Route("get-user-reservations")]
        public ActionResult GetUserReservations()
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            List<Reservation> reservations = _service.GetUserReservations(userId);

            //List<ReservationDTO> reservationDTOs = ReservationAdapter.ReservationsToDto(reservations);

            return Ok(reservations);
        }
       

    }
}
