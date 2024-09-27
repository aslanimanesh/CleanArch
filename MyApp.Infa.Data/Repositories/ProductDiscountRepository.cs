using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;
using System.Linq.Expressions;

namespace MyApp.Infa.Data.Repositories
{
    public class ProductDiscountRepository : GenericRepository<ProductDiscount>, IProductDiscountRepository
    {
        #region Fields
        private readonly MyAppDbContext _dbContext;
        #endregion

        #region Constructor
        public ProductDiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Public Methods

        #region GetAsync
        public async Task<ProductDiscount> GetAsync(Expression<Func<ProductDiscount, bool>> predicate)
        {
            return await _dbContext.ProductDiscounts.FirstOrDefaultAsync(predicate);
        }
        #endregion

        #region GetDiscountsForProductAsync
        public async Task<List<ProductDiscount>> GetDiscountsForProductAsync(int productId)
        {
            return await _dbContext.ProductDiscounts
               .Where(pd => pd.ProductId == productId)
               .Include(pd => pd.Discount)
               .ToListAsync();
        }
        #endregion

        #region GetDiscountsForProductsAsync
        public async Task<IEnumerable<ProductDiscount>> GetDiscountsForProductsAsync(List<int> productIds, int discountId)
        {
            return await _dbContext.ProductDiscounts
                .Where(pd => productIds.Contains(pd.ProductId) && pd.DiscountId == discountId)
                .ToListAsync();
        }
        #endregion

        #endregion
    }
}
