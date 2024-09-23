using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProduct();
    }
}
