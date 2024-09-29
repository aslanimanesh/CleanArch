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

        #region GetAllActiveDiscounts
        public async Task<IEnumerable<Discount>> GetAllActiveDiscountsAsync()
        {
            return await _dbContext.Discounts
                .Where(d => d.IsActive &&
                            (d.StartDate == null || d.StartDate <= DateTime.UtcNow) &&
                            (d.EndDate == null || d.EndDate >= DateTime.UtcNow) &&
                            (d.UsableCount == null || d.UsableCount > 0))
                .ToListAsync();
        }

        #endregion

        #region GetLatestActiveDiscount

        public async Task<Discount> GetLatestActiveDiscountAsync(int? userId)
        {
            // ابتدا تمام تخفیف‌های فعال را دریافت می‌کنیم
            var discounts = await GetAllActiveDiscountsAsync();

            // تخفیف‌های عمومی که برای همه کاربران هستند
            var generalDiscounts = discounts
                .Where(d => d.IsGeneralForUsers)
                .OrderByDescending(d => d.StartDate)
                .ThenByDescending(d => d.DiscountPercentage)
                .FirstOrDefault();

            // اگر کاربر لاگین کرده باشد
            if (userId.HasValue)
            {
                // تخفیف‌های مرتبط با این کاربر را پیدا کنید
                var userSpecificDiscounts = discounts
                    .Where(d => d.UserDiscounts.Any(ud => ud.UserId == userId.Value))
                    .OrderByDescending(d => d.StartDate)
                    .ThenByDescending(d => d.DiscountPercentage)
                    .FirstOrDefault();

                // اگر تخفیف اختصاصی برای کاربر وجود داشت، آن را برگردانید
                if (userSpecificDiscounts != null)
                {
                    return userSpecificDiscounts;
                }
            }

            // اگر کاربر لاگین نکرده یا تخفیف اختصاصی نداشت، تخفیف عمومی را برگردانید
            return generalDiscounts;
        }

        #endregion

        #endregion
    }
}
