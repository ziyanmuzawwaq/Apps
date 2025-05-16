using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Contracts;
using User.Application.Interfaces;
using User.Application.Services;
using User.Domain.Interfaces;
using User.Domain.Interfaces.Messaging;
using User.Infrastructure.Messaging;
using User.Infrastructure.Repositories;

namespace User.Infrastructure
{
    public static class DependencyContainer
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            Contract.Assert(configuration != null);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDonationService, DonationService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDonationRepository, DonationRepository>();

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