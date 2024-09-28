using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class UsedProductDiscountRepository : GenericRepository<UsedProductDiscount>, IUsedProductDiscountRepository
    {
        private readonly MyAppDbContext _dbContext;

        public UsedProductDiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UsedProductDiscount> FindUsedProductDiscountByCodeAsync(int productId, string discountCode, int orderId)
        {
            return await _dbContext.UsedProductDiscounts
             .Include(upd => upd.Discount) // اطمینان از بارگذاری موجودیت تخفیف
             .FirstOrDefaultAsync(upd => upd.ProductId == productId && upd.Discount.DiscountCode == discountCode && upd.OrderId == orderId);
        }
    }
}
