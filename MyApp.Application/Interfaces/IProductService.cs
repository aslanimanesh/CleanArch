using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Products;

namespace MyApp.Application.Interfaces
{
    public interface IProductService : IGenericService<Product>
    {

        #region GetDiscountedProductsByUserStatus

        Task<IEnumerable<ProductViewModel>> GetDiscountedProductsByUserStatusAsync(int? userId);

        Task<ProductViewModel> GetProductForShowInBasket(int userId, int productId);

        #endregion

    }
}
