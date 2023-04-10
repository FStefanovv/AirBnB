using Accommodation.DTO;
using Accommodation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Accommodation.Controllers
{
    [Route("api/accommodation")]
    [ApiController]
    public class AccommodationController : Controller
    {
        private readonly AccommodationService _accommodationService;
        public AccommodationController(AccommodationService service)
        {
            _accommodationService = service;
        }


        [HttpPost]
        [Route("create")]
        public ActionResult Create()
        {
            Request.Headers.TryGetValue("HostUsername", out StringValues hostUsername);
            Request.Headers.TryGetValue("HostId", out StringValues hostId);

            var uploadedPhotos = Request.Form.Files.ToList();

            var accommodationData = Request.Form["accommodationData"].ToList();
            CreateAccommodationDTO dto = JsonConvert.DeserializeObject<CreateAccommodationDTO>(accommodationData[0]);

            _accommodationService.Create(dto, hostUsername, uploadedPhotos);

            return Ok();
        }

        [HttpGet]
        [Route("get-photos/{id}")]
        public async Task<List<IFormFile>> GetAccommPhotos(string id)
        {

            return await _accommodationService.GetAccommodationPhotos(id);
        }

    }
}
