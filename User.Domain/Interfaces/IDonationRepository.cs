using User.Domain.Entities.ViewModels;

namespace User.Domain.Interfaces
{
    public interface IDonationRepository
    {
        Task<List<DonationViewModel>> GetByUserId(int userId);

        Task AddAsync(DonationViewModel donation);

        Task UpdateAsync(DonationViewModel donation);

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}