using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        #region Fields
        private readonly MyAppDbContext _dbContext;
        #endregion

        #region Constructor
        public ProductRepository(MyAppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion
    }
}
