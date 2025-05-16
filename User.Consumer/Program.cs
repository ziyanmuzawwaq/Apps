using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using User.Application.Interfaces;
using User.Application.Services;
using User.Consumer;
using User.Domain.Interfaces;
using User.Infrastructure.Data;
using User.Infrastructure.Repositories;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IEventJobService, EventJobService>();

builder.Services.AddScoped<IDonationRepository, DonationRepository>();
builder.Services.AddScoped<IEventJobRepository, EventJobRepository>();

builder.Services.AddHostedService<EventConsumerJob>();

builder.Services.AddMemoryCache();

builder.Build().Run();