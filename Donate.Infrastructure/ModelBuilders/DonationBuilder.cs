using Donate.Domain.Entities;
using Donate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Donate.Infrastructure.ModelBuilders
{
    public class DonationBuilder : IEntityTypeConfiguration<Donation>
    {
        private readonly AppDbContext _context;

        public DonationBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.ToTable("Donation");

            builder.HasKey(x => x.DonationId);

            builder.Property(x => x.DonorName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.DonorEmail)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Amount)
                   .HasPrecision(18, 0)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .HasConversion<DateTime>()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValueSql("0");
        }
    }
}