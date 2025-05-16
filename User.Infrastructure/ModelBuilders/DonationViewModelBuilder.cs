using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Entities.ViewModels;
using User.Infrastructure.Data;

namespace User.Infrastructure.ModelBuilders
{
    public class DonationViewModelBuilder
    {
        private readonly AppDbContext _context;

        public DonationViewModelBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Configure(EntityTypeBuilder<DonationViewModel> builder)
        {
            builder.ToTable("DonationViewModel");

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