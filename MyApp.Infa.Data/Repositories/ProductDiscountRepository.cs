using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class ProductDiscountRepository : GenericRepository<ProductDiscount>, IProductDiscountRepository
    {
        private readonly MyAppDbContext _dbContext;

        public ProductDiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductDiscount>> GetDiscountsByProductIdAsync(int productId)
        {
            return await _dbContext.Set<ProductDiscount>()
                .Where(pd => pd.ProductId == productId)
                .ToListAsync();
        }
    }
}
