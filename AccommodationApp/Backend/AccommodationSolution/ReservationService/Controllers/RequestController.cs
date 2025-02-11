﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OpenTracing;
using ReservationService.DTO;
using ReservationService.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationService.Controllers
{
    [Route("api/request")]
    [ApiController]
    public class RequestController:ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IReservationService _reservationService;
        private readonly ITracer _tracer;

        public RequestController(IRequestService requestService,IReservationService reservationService, ITracer tracer)
        {       
            _requestService = requestService;
            _reservationService = reservationService;
            _tracer = tracer;
        }

        [HttpDelete]
        [Route("update-request/{id}")]
        public ActionResult UpdateRequestsPostUserDeletion(string id)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            //_requestService.UpdateRequestsPostUserDeletion(id);
            return Ok();
        }

        [HttpPost]
        [Route("create-request")]
        public async Task<ActionResult> CreateRequest(RequestReservationDTO dto)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);

            Request.Headers.TryGetValue("Username", out StringValues username);


            await _requestService.CreateReservationRequestOrReservation(dto, username);
            return Ok();
        }

        [HttpPost]
        [Route("accept-request/{requestId}/{accommodationId}")]
        public async Task<ActionResult> AcceptRequest(string requestId, string accommodationId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            await _requestService.AcceptRequest(requestId, accommodationId);
            return Ok();
        }

        [HttpGet]
        [Route("get-requests/{hostId}")]
        public ActionResult GetRequests(string hostId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            var list = _requestService.GetRequestsForHost(hostId);
            return Ok(list);
        }

        [HttpGet]
        [Route("get-user-requests")]
        public ActionResult GetRequestsForUser()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            Request.Headers.TryGetValue("UserId", out StringValues userId);
            List<ShowRequestDTO> list = _requestService.GetRequestsForUsers(userId);
            return Ok(list);
        }

    }
}
