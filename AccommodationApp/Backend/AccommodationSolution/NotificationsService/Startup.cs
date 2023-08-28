using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NotificationsService.RabbitMQ;
using NotificationsService.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationsService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<NotificationsService.Service.NotificationService>();

                cfg.AddBus(provider => RabbitMQBus.ConfigureBus(provider, (cfg, host) =>
                {
                    cfg.ReceiveEndpoint(BusConstants.NotificationQueue, ep =>
                    {
                        ep.ConfigureConsumer<NotificationsService.Service.NotificationService>(provider);
                    });
                }));
            });


            services.AddMassTransitHostedService();


            services.AddGrpc();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NotificationsService", Version = "v1" });
            });

            services.AddSingleton<IDbContext, MongoDbContext>();
            services.AddSingleton<NotificationsService.Repository.NotificationRepository>();
            services.AddSingleton<NotificationsService.Service.NotificationService>();
            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NotificationsService v1"));
            }

            app.UseWebSockets();

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<NotificationsService.Service.NotificationService>();
                endpoints.MapHub<NotificationHub>("/notifications");
            });
        }
    }
}
