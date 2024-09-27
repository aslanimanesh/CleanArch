using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;
using System.Linq.Expressions;

namespace MyApp.Infa.Data.Repositories
{
    public class UserDiscountRepository : GenericRepository<UserDiscount>, IUserDiscountRepository
    {
        #region Fields
        private readonly MyAppDbContext _dbContext;
        #endregion

        #region Constructor
        public UserDiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Public Methods

        #region GetAsync
        public async Task<UserDiscount> GetAsync(Expression<Func<UserDiscount, bool>> predicate)
        {
            return await _dbContext.UserDiscounts.FirstOrDefaultAsync(predicate);
        }
        #endregion

        #region GetDiscountsForUserAsync
        public async Task<List<UserDiscount>> GetDiscountsForUserAsync(int userId)
        {
            return await _dbContext.UserDiscounts
               .Where(ud => ud.UserId == userId)
               .Include(ud => ud.Discount)
               .ToListAsync();
        }
        #endregion

        #region GetUserDiscountAsync
        public async Task<UserDiscount> GetUserDiscountAsync(int userId, int discountId)
        {
            return await _dbContext.UserDiscounts.SingleOrDefaultAsync(ud => ud.UserId == userId && ud.DiscountId == discountId);
        }
        #endregion

        #endregion
    }
}
