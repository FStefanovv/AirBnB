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
using Accommodation.Model;


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

        public List<Model.Accommodation> SearchAccomodation(SearchDTO searchDTO)
        {
            List<Model.Accommodation> allAccomodations = _repository.GetAll();
            List<Model.Accommodation> searchedAccomodations = new List<Model.Accommodation>();
            foreach(Model.Accommodation accommodation in allAccomodations)
            {
                if(accommodation.Address == searchDTO.Location && accommodation.MaxGuests >= searchDTO.numberOfGuests && searchDTO.numberOfGuests >= accommodation.MinGuests)
                {
                    searchedAccomodations.Add(accommodation);
                }
            }
            if(searchedAccomodations != null)
            {
                return searchedAccomodations;
            }
            else
            {
                return null;
            }
            
        }

        public List<Model.Accommodation> GetAll()
        {
            return _repository.GetAll();
        }
       
    }
}
