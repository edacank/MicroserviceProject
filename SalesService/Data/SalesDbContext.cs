using Microsoft.EntityFrameworkCore;
using SalesService.Models;

namespace SalesService.Data
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }

        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Sale tablosundaki Amount için precision ve scale belirleme
            modelBuilder.Entity<Sale>()
                .Property(s => s.Amount)
                .HasPrecision(18, 4); // 18 toplam basamak, 4 ondalık basamak

            base.OnModelCreating(modelBuilder); // Temel sınıf çağrısı
        }
    }
}
