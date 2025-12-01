using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Runtime;

namespace EFCoreProject_TRPO
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=EFCoreProject;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

            //modelBuilder.Entity<Role>().HasData(
            //    new Role { Id = 1, Title = "Пользователь" },
            //    new Role { Id = 2, Title = "Менеджер" },
            //    new Role { Id = 3, Title = "Администратор" }
            //);

            //modelBuilder.Entity<User>()
            //    .Property(u => u.RoleId)
            //    .HasDefaultValue(1);
        }
    }
}
