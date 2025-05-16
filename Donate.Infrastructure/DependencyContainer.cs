using Donate.Application.Interfaces;
using Donate.Application.Services;
using Donate.Domain.Interfaces;
using Donate.Domain.Interfaces.Messaging;
using Donate.Infrastructure.Messaging;
using Donate.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Contracts;

namespace Donate.Infrastructure
{
    public static class DependencyContainer
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            Contract.Assert(configuration != null);

            services.AddScoped<IDonationRepository, DonationRepository>();

            services.AddScoped<IDonationService, DonationService>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(Application.AssemblyReference).Assembly);
            });

            var kafkaBootstrapServers = configuration.GetSection("Kafka")["BootstrapServers"];

            services.AddSingleton<IMessageProducer>(sp =>
                new KafkaProducer(
                    sp.GetRequiredService<ILogger<KafkaProducer>>(),
                    kafkaBootstrapServers ?? "localhost:9092"));

            return services;
        }
    }
}