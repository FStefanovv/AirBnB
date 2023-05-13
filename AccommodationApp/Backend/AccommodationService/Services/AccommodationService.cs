using Accommodation.Adapters;
using Accommodation.DTO;
using Accommodation.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Accommodation.Services
{
    public class AccommodationService
    {
        private readonly AccommodationRepository _repository;

        public AccommodationService(AccommodationRepository repository)
        {
            _repository = repository;
        }

        public void Create(CreateAccommodationDTO dto, StringValues hostId, List<IFormFile> photos)
        {
            Model.Accommodation accommodation = CreateAccommodationAdapter.CreateAccommodaitonDtoToObject(dto, hostId);

            _repository.Create(accommodation, photos);
        }

        public async Task<List<IFormFile>> GetAccommodationPhotos(string accommId)
        {
            return await _repository.GetAccommodationPhotos(accommId);
        }

        public void DeleteAccwithoutHost(string id)
        {
           
            _repository.DeleteAccWithoutHost(id);
        }
        
        public Model.Accommodation GetById(string id)
        {
            return _repository.GetById(id);
        }
       
    }
}
