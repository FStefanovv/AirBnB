﻿using Microsoft.AspNetCore.Http;
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
using Grpc.Net.Client;
using OpenTracing;
using System.Net;

namespace ReservationService.Controllers
{
    [Route("api/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IRequestService _requestService;
        private readonly ITracer _tracer;

        public ReservationController(IReservationService reservationService, IRequestService requestService, ITracer tracer)
        {
            _reservationService = reservationService;
            _requestService = requestService;
            _tracer = tracer;
        }

        [HttpPut]
        [Route("cancel-reservation/{id}/{hostId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CancelReservation(string id,string hostId)
        {
            try
            {
                var actionName = ControllerContext.ActionDescriptor.DisplayName;
                using var scope = _tracer.BuildSpan(actionName).StartActive(true);

                Request.Headers.TryGetValue("UserId", out StringValues userId);
                Request.Headers.TryGetValue("Username", out StringValues username);

                await _reservationService.CancelReservation(id, userId, username);
                //Task<bool> checkStatus = _reservationService.CheckHostStatus(hostId);
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
                var actionName = ControllerContext.ActionDescriptor.DisplayName;
                using var scope = _tracer.BuildSpan(actionName).StartActive(true);
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
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            List<ShowReservationDTO> reservations = _reservationService.GetUserReservations(userId);

            return Ok(reservations);
        }


        [HttpGet]
        [Route("get-pending-requests")]
        public ActionResult GetPendingRequests()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            List<ReservationRequest> requests = _requestService.GetPendingRequestsByHost(userId);

            return Ok(requests);
        }

        [HttpGet]
        [Route("get-resolved-requests")]
        public ActionResult GetHostReservations()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            List<ReservationRequest> requests = _requestService.GetResolvedRequestsByHost(userId);

            return Ok(requests);
        }

        [HttpPost]
        [Route("get-cost")]
        public async Task<ActionResult> GetCost(ReservationCostDTO dto)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            double cost= await _reservationService.GetCost(dto);

            return Ok(cost);

        }
     
        [HttpGet]
        [Route("can-rate/{accommId}/{hostId}")]
        public ActionResult CanRate(string accommId, string hostId)
        {
            Request.Headers.TryGetValue("UserId", out StringValues userId);

            CanRateDTO canRate = _reservationService.CheckIfUserCanRate(userId, hostId, accommId);

            return Ok(canRate);
        }

        [HttpGet]
        [Route("get-reserved-start-dates/{accommodationId}")]
        public ActionResult GetReservedStartDates(string accommodationId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            var startDates = Adapter.DatesAdapter.ObjectToStartDateDTO(_reservationService.GetStartReservationDate(accommodationId));
         
            return Ok(startDates);
        }


        [HttpGet]
        [Route("get-reserved-end-dates/{accommodationId}")]
        public ActionResult GetReservedEndDates(string accommodationId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            var endDates = Adapter.DatesAdapter.ObjectToEndDateDTO(_reservationService.GetEndReservationDate(accommodationId));

            return Ok(endDates);
        }

        [HttpGet]
        [Route("get-busy-dates-for-accommodation/{accommodationId}")]
        public ActionResult GetReservationDatesForAccommodation(string accommodationId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            var dates = _reservationService.GetBusyDatesForAccommodation(accommodationId);
            return Ok(dates);
        }

        [HttpGet]
        [Route("get-reservation/{id}")]
        public ActionResult GetReservation(string id)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            return Ok(_reservationService.GetEndReservation(id));
        }
    }
}
