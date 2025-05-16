using Donate.Application.Interfaces;
using Donate.Domain.Entities;
using Donate.Domain.Interfaces;

namespace Donate.Application.Services
{
    public class DonationService : IDonationService
    {
        private readonly IDonationRepository _donationRepository;

        public DonationService(
            IDonationRepository donationRepository)
        {
            _donationRepository = donationRepository;
        }

        public async Task<Donation?> GetDonationById(int donationId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _donationRepository.GetById(donationId, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Donation> CreateDonation(Donation donation, CancellationToken cancellationToken = default)
        {
            try
            {
                await _donationRepository.AddAsync(donation);
                await _donationRepository.CommitAsync(cancellationToken);

                return donation;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}