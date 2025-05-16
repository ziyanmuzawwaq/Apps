using System.ComponentModel.DataAnnotations;

namespace Donate.Domain.Models.Requests
{
    public record CreateDonationRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string DonorName { get; set; } = string.Empty;

        [EmailAddress]
        public string DonorEmail { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; } = 0;
    }
}