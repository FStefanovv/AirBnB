
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTracing;
using OpenTracing.Contrib.NetCore.Configuration;
using OpenTracing.Util;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Users.RabbitMQ;
using Users.Repository;
using Users.Services;
using Prometheus;

namespace Users
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
            services.AddCors();
            services.AddGrpc();

            //services.AddSingleton<IDbContext, MongoDbContext>();

            services.AddDbContext<PostgresDbContext>(opts =>
                opts.UseNpgsql(Configuration.GetConnectionString("PostgresDatabaseConnectionString")));

            services.AddScoped<IUserRepository, UserRepositoryPostgres>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<CancelDeleteConsumer>();
            services.AddScoped<EndDeleteConsumer>();
            services.AddControllers();

            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<CancelDeleteConsumer>();
                cfg.AddConsumer<EndDeleteConsumer>();

                cfg.AddBus(provider => RabbitMQBus.ConfigureBus(provider, (cfg, host) =>
                {
                    cfg.ReceiveEndpoint(BusConstants.CancelDeleteQueue, ep =>
                    {
                        ep.ConfigureConsumer<CancelDeleteConsumer>(provider);
                    });
                    cfg.ReceiveEndpoint(BusConstants.EndDeleteQueue, ep =>
                    {
                        ep.ConfigureConsumer<EndDeleteConsumer>(provider);
                    });
                }));

            });

            services.AddMassTransitHostedService();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserService", Version = "v1" });
            });

            services.AddOpenTracing();
            services.AddSingleton<ITracer>(sp =>
            {
                var serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(loggerFactory)
                    .WithSender(new UdpSender("jaeger", 6831, 0))
                    .Build();
                var tracer = new Tracer.Builder(serviceName)
                    // The constant sampler reports every span.
                    .WithSampler(new ConstSampler(true))
                    // LoggingReporter prints every reported span to the logging framework.
                    .WithReporter(reporter)
                    .Build();

                return tracer;
            });

            services.Configure<HttpHandlerDiagnosticOptions>(options =>
                options.OperationNameResolver =
                    request => $"{request.Method.Method}: {request?.RequestUri?.AbsoluteUri}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserService v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });


            app.UseMetricServer();
            app.UseHttpMetrics();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<UserService>();
            });
        }
    }
}
