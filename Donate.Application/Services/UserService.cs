using Donate.Application.Interfaces;
using Donate.Domain.Entities.ViewModels;
using Donate.Domain.Interfaces;

namespace Donate.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserViewModel?> GetUserById(int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _userRepository.GetById(userId, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> CreateUserByEvent(UserViewModel user, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userRepository.AddAsync(user);
                return await _userRepository.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateUserByEvent(UserViewModel user, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userRepository.UpdateAsync(user);
                return await _userRepository.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}