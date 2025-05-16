using Shared.Entities;

namespace User.Domain.Entities.ViewModels
{
    public class DonationViewModel : BaseEntities
    {
        public int DonationId { get; set; }
        public int UserId { get; set; }
        public Users? Users { get; set; }
        public string DonorName { get; set; } = string.Empty;
        public string DonorEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}