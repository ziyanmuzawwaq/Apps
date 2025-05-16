using User.Domain.Entities;

namespace User.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Users?> GetById(int userId);

        Task<Users?> GetByNameAndEmail(string name, string email);

        Task<Users?> GetByNameOrEmail(string name, string email);

        Task<List<Users>?> GetAll();

        Task Post(Users user);

        Task Put(Users user);

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}