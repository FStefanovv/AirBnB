using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ReservationService.RabbitMQ;


//using ReservationService.BackgroundServices;
using ReservationService.Repository;
using ReservationService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService
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
                cfg.AddConsumer<ReservationServiceConsumer>();

                cfg.AddBus(provider => RabbitMQBus.ConfigureBus(provider, (cfg, host) =>
                {
                    cfg.ReceiveEndpoint(BusConstants.StartDeleteQueue, ep =>
                    {
                        ep.ConfigureConsumer<ReservationServiceConsumer>(provider);
                    });
                }));
            });

          
            services.AddMassTransitHostedService();
            services.AddCors();

            services.AddDbContext<PostgresDbContext>(opts =>
                opts.UseNpgsql(Configuration.GetConnectionString("PostgresDatabaseConnectionString")));

            //services.AddHostedService<UpdateReservationStatus>();

            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IReservationService, Service.ReservationService>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IRequestService, Service.RequestService>();
            services.AddScoped<ReservationServiceConsumer>();

            services.AddGrpc();

            services.AddControllers();

          
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReservationService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReservationService v1"));
            }

            app.UseHttpsRedirection();

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
                endpoints.MapGrpcService<ReservationService.Service.ReservationService>();
               // endpoints.MapGrpcService<ReservationService.Service.RequestService>();
            });
        }
    }
}
