using MyApp.Domain.Models;
using System.Linq.Expressions;

namespace MyApp.Domain.Interfaces
{
    public interface IProductDiscountRepository : IGenericRepository<ProductDiscount>
    {
        Task<ProductDiscount> GetAsync(Expression<Func<ProductDiscount, bool>> predicate);
        Task<List<ProductDiscount>> GetDiscountsForProductAsync(int productId);
        Task<IEnumerable<ProductDiscount>> GetDiscountsForProductsAsync(List<int> productIds, int discountId);


    }
}
