using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.AssignDiscount;

namespace MyApp.Application.Interfaces
{
    public interface IDiscountAssignmentToProductService : IGenericService<ProductDiscount>
    {
        Task AssignDiscountToProductsAsync(AssignDiscountToProductViewModel model);
        Task<ProductDiscount> GetProductDiscountAsync(int productId, int discountId);
    }
}
