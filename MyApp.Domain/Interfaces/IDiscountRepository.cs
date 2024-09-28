using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {

        #region GetByDiscountCodeAsync
        Task<Discount> GetByDiscountCodeAsync(string discountCode);
        #endregion

        #region IsExistDiscountCode
        Task<bool> IsExistDiscountCode(string discountCode);
        #endregion

    }
}
