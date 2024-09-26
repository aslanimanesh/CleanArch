using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        private readonly MyAppDbContext _dbContext;

        public DiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Discount> GetByDiscountCodeAsync(string discountCode)
        {
            return await _dbContext.Discounts.SingleOrDefaultAsync(d => d.DiscountCode == discountCode);
        }
    }
}
