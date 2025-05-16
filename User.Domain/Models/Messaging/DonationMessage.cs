using Shared.Entities;

namespace User.Domain.Models.Messaging
{
    public class DonationMessage : BaseEntities
    {
        public int DonationId { get; set; }
        public int UserId { get; set; }
        public string DonorName { get; set; } = string.Empty;
        public string DonorEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}