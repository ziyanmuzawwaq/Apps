using Shared.Entities;
using User.Application.Interfaces;
using User.Domain.Interfaces;

namespace User.Application.Services
{
    public class EventJobService : IEventJobService
    {
        private readonly IEventJobRepository _eventJobRepository;

        public EventJobService(
            IEventJobRepository eventJobRepository)
        {
            _eventJobRepository = eventJobRepository;
        }

        public async Task<EventJobMonitoring?> GetEventJobMonitoringByName(string eventJobName, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _eventJobRepository.GetByName(eventJobName, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> CreateEventJobMonitoring(EventJobMonitoring eventJob, CancellationToken cancellationToken = default)
        {
            try
            {
                await _eventJobRepository.AddAsync(eventJob);
                return await _eventJobRepository.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateEventJobMonitoring(EventJobMonitoring eventJob, CancellationToken cancellationToken = default)
        {
            try
            {
                await _eventJobRepository.UpdateAsync(eventJob);
                return await _eventJobRepository.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}