using Donate.Domain.Entities.ViewModels;
using Shared.Entities;

namespace Donate.Domain.Entities
{
    public class Donation : BaseEntities
    {
        public int DonationId { get; set; }
        public int UserId { get; set; }
        public UserViewModel? Users { get; set; }
        public string DonorName { get; set; } = string.Empty;
        public string DonorEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}