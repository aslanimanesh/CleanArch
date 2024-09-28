using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {

        #region GetByDiscountCodeAsync
        Task<Discount> GetDiscountByDiscountCodeAsync(string discountCode);
        #endregion

        #region Check DiscountCode duplicate 
        Task<bool> IsExistDiscountCode(string discountCode);
        #endregion

    }
}
