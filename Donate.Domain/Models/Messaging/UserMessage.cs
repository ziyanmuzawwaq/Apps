using Shared.Entities;

namespace Donate.Domain.Models.Messaging
{
    public class UserMessage : BaseEntities
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}