using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class UsedUserDiscountRepository : GenericRepository<UsedUserDiscount>, IUsedUserDiscountRepository
    {
        private readonly MyAppDbContext _dbContext;

        public UsedUserDiscountRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
