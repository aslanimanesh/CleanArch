using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        #region Fields

        private readonly MyAppDbContext _dbContext;

        #endregion

        #region Constructor

        public DiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Public Methods

        #region GetByDiscountCodeAsync
        public async Task<Discount> GetDiscountByDiscountCodeAsync(string discountCode)
        {
            return await _dbContext.Discounts
            .Include(d => d.UserDiscounts)   // بارگذاری UserDiscounts
            .Include(d => d.ProductDiscounts) // بارگذاری ProductDiscounts
            .SingleOrDefaultAsync(d => d.DiscountCode == discountCode);
        }

        #endregion

        #region IsExistDiscountCode
        public async Task<bool> IsExistDiscountCode(string discountCode)
        {
            return await _dbContext.Discounts.AnyAsync(d => d.DiscountCode == discountCode);
        }
        #endregion


        public async Task<IEnumerable<Discount>> GetAllActiveDiscountsWithoutCodeForUserAsync(int? userId)
        {
            // ابتدا تمام تخفیف‌های فعال و بدون کد را فیلتر می‌کنیم
            var activeDiscounts = await _dbContext.Discounts
                .Where(d => d.IsActive &&
                             d.DiscountCode == null &&
                             (d.StartDate == null || d.StartDate <= DateTime.UtcNow) &&
                             (d.EndDate == null || d.EndDate >= DateTime.UtcNow) &&
                             (d.UsableCount == null || d.UsableCount > 0))
                .Include(d=>d.UserDiscounts)
                .Include(d => d.ProductDiscounts) // بارگذاری ProductDiscounts
                .ToListAsync();
               

            // تخفیف‌های عمومی که برای همه کاربران هستند
            var generalDiscounts = activeDiscounts
                .Where(d => d.IsGeneralForUsers)
                .OrderByDescending(d => d.StartDate)
                .ThenByDescending(d => d.DiscountPercentage)
                .ToList();

            // اگر کاربر لاگین کرده باشد
            if (userId.HasValue)
            {
                // تخفیف‌های مرتبط با این کاربر را پیدا کنید
                var userSpecificDiscounts = activeDiscounts
                    .Where(d => d.UserDiscounts != null && d.UserDiscounts.Any(ud => ud.UserId == userId.Value))
                    .OrderByDescending(d => d.StartDate)
                    .ThenByDescending(d => d.DiscountPercentage)
                    .ToList();

                // حذف موارد تکراری و حفظ تخفیف اختصاصی در اولویت
                var combinedDiscounts = userSpecificDiscounts
                    .Concat(generalDiscounts)
                    .GroupBy(d => d.Id)
                    .Select(g => g.FirstOrDefault(d => d.UserDiscounts != null && d.UserDiscounts.Any(ud => ud.UserId == userId.Value)) ?? g.First())
                    .ToList();

                return combinedDiscounts;
            }


            return generalDiscounts;
        }


        #endregion
    }
}
