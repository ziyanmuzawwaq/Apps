using Shared.Entities;

namespace Donate.Domain.Entities.ViewModels
{
    public class UserViewModel : BaseEntities
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public List<Donation> Donations { get; set; } = new();
    }
}