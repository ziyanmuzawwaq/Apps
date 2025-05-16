using User.Domain.Entities.ViewModels;

namespace User.Application.Interfaces
{
    public interface IDonationService
    {
        Task<List<DonationViewModel>> GetDonationByUserId(int userId);

        Task<int> CreateDonationByEvent(DonationViewModel donation, CancellationToken cancellationToken = default);
    }
}