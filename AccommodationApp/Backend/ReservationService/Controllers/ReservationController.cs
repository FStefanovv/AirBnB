using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using ReservationService.Adapter;
using ReservationService.DTO;
using ReservationService.Model;
using ReservationService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReservationService.Controllers
{
    [Route("api/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IRequestService _requestService;

        public ReservationController(IReservationService reservationService, IRequestService requestService)
        {
            _reservationService = reservationService;
            _requestService = requestService;
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
                _reservationService.CancelReservation(id, userId);

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
                _requestService.CancelReservationRequest(id, userId);

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
            List<Reservation> reservations = _reservationService.GetUserReservations(userId);

            
            return Ok(reservations);
        }

        [HttpGet]
        [Route("get-pending-requests")]
        public ActionResult GetPendingRequests()
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            List<ReservationRequest> requests = _requestService.GetPendingRequestsByHost(userId);

            return Ok(requests);
        }

        [HttpGet]
        [Route("get-resolved-requests")]
        public ActionResult GetHostReservations()
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            List<ReservationRequest> requests = _requestService.GetResolvedRequestsByHost(userId);

            return Ok(requests);
        }

        [HttpGet]
        [Route("get-status/{id}")]
        public bool GuestHasActiveReservations(string id)
        {
            
            bool requests = _reservationService.GuestHasActiveReservations(id);

            return requests;
        }

        [HttpGet]
        [Route("get-status-host/{id}")]
        public bool HostHasActiveReservations(string id)
        {

            bool requests = _reservationService.HostHasActiveReservations(id);

            return requests;
        }

        [HttpPost]
        [Route("create-reservation")]

        public async Task<ActionResult> CreateReservation(ReservationDTO dto)
        {

            Request.Headers.TryGetValue("UserId", out StringValues userId);

            Reservation reservation= Adapter.ReservationAdapter.CreateReservationDtoToObject(dto, userId);

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:5002/api/accommodation/get-by-id/" + reservation.AccommodationId);
            Console.WriteLine("Status: " + response.StatusCode.ToString());
            string jsonContent = response.Content.ReadAsStringAsync().Result;
            DTO.AccommodationDTO result = JsonConvert.DeserializeObject<DTO.AccommodationDTO>(jsonContent);

            _reservationService.CreateReservation(reservation, result);

            return Ok();

        }


        [HttpGet]
        [Route("get-reserved-start-dates/{accommodationId}")]
        public ActionResult GetReservedStartDates(string accommodationId)
        {
            var startDates = Adapter.DatesAdapter.ObjectToStartDateDTO(_reservationService.GetStartReservationDate(accommodationId));
         
            return Ok(startDates);
        }


        [HttpGet]
        [Route("get-reserved-end-dates/{accommodationId}")]
        public ActionResult GetReservedEndDates(string accommodationId)
        {
            var endDates = Adapter.DatesAdapter.ObjectToEndDateDTO(_reservationService.GetEndReservationDate(accommodationId));

            return Ok(endDates);
        }




    }
}
