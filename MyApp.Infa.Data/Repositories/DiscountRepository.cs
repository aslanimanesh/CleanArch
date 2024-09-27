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
        public async Task<Discount> GetByDiscountCodeAsync(string discountCode)
        {
            return await _dbContext.Discounts.SingleOrDefaultAsync(d => d.DiscountCode == discountCode);
        }

        #endregion

        #region IsExistDiscountCode
        public async Task<bool> IsExistDiscountCode(string discountCode)
        {
            return _dbContext.Discounts.Any(d=> d.DiscountCode == discountCode);
        }
        #endregion

        #endregion
    }
}
