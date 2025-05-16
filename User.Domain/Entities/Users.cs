using Shared.Entities;
using User.Domain.Entities.ViewModels;

namespace User.Domain.Entities
{
    public class Users : BaseEntities
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<DonationViewModel> Donations { get; set; } = new();
    }
}