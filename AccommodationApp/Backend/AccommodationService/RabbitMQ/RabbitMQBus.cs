using MassTransit.RabbitMqTransport;
using MassTransit;
using System;

namespace Accommodation.RabbitMQ
{ 
    public class RabbitMQBus
    {
        public static IBusControl ConfigureBus(IServiceProvider provider, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost>
        registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(BusConstants.RabbitMqUri), hst =>
                {
                    hst.Username(BusConstants.UserName);
                    hst.Password(BusConstants.Password);
                });

                cfg.ConfigureEndpoints(provider);

                registrationAction?.Invoke(cfg, host);
            });
        }

    }
}
