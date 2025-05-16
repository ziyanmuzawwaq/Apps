using Donate.Domain.Entities.ViewModels;

namespace Donate.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel?> GetUserById(int userId, CancellationToken cancellationToken = default);

        Task<int> CreateUserByEvent(UserViewModel user, CancellationToken cancellationToken = default);

        Task<int> UpdateUserByEvent(UserViewModel user, CancellationToken cancellationToken = default);
    }
}