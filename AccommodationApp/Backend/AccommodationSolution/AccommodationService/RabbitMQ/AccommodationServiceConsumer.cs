using Accommodation.Repository;
using MassTransit;
using MassTransit.Transports;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Users.RabbitMQ;

namespace Accommodation.RabbitMQ
{
    public class AccomodationServiceConsumer:IConsumer<IUserMessage>
    {
        private readonly AccommodationRepository _repository;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        public AccomodationServiceConsumer(AccommodationRepository repository, ISendEndpointProvider sendEndpointProvider)
        {
            _repository = repository;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Consume(ConsumeContext<IUserMessage> context)
        {
            var data = context.Message;
           bool isDeleted=_repository.DeleteAccWithoutHost( data.Id);


            if (isDeleted ==true)
            {
                var endPoint = await _sendEndpointProvider.
                GetSendEndpoint(new Uri("queue:" + BusConstants.EndDeleteQueue));
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
