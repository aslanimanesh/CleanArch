using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Products;

namespace MyApp.Application.Interfaces
{
    public interface IProductService : IGenericService<Product>
    {
        Task<IEnumerable<ProductViewModel>> GetDiscountedProductsAsync();
        Task<IEnumerable<ProductViewModel>> GetDiscountedProductsByUserStatusAsync(int? userId);
    }
}
