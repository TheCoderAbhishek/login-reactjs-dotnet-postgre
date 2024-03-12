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
                .HasKey(u => u.Id);
        }
    }
}
