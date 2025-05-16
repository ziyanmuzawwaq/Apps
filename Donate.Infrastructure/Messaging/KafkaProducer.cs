using Confluent.Kafka;
using Donate.Domain.Interfaces.Messaging;
using Microsoft.Extensions.Logging;

namespace Donate.Infrastructure.Messaging
{
    public class KafkaProducer : IMessageProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(ILogger<KafkaProducer> logger, string bootstrapServers)
        {
            _logger = logger;
            var config = new ProducerConfig { BootstrapServers = bootstrapServers, AllowAutoCreateTopics = true, Acks = Acks.All };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task PublishAsync(string topic, string message, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                _logger.LogInformation($"{DateTime.Now} - Kafka: Published message to {topic}, offset: {result.Offset}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} - Kafka publish failed");
                throw;
            }

            _producer.Flush(cancellationToken);
        }
    }
}