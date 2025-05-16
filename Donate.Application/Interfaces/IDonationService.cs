using Donate.Domain.Entities;

namespace Donate.Application.Interfaces
{
    public interface IDonationService
    {
        Task<Donation?> GetDonationById(int donationId, CancellationToken cancellationToken = default);

        Task<Donation> CreateDonation(Donation donation, CancellationToken cancellationToken = default);
    }
}