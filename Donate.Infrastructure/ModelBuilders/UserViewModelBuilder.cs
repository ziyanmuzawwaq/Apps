using Donate.Domain.Entities.ViewModels;
using Donate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Donate.Infrastructure.ModelBuilders
{
    public class UserViewModelBuilder : IEntityTypeConfiguration<UserViewModel>
    {
        private readonly AppDbContext _context;

        public UserViewModelBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Configure(EntityTypeBuilder<UserViewModel> builder)
        {
            builder.ToTable("UserViewModel");

            builder.HasKey(x => x.UserId);

            builder.Property(x => x.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Email)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValueSql("0");

            builder.HasMany(x => x.Donations)
                   .WithOne(x => x.Users)
                   .HasForeignKey(x => x.UserId);
        }
    }
}