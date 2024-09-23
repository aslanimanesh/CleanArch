using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.ViewModels.Products;

namespace MyApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
           _productRepository = productRepository;
        }
        public ProductViewModel GetProducts()
        {
            return new ProductViewModel()
            {
                Products = _productRepository.GetProduct()
            };
        }
    }
}
