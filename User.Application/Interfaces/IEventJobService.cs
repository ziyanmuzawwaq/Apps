using Shared.Entities;

namespace User.Application.Interfaces
{
    public interface IEventJobService
    {
        Task<EventJobMonitoring?> GetEventJobMonitoringByName(string eventJobName, CancellationToken cancellationToken = default);

        Task<int> CreateEventJobMonitoring(EventJobMonitoring eventJob, CancellationToken cancellationToken = default);

        Task<int> UpdateEventJobMonitoring(EventJobMonitoring eventJob, CancellationToken cancellationToken = default);
    }
}