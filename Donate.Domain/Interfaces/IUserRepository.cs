using Donate.Domain.Entities.ViewModels;

namespace Donate.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserViewModel?> GetById(int userId, CancellationToken cancellationToken = default);

        Task AddAsync(UserViewModel user);

        Task UpdateAsync(UserViewModel user);

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}