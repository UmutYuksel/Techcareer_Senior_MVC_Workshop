using EfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Duty> Dutys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kullanıcı silindiğinde, ona bağlı görevlerin de silinmesi
            modelBuilder.Entity<Duty>()
                .HasOne(d => d.User)
                .WithMany(u => u.Duties)  // Her kullanıcı birden fazla göreve sahip olabilir
                .HasForeignKey(d => d.UserId)  // Foreign key'i doğru şekilde tanımladık
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete
        }
    }

}


