using Donate.Domain.Entities;
using Donate.Domain.Entities.ViewModels;
using Donate.Infrastructure.ModelBuilders;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Donate.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Donation> Donation { get; set; }
        public DbSet<UserViewModel> UserViewModel { get; set; }
        public DbSet<EventJobMonitoring> EventJobMonitoring { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new UserViewModelBuilder(this).Configure(modelBuilder.Entity<UserViewModel>());

            new DonationBuilder(this).Configure(modelBuilder.Entity<Donation>());

            new EventJobMonitoringBuilder(this).Configure(modelBuilder.Entity<EventJobMonitoring>());
        }
    }
}