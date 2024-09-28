using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class UsedProductDiscountRepository : GenericRepository<UsedProductDiscount>, IUsedProductDiscountRepository
    {
        private readonly MyAppDbContext _dbContext;

        public UsedProductDiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
