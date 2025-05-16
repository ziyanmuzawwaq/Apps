using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;
using User.Infrastructure.Data;

namespace User.Infrastructure.ModelBuilders
{
    public class EventJobMonitoringBuilder : IEntityTypeConfiguration<EventJobMonitoring>
    {
        private readonly AppDbContext _context;

        public EventJobMonitoringBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Configure(EntityTypeBuilder<EventJobMonitoring> builder)
        {
            builder.ToTable("EventJobMonitoring");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EventJobName)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValueSql("0");
        }
    }
}