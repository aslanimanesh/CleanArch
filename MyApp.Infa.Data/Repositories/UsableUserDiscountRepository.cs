using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class UsableUserDiscountRepository : GenericRepository<UsableUserDiscount>, IUsableUserDiscount
    {
        #region Fields
        private readonly MyAppDbContext _dbContext;
        #endregion

        #region Constructor
        public UsableUserDiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Public Methods

        #region GetUsableUserDiscountAsync
        public async Task<UsableUserDiscount> GetUsableUserDiscountAsync(int userId, int discountId)
        {
            return await _dbContext.UsableUserDiscounts.SingleOrDefaultAsync(ud => ud.UserId == userId && ud.DiscountId == discountId);
        }
        #endregion

        #endregion
    }
}
