using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IUsedProductDiscountRepository : IGenericRepository<UsedProductDiscount>
    {
        #region FindUsedProductDiscountByCode

        Task<UsedProductDiscount> FindUsedProductDiscountByCodeAsync(int productId, string discountCode, int orderId);

        #endregion
    }
}
