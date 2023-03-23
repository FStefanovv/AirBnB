using Flights.Model;
using Flights.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights.Service
{
    public class FlightsService
    {
        private readonly FlightsRepository _flightsRepository;

        public FlightsService(FlightsRepository flightsRepository)
        {
            _flightsRepository = flightsRepository;
        }

        public List<Flight> GetAll()
        {
            return _flightsRepository.GetAll();
        }
    }
}
