using Flights.DTOs;
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

        public void Create(Flight flight)
        {
            _flightsRepository.Create(flight);
        }

        public void Delete(Flight flight)
        {
            _flightsRepository.Delete(flight);
        }
        public Flight GetById(String id)
        {
          return  _flightsRepository.GetById(id);
        }

        public List<Flight> GetSearchedFlights(string departurePoint, string arrivalPoint, int numberOfPassenger, string dateOfDeparture)
        {
            return _flightsRepository.GetSearched(departurePoint,arrivalPoint,numberOfPassenger,dateOfDeparture);
        }
    }
}
