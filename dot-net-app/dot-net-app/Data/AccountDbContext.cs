using dot_net_app.Model.AccountModel;
using Microsoft.EntityFrameworkCore;

namespace dot_net_app.Data
{
    public class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
        .ToTable("users")
        .HasKey(u => u.Id); // Assuming Id is the primary key

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasColumnName("id");

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .HasColumnName("username");

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnName("email");

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .HasColumnName("password_hash");

            modelBuilder.Entity<User>()
                .Property(u => u.FullName)
                .HasColumnName("full_name");

            modelBuilder.Entity<User>()
                .Property(u => u.MobileNumber)
                .HasColumnName("mobile_number");

            modelBuilder.Entity<User>()
                .Property(u => u.Gender)
                .HasColumnName("gender");

            modelBuilder.Entity<User>()
                .Property(u => u.DateOfBirth)
                .HasColumnName("date_of_birth");

            modelBuilder.Entity<User>()
                .Property(u => u.IsAdmin)
                .HasColumnName("is_admin");

            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasColumnName("is_active");

            modelBuilder.Entity<User>()
                .Property(u => u.IsVerified)
                .HasColumnName("is_verified");

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasColumnName("created_at");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasColumnName("updated_at");

            modelBuilder.Entity<User>()
                .Property(u => u.LastLoginAt)
                .HasColumnName("last_login_at");
        }
    }
}
