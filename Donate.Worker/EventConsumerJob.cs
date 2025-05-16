using Confluent.Kafka;
using Donate.Application.Interfaces;
using Donate.Domain.Entities.ViewModels;
using Donate.Domain.Models.Constants;
using Donate.Domain.Models.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Entities;
using Shared.Models.Constants;
using Shared.Models.Enum;
using System.Text.Json;

namespace Donate.Worker
{
    public class EventConsumerJob : BackgroundService
    {
        private readonly ILogger<EventConsumerJob> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _bootstrapServers;
        private readonly IUserService _userService;
        private readonly IEventJobService _eventJobService;

        private Task? _executeService;
        private CancellationTokenSource? _cancellationTokenSource;

        public EventConsumerJob(
            ILogger<EventConsumerJob> logger,
            IConfiguration configuration,
            IUserService userService,
            IEventJobService eventJobService)
        {
            _logger = logger;
            _configuration = configuration;
            _bootstrapServers = _configuration.GetValue<string>("Kafka:BootstrapServers") ?? "localhost:9092";
            _userService = userService;
            _eventJobService = eventJobService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation($"{nameof(EventConsumerJob)} is starting.");

                EventJobMonitoring? eventJobMonitoring = await _eventJobService.GetEventJobMonitoringByName(EventJobConstant.EventConsumerJob, stoppingToken);

                if (eventJobMonitoring == null)
                {
                    await _eventJobService.CreateEventJobMonitoring(new EventJobMonitoring
                    {
                        EventJobName = EventJobConstant.EventConsumerJob,
                        Status = EventJobStatus.Online.ToString()
                    }, stoppingToken);
                }
                else
                {
                    eventJobMonitoring.Status = EventJobStatus.Online.ToString();
                    eventJobMonitoring.UpdatedAt = DateTime.UtcNow;

                    await _eventJobService.UpdateEventJobMonitoring(eventJobMonitoring, stoppingToken);
                }

                _executeService = ExecuteAsync(_cancellationTokenSource!.Token);
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to start {nameof(EventConsumerJob)}");
                _executeService = StartAsync(_cancellationTokenSource!.Token);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(EventConsumerJob)} is stopping.");

            try
            {
                await _eventJobService.UpdateEventJobMonitoring(new EventJobMonitoring
                {
                    Status = EventJobStatus.Offline.ToString(),
                    UpdatedAt = DateTime.UtcNow,
                }, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(EventConsumerJob)} Failed to update status event job, Message: {ex.Message}");
            }

            if (_executeService == null) return;

            try
            {
                _cancellationTokenSource?.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executeService, Task.Delay(Timeout.Infinite, stoppingToken));
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "user-consumer-group",
                BootstrapServers = _bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe([TopicConstant.CreateUserTopic, TopicConstant.UpdateUserTopic]);

            _logger.LogInformation($"Started consuming topic: {TopicConstant.CreateUserTopic}, {TopicConstant.UpdateUserTopic}");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    if (string.IsNullOrEmpty(result.Message.Value)) continue;

                    var user = JsonSerializer.Deserialize<UserMessage>(result.Message.Value);

                    if (user != null)
                    {
                        UserViewModel? existUser = null;

                        switch (result.Topic)
                        {
                            case TopicConstant.CreateUserTopic:

                                existUser = await _userService.GetUserById(user.UserId, stoppingToken);

                                if (existUser != null) continue;

                                var newUser = new UserViewModel
                                {
                                    UserId = user.UserId,
                                    Name = user.Name,
                                    Email = user.Email,
                                    CreatedAt = user.CreatedAt,
                                    UpdatedAt = user.UpdatedAt,
                                    IsDeleted = user.IsDeleted
                                };

                                await _userService.CreateUserByEvent(newUser, stoppingToken);

                                _logger.LogInformation($"{DateTime.Now} - Consumed user: {newUser?.UserId} created at {newUser?.CreatedAt}");

                                break;

                            case TopicConstant.UpdateUserTopic:

                                existUser = await _userService.GetUserById(user.UserId, stoppingToken);

                                if (existUser == null) continue;

                                existUser.Name = user.Name;
                                existUser.Email = user.Email;
                                existUser.CreatedAt = user.CreatedAt;
                                existUser.UpdatedAt = user.UpdatedAt;
                                existUser.IsDeleted = user.IsDeleted;

                                var updatedUser = await _userService.UpdateUserByEvent(existUser, stoppingToken);

                                _logger.LogInformation($"{DateTime.Now} - Consumed user: {existUser?.UserId} updated at {existUser?.UpdatedAt}");

                                break;
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"{DateTime.Now} - Message value is empty!");
                    }
                }
                catch (ConsumeException e)
                {
                    _logger.LogError(e, $"{DateTime.Now} - Kafka consumption error");
                }
            }

            consumer.Close();
        }
    }
}