using System.ComponentModel.DataAnnotations;

namespace User.Domain.Models.Requests
{
    public record CreateUserRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}