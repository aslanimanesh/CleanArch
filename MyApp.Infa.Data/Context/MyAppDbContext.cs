using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Models;

namespace MyApp.Infa.Data.Context
{
    public class MyAppDbContext : DbContext
    {
        #region Constructor
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {

        }
        #endregion

        #region DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<UserDiscount> UserDiscounts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<UsableUserDiscount> UsableUserDiscounts { get; set; }

        #endregion

        #region Model Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDiscount>()
                .HasKey(pd => new { pd.ProductId, pd.DiscountId });


            modelBuilder.Entity<UserDiscount>()
                .HasKey(ud => new { ud.UserId, ud.DiscountId });


            modelBuilder.Entity<UsableUserDiscount>()
                .HasKey(ud => new { ud.UserId, ud.DiscountId });

            base.OnModelCreating(modelBuilder);
        }

        #endregion

    }
}
