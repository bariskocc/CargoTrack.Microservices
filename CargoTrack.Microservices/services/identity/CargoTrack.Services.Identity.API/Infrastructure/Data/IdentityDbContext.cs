using CargoTrack.Services.Identity.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Services.Identity.API.Infrastructure.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(256);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.CompanyName).HasMaxLength(200);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.HasMany(u => u.Roles)
                    .WithMany(r => r.Users)
                    .UsingEntity(j => j.ToTable("UserRoles"));
            });

            // Role entity configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.HasMany(r => r.Permissions)
                    .WithMany(p => p.Roles)
                    .UsingEntity(j => j.ToTable("RolePermissions"));
            });

            // Permission entity configuration
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Description).HasMaxLength(500);
            });
        }
    }
} 