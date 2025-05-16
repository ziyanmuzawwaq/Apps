using User.Application.Interfaces;
using User.Domain.Entities;
using User.Domain.Interfaces;

namespace User.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Users?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _userRepository.GetById(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Users>?> GetAllUserAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _userRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Users> CreateUserAsync(Users user, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userRepository.Post(user);
                await _userRepository.CommitAsync(cancellationToken);

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Users> UpdateUserAsync(Users user, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userRepository.Put(user);
                await _userRepository.CommitAsync(cancellationToken);

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}