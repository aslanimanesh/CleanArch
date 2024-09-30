using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Products;

namespace MyApp.Application.Interfaces
{
    public interface IProductService : IGenericService<Product>
    {

        #region GetDiscountedProducts

        //<IEnumerable<ProductViewModel>> GetDiscountedProductsAsync();

        #endregion

        #region GetDiscountedProductsByUserStatus

        Task<IEnumerable<ProductViewModel>> GetDiscountedProductsByUserStatusAsync(int? userId);

        #endregion

    }
}
