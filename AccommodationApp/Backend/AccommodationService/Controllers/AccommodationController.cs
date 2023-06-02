using Accommodation.DTO;
using Accommodation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
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
        public AccommodationController(AccommodationService service)
        {
            _accommodationService = service;
        }


        [HttpPost]
        [Route("create")]
        [Consumes("multipart/form-data")]
        public ActionResult Create()
        {
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

            return await _accommodationService.GetAccommodationPhotos(id);
        }

      

        [HttpGet]
        [Route("get-by-id/{id}")]
        public ActionResult GetById(string id)
        {
            var accommodation = _accommodationService.GetById(id);
            var accomodationDTO=Adapters.CreateAccommodationAdapter.ObjectToAccommodationDTO(accommodation);

            return Ok(accomodationDTO);
        }



       [HttpPost]
       [Route("update-acc")]
       public ActionResult Update(UpdateAccommodationDTO updateAccommodationDTO)
        {           
           _accommodationService.Update(updateAccommodationDTO);

            return Ok();
        }



        [HttpPost]
        [Route("get-searched")]
        public ActionResult GetSearchedAccomodations(SearchDTO dto)
        {
            List<Model.Accommodation> searchedAccomodations = _accommodationService.SearchAccomodation(dto);
            return Ok(searchedAccomodations);
        }

        [HttpGet]
        [Route("get-all")]
        public ActionResult GetAll()
        {
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
