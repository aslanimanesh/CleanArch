using MyApp.Domain.Models;
using System.Linq.Expressions;

namespace MyApp.Domain.Interfaces
{
    public interface IUserDiscountRepository : IGenericRepository<UserDiscount>
    {

        #region GetAsync
        Task<UserDiscount> GetAsync(Expression<Func<UserDiscount, bool>> predicate);
        #endregion

        #region GetUserDiscountAsync
        Task<UserDiscount> GetUserDiscountAsync(int userId, int discountId);
        #endregion

    }
}
