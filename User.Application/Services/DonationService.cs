using User.Application.Interfaces;
using User.Domain.Entities.ViewModels;
using User.Domain.Interfaces;

namespace User.Application.Services
{
    public class DonationService : IDonationService
    {
        private readonly IDonationRepository _donationRepository;

        public DonationService(
            IDonationRepository donationRepository)
        {
            _donationRepository = donationRepository;
        }

        public async Task<List<DonationViewModel>> GetDonationByUserId(int userId)
        {
            try
            {
                return await _donationRepository.GetByUserId(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> CreateDonationByEvent(DonationViewModel donation, CancellationToken cancellationToken = default)
        {
            try
            {
                await _donationRepository.AddAsync(donation);
                return await _donationRepository.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}