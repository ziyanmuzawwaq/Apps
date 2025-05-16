using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Entities;
using User.Infrastructure.Data;

namespace User.Infrastructure.ModelBuilders
{
    public class UsersBuilder : IEntityTypeConfiguration<Users>
    {
        private readonly AppDbContext _context;

        public UsersBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users");

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