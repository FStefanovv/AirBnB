using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accommodation.Model;
using MongoDB.Bson;
using System.IO;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver.GridFS;
using Microsoft.Extensions.Hosting;

namespace Accommodation.Repository
{
    public class AccommodationRepository
    {
        private readonly IDbContext _context;
        private IMongoCollection<Model.Accommodation> _accommodation;

        public AccommodationRepository(IDbContext context)
        {
            _context = context;
            _accommodation = _context.GetCollection<Model.Accommodation>("accommodation");
        }

        
        public void Create(Model.Accommodation accommodation, List<IFormFile> photos)
        {
            _accommodation.InsertOne(accommodation);
            foreach (IFormFile photo in photos)
            {
                _context.UploadImage(photo, accommodation.Id);
            }
        }

        public async Task<List<byte[]>> GetAccommodationPhotos(string accommId)
        {
            return await _context.GetAccommodationPhotos(accommId);
        }

        public bool DeleteAccWithoutHost(string id)
        {
            try {
                var filter = Builders<Model.Accommodation>.Filter.Eq("HostId", id);
                _accommodation.DeleteMany(filter);
                return true;
            }
            catch(Exception ex) { return false; }
            

        }


        public List<Model.Accommodation> GetByHostId(string hostId)
        {
            return _accommodation.Find(accomodation => accomodation.HostId == hostId).ToList();
        }

        public Model.Accommodation GetById(string id)
        {
            return _accommodation.Find(Builders<Model.Accommodation>.Filter.Eq("_id", ObjectId.Parse(id))).FirstOrDefault();

        }

        public List<Model.Accommodation> GetAll()
        {
            return _accommodation.Find(accomodation => true).ToList();
        }
      
        /*
        public AccommodationGRPC GetByIdGRPC(AccommodationId id)
        {
            Model.Accommodation accommodation = GetById(id.Id);

            return new AccommodationGRPC
                {
                    Name = accommodation.Name
                };
        }*/

        public void Update(Model.Accommodation accommodation)
        {

           _accommodation.ReplaceOne(acc => acc.Id==accommodation.Id,accommodation);


        }

    }
}
