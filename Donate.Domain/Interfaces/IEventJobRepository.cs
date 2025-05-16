using Shared.Entities;

namespace Donate.Domain.Interfaces
{
    public interface IEventJobRepository
    {
        Task<EventJobMonitoring?> GetByName(string eventJobName, CancellationToken cancellationToken = default);

        Task AddAsync(EventJobMonitoring request);

        Task UpdateAsync(EventJobMonitoring request);

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}