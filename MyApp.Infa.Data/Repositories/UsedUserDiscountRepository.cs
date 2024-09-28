using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class UsedUserDiscountRepository : GenericRepository<UsedUserDiscount>, IUsedUserDiscountRepository
    {
        private readonly MyAppDbContext _dbContext;

        public UsedUserDiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UsedUserDiscount> FindUsedUserDiscountByCodeAsync(int userId, string discountCode)
        {
            return await _dbContext.UsedUserDiscounts
            .Include(ud => ud.Discount) // اطمینان از بارگذاری موجودیت تخفیف
            .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.Discount.DiscountCode == discountCode);
        }
    }
}
