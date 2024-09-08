using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class InvoicikaDbContext : DbContext
    {
        public InvoicikaDbContext(DbContextOptions<InvoicikaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary key for User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            // Define primary key for Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);

            // Define primary key for Item
            modelBuilder.Entity<Item>()
                .HasKey(i => i.ItemId);

            // Define relationships, indexes, etc.
            // Example: One-to-many relationship between Role and User
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // Example: Seed initial data here if required
            // Note: Seeding here is optional if you seed data in Program.cs
        }
    }
}
