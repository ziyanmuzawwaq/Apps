using Donate.Domain.Entities;
using Donate.Domain.Interfaces;
using Donate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Donate.Infrastructure.Repositories
{
    public class DonationRepository : BaseRepository, IDonationRepository
    {
        public AppDbContext _context;

        public DonationRepository(IConfiguration configuration, AppDbContext context, IMemoryCache memoryCache)
            : base(configuration, memoryCache)
        {
            _context = context;
        }

        public async Task<Donation?> GetById(int donationId, CancellationToken cancellationToken = default)
        {
            return await _context.Donation.Include(x => x.Users).FirstOrDefaultAsync(x => x.DonationId == donationId, cancellationToken);
        }

        public async Task AddAsync(Donation donation)
        {
            await _context.Donation.AddAsync(donation);
        }

        public Task UpdateAsync(Donation donation)
        {
            var trackedEntity = _context.ChangeTracker.Entries<Donation>()
                                                      .FirstOrDefault(e => e.Entity.DonationId == donation.DonationId);

            if (trackedEntity != null) trackedEntity.State = EntityState.Detached;

            _context.Donation.Update(donation);

            return Task.CompletedTask;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync();
        }
    }
}