using MyApp.Domain.Models;

namespace MyApp.Application.Interfaces
{
    public interface IDiscountService : IGenericService<Discount>
    {
        Task<string> ApplyDiscountToOrderAsync(string discountCode, int orderId, int userId);

    }
}
