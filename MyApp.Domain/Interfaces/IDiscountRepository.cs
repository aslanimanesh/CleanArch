using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        #region Public Methods

        Task<Discount> GetByDiscountCodeAsync(string discountCode);
        Task<bool> IsExistDiscountCode(string discountCode);

        #endregion
    }
}
