using Accommodation.DTO;
using Accommodation.Services;
using Jaeger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly ITracer _tracer;

        public AccommodationController(AccommodationService service, ITracer tracer)
        {
            _accommodationService = service;
            _tracer = tracer;
        }


        [HttpPost]
        [Route("create")]
        [Consumes("multipart/form-data")]
        public ActionResult Create()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            Request.Headers.TryGetValue("HostId", out StringValues hostId);

            var uploadedPhotos = Request.Form.Files.ToList();

            var accommodationData = Request.Form["accomm-data"].ToList();
            CreateAccommodationDTO dto = JsonConvert.DeserializeObject<CreateAccommodationDTO>(accommodationData[0]);

            _accommodationService.Create(dto, hostId, uploadedPhotos);

            return Ok();
        }


        [HttpGet]
        [Route("get-photos/{id}")]
        public async Task<List<byte[]>> GetAccommPhotos(string id)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            return await _accommodationService.GetAccommodationPhotos(id);
        }

      

        [HttpGet]
        [Route("get-by-id/{id}")]
        public ActionResult GetById(string id)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            var accommodation = _accommodationService.GetById(id);
            var accomodationDTO=Adapters.CreateAccommodationAdapter.ObjectToAccommodationDTO(accommodation);

            return Ok(accomodationDTO);
        }



       [HttpPost]
       [Route("update")]
       public async Task<IActionResult> Update(UpdateAccommodationDTO updateAccommodationDTO)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            bool canBeUpdate= await _accommodationService.Update(updateAccommodationDTO);

            return Ok(canBeUpdate);
        }



        [HttpPost]
        [Route("get-searched")]
        public async Task<ActionResult> GetSearchedAccomodations(SearchDTO dto)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            Task<List<Model.Accommodation>> searchedAccomodationsTask = _accommodationService.SearchAccomodation(dto);
            List<Model.Accommodation> searchedAccomodations = await searchedAccomodationsTask;
            List<AccommodationDTO> searhedAccomodationDTOs = new List<AccommodationDTO>();
            foreach (Model.Accommodation accomm in searchedAccomodations)
            {
                searhedAccomodationDTOs.Add(Adapters.CreateAccommodationAdapter.ObjectToAccommodationDTOForSearch(accomm));
            }

            scope.Span.Log($"Searched accommodations: {searhedAccomodationDTOs}");
            return Ok(searhedAccomodationDTOs);
        }

        [HttpGet]
        [Route("get-all")]
        public ActionResult GetAll()
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            List<Model.Accommodation> accommodations = _accommodationService.GetAll();
            List<AccommodationDTO> accomodationDTOs = new List<AccommodationDTO>();
            foreach(Model.Accommodation accomm in accommodations)
            {
                accomodationDTOs.Add(Adapters.CreateAccommodationAdapter.ObjectToAccommodationDTOForSearch(accomm));
            }
            return Ok(accomodationDTOs);
        }

    }
}
