using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using User.Domain.Entities.ViewModels;
using User.Domain.Interfaces;
using User.Infrastructure.Data;

namespace User.Infrastructure.Repositories
{
    public class DonationRepository : BaseRepository, IDonationRepository
    {
        public AppDbContext _context;

        public DonationRepository(
            IConfiguration configuration,
            AppDbContext context)
            : base(configuration)
        {
            _context = context;
        }

        public async Task<List<DonationViewModel>> GetByUserId(int userId)
        {
            return await _context.DonationViewModel.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task AddAsync(DonationViewModel donation)
        {
            await _context.DonationViewModel.AddAsync(donation);
        }

        public Task UpdateAsync(DonationViewModel donation)
        {
            var trackedEntity = _context.ChangeTracker.Entries<DonationViewModel>()
                                                      .FirstOrDefault(e => e.Entity.DonationId == donation.DonationId);

            if (trackedEntity != null) trackedEntity.State = EntityState.Detached;

            _context.DonationViewModel.Update(donation);

            return Task.CompletedTask;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync();
        }
    }
}