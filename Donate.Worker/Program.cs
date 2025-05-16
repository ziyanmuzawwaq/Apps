using Donate.Application.Interfaces;
using Donate.Application.Services;
using Donate.Domain.Interfaces;
using Donate.Infrastructure.Data;
using Donate.Infrastructure.Repositories;
using Donate.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEventJobService, EventJobService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventJobRepository, EventJobRepository>();

builder.Services.AddHostedService<EventConsumerJob>();

builder.Services.AddMemoryCache();

builder.Build().Run();