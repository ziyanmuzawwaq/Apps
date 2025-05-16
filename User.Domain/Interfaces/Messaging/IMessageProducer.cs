namespace User.Domain.Interfaces.Messaging
{
    public interface IMessageProducer
    {
        Task PublishAsync(string topic, string message, CancellationToken cancellationToken);
    }
}