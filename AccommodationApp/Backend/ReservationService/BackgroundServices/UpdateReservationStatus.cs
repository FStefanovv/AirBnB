using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationService.Service;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace ReservationService.BackgroundServices
{
    public class UpdateReservationStatus : BackgroundService
    {
        private readonly IReservationService _reservationService;

        public UpdateReservationStatus(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _reservationService.UpdatePastReservations();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

    }
}
