using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Entities;
using User.Domain.Interfaces;
using User.Infrastructure.Data;

namespace User.Infrastructure.Repositories
{
    public class EventJobRepository : BaseRepository, IEventJobRepository
    {
        public AppDbContext _context;

        public EventJobRepository(IConfiguration configuration, AppDbContext context)
            : base(configuration)
        {
            _context = context;
        }

        public async Task<EventJobMonitoring?> GetByName(string eventJobName, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.EventJobMonitoring.FirstOrDefaultAsync(x => x.EventJobName == eventJobName, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task AddAsync(EventJobMonitoring request)
        {
            try
            {
                _context.Entry<EventJobMonitoring>(request).State = EntityState.Added;

                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task UpdateAsync(EventJobMonitoring request)
        {
            try
            {
                _context.Entry<EventJobMonitoring>(request).State = EntityState.Modified;

                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}