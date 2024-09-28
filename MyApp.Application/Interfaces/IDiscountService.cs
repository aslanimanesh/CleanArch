using MyApp.Domain.Models;

namespace MyApp.Application.Interfaces
{
    public interface IDiscountService : IGenericService<Discount>
    {
        #region ApplyDiscountToOrderAsync
        Task<string> ApplyDiscountToOrderAsync(string discountCode, int orderId, int userId);
        #endregion

        #region Check DiscountCode duplicate 
        Task<bool> IsExistDiscountCode(string discountCode);

        #endregion

    }
}
