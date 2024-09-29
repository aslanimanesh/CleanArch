using MyApp.Domain.Models;
using System.Linq.Expressions;

namespace MyApp.Domain.Interfaces
{
    public interface IProductDiscountRepository : IGenericRepository<ProductDiscount>
    {

        #region GetAsync

        Task<ProductDiscount> GetAsync(Expression<Func<ProductDiscount, bool>> predicate);

        #endregion

        #region GetDiscountsForProducts

        Task<IEnumerable<ProductDiscount>> GetDiscountsForProductsAsync(List<int> productIds, int discountId);

        #endregion

    }
}
