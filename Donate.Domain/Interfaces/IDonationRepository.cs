using Donate.Domain.Entities;

namespace Donate.Domain.Interfaces
{
    public interface IDonationRepository
    {
        Task<Donation?> GetById(int donationId, CancellationToken cancellationToken = default);

        Task AddAsync(Donation donation);

        Task UpdateAsync(Donation donation);

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}