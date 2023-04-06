using Flights.Repository;
using Flights.Service;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Flights.BackgroundTasks
{
    public class UpdateFlightStatus : BackgroundService
    {
        private readonly FlightsService _flightsService;

        public UpdateFlightStatus(FlightsService flightsService)
        {
           _flightsService = flightsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _flightsService.UpdatePastFlights();
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
