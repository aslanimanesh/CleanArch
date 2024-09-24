using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IProductDiscountRepository : IGenericRepository<ProductDiscount>
    {
        Task<List<ProductDiscount>> GetDiscountsByProductIdAsync(int productId);
    }
}
