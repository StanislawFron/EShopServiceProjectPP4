using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;
using User.Domain.Models;
using User.Domain.Repositories;

namespace User.Domain.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User.Domain.Models.Entities.User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.Address)
                .WithOne()
                .HasForeignKey<Client>(c => c.AddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
