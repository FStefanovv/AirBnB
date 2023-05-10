using Microsoft.AspNetCore.Mvc;
using ReservationService.DTO;
using ReservationService.Service;

namespace ReservationService.Controllers
{
    [Route("api/request")]
    [ApiController]
    public class RequestController:ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {       
            _requestService = requestService;
        }
        [HttpDelete]
        [Route("update-request/{id}")]
        public ActionResult UpdateRequestsPostUserDeletion(string id)
        {
            _requestService.UpdateRequestsPostUserDeletion(id);
            return Ok();
        }

        [HttpPost]
        [Route("create-request")]
        public ActionResult CreateRequest(RequestReservationDTO dto)
        {
            _requestService.CreateReserervationRequest(dto);
            return Ok();
        }
    }
}
