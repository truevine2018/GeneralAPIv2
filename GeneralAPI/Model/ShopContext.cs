using Microsoft.EntityFrameworkCore;

namespace GeneralAPI.Model
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> option) : base(option) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(a => a.Category)
                .HasForeignKey(n => n.CategoryId);

            modelBuilder.Seed();
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
