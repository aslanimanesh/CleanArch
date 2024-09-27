using MyApp.Domain.Models;

namespace MyApp.Application.Interfaces
{
    public interface IDiscountService : IGenericService<Discount>
    {
        #region Discount Operations
        Task<string> ApplyDiscountToOrderAsync(string discountCode, int orderId, int userId);
        Task<bool> IsExistDiscountCode(string discountCode);
        
        #endregion
    }
}
