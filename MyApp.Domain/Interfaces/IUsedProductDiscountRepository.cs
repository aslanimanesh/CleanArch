using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IUsedProductDiscountRepository : IGenericRepository<UsedProductDiscount>
    {
        Task<UsedProductDiscount> FindUsedProductDiscountByCodeAsync(int productId, string discountCode, int orderId);
    }
}
