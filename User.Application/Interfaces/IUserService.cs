using User.Domain.Entities;

namespace User.Application.Interfaces
{
    public interface IUserService
    {
        Task<Users> CreateUserAsync(Users user, CancellationToken cancellationToken = default);

        Task<Users?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default);

        Task<List<Users>?> GetAllUserAsync(CancellationToken cancellationToken = default);

        Task<Users> UpdateUserAsync(Users user, CancellationToken cancellationToken = default);
    }
}