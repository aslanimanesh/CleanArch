using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Models;

namespace MyApp.Infa.Data.Context
{
    public class MyAppDbContext : DbContext
    {

        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<UserDiscount> UserDiscounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<ProductDiscount>()
                .HasKey(pd => new { pd.ProductId, pd.DiscountId });

          
            modelBuilder.Entity<UserDiscount>()
                .HasKey(ud => new { ud.UserId, ud.DiscountId });

            base.OnModelCreating(modelBuilder);
        }

    }
}
