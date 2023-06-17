using MassTransit;
using MassTransit.Transports;
using Microsoft.Extensions.Logging;
using ReservationService.Model;
using ReservationService.Repository;
using ReservationService.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Users.RabbitMQ;

namespace ReservationService.RabbitMQ
{
    public class ReservationServiceConsumer:IConsumer<IUserMessage>
    {
        private readonly IReservationRepository _repository;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        public ReservationServiceConsumer(IReservationRepository repository, ISendEndpointProvider sendEndpointProvider)
        {
            _repository = repository;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Consume(ConsumeContext<IUserMessage> context)
        {
            var data = context.Message;
            List<Reservation> activeReservations= _repository.GetActiveHostReservations(data.Id);


            if (activeReservations.Count ==0)
            {
                var endPoint = await _sendEndpointProvider.
                GetSendEndpoint(new Uri("queue:" + BusConstants.StartDeleteAccommodation));
                await endPoint.Send<IUserMessage>(new { Id = data.Id });
            }
            else
            {
                var endPoint = await _sendEndpointProvider.
                GetSendEndpoint(new Uri("queue:" + BusConstants.CancelDeleteQueue));
                await endPoint.Send<IUserMessage>(new { Id = data.Id });
            }
            Console.WriteLine(data.Id);
        }
    }
   
}
