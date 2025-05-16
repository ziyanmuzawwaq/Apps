using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using User.Application.Interfaces;
using User.Domain.Entities;
using User.Domain.Entities.ViewModels;
using User.Domain.Models.Constants;
using User.Domain.Models.Enum;
using User.Domain.Models.Messaging;

namespace User.Consumer
{
    public class EventConsumerJob : BackgroundService
    {
        private readonly ILogger<EventConsumerJob> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _bootstrapServers;
        private readonly IDonationService _donationService;
        private readonly IEventJobService _eventJobService;

        private Task? _executeService;
        private CancellationTokenSource? _cancellationTokenSource;

        public EventConsumerJob(
            ILogger<EventConsumerJob> logger,
            IConfiguration configuration,
            IDonationService donationService,
            IEventJobService eventJobService)
        {
            _logger = logger;
            _configuration = configuration;
            _bootstrapServers = _configuration.GetValue<string>("Kafka:BootstrapServers") ?? "localhost:9092";
            _donationService = donationService;
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

            consumer.Subscribe([TopicConstant.CreateDonationTopic]);

            _logger.LogInformation($"Started consuming topic: {TopicConstant.CreateDonationTopic}");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    if (string.IsNullOrEmpty(result.Message.Value)) continue;

                    var donation = JsonSerializer.Deserialize<DonationMessage>(result.Message.Value);

                    if (donation != null)
                    {
                        var newDonation = new DonationViewModel
                        {
                            DonationId = donation.DonationId,
                            UserId = donation.UserId,
                            DonorName = donation.DonorName,
                            DonorEmail = donation.DonorEmail,
                            Amount = donation.Amount,
                            CreatedAt = donation.CreatedAt,
                            UpdatedAt = donation.UpdatedAt,
                            IsDeleted = donation.IsDeleted
                        };

                        await _donationService.CreateDonationByEvent(newDonation, stoppingToken);

                        _logger.LogInformation($"{DateTime.Now} - Consumed donation: {newDonation?.DonationId} created at {newDonation?.CreatedAt}");
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