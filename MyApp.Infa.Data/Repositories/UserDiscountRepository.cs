using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;
using System.Linq.Expressions;

namespace MyApp.Infa.Data.Repositories
{
    public class UserDiscountRepository : GenericRepository<UserDiscount>, IUserDiscountRepository
    {
        private readonly MyAppDbContext _dbContext;

        public UserDiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDiscount> GetAsync(Expression<Func<UserDiscount, bool>> predicate)
        {
            return await _dbContext.UserDiscounts.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<UserDiscount>> GetDiscountsForUserAsync(int userId)
        {
            return await _dbContext.UserDiscounts
           .Where(ud => ud.UserId == userId)
           .Include(ud => ud.Discount)
           .ToListAsync();
        }

        public async Task<UserDiscount> GetUserDiscountAsync(int userId, int discountId)
        {
            return await _dbContext.UserDiscounts
            .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.DiscountId == discountId);
        }
    }
}
