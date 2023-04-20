using Microsoft.AspNetCore.Mvc;
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
    }
}
