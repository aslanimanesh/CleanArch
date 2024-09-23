using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Infa.Data.Context;

namespace MyApp.Infa.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyAppDbContext _dbContext;

        public ProductRepository(MyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Product> GetProduct()
        {
            return _dbContext.Products;
        }
    }
}
