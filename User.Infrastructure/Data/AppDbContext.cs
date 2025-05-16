using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using User.Domain.Entities;
using User.Domain.Entities.ViewModels;
using User.Infrastructure.ModelBuilders;

namespace User.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<DonationViewModel> DonationViewModel { get; set; }
        public DbSet<EventJobMonitoring> EventJobMonitoring { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new UsersBuilder(this).Configure(modelBuilder.Entity<Users>());

            new DonationViewModelBuilder(this).Configure(modelBuilder.Entity<DonationViewModel>());

            new EventJobMonitoringBuilder(this).Configure(modelBuilder.Entity<EventJobMonitoring>());
        }
    }
}