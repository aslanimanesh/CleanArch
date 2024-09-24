using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.AssignDiscount;

namespace MyApp.Application.Interfaces
{
    public interface IDiscountAssignmentService : IGenericService<ProductDiscount>
    {
        Task AssignDiscountToProductsAsync(AssignDiscountViewModel model);
        Task RemoveDiscountFromProductAsync(int productId, int discountId);
    }
}
