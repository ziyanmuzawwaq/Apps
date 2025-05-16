using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using User.Domain.Entities;
using User.Domain.Interfaces;
using User.Infrastructure.Data;

namespace User.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public AppDbContext _context;

        public UserRepository(IConfiguration configuration, AppDbContext context)
            : base(configuration)
        {
            _context = context;
        }

        public async Task<Users?> GetById(int userId)
        {
            try
            {
                return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Users?> GetByNameAndEmail(string name, string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(x => string.Equals(x.Name, name) && string.Equals(x.Email, email) && x.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Users?> GetByNameOrEmail(string name, string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(x => string.Equals(x.Name, name) || string.Equals(x.Email, email) && x.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Users>?> GetAll()
        {
            try
            {
                return await _context.Users.Where(x => x.IsDeleted == false).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task Post(Users user)
        {
            try
            {
                _context.Entry<Users>(user).State = EntityState.Added;

                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task Put(Users user)
        {
            try
            {
                _context.Entry<Users>(user).State = EntityState.Modified;

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