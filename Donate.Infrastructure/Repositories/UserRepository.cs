using Donate.Domain.Entities.ViewModels;
using Donate.Domain.Interfaces;
using Donate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Donate.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public AppDbContext _context;

        public UserRepository(IConfiguration configuration, AppDbContext context, IMemoryCache memoryCache)
            : base(configuration, memoryCache)
        {
            _context = context;
        }

        public async Task<UserViewModel?> GetById(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserViewModel.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }

        public Task AddAsync(UserViewModel user)
        {
            var trackedEntity = _context.ChangeTracker.Entries<UserViewModel>()
                                                      .FirstOrDefault(e => e.Entity.UserId == user.UserId);

            if (trackedEntity != null) trackedEntity.State = EntityState.Detached;

            _context.UserViewModel.Add(user);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(UserViewModel user)
        {
            var trackedEntity = _context.ChangeTracker.Entries<UserViewModel>()
                                                      .FirstOrDefault(e => e.Entity.UserId == user.UserId);

            if (trackedEntity != null) trackedEntity.State = EntityState.Detached;

            _context.UserViewModel.Update(user);

            return Task.CompletedTask;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync();
        }
    }
}