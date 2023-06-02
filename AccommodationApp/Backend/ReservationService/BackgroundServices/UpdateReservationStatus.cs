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
        private IReservationService _reservationService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UpdateReservationStatus(IReservationService reservationService, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedService = scope.ServiceProvider.GetRequiredService<IReservationService>();

                     _reservationService.UpdatePastReservations();

                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

    }
}
