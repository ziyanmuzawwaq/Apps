using System.ComponentModel.DataAnnotations;

namespace User.Domain.Models.Requests
{
    public record UpdateUserRequest
    {
        [Required]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}