using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.AssignDiscount;

namespace MyApp.Application.Services
{
    public class DiscountAssignmentToProductService : GenericService<ProductDiscount>, IDiscountAssignmentToProductService
    {
        private readonly IProductDiscountRepository _productDiscountRepository;

        public DiscountAssignmentToProductService(IProductDiscountRepository productDiscountRepository) : base(productDiscountRepository)
        {
            _productDiscountRepository = productDiscountRepository;
        }

        public async Task AssignDiscountToProductsAsync(AssignDiscountToProductViewModel model)
        {
            var productDiscounts = model.ProductIds.Select(productId => new ProductDiscount
            {
                ProductId = productId,
                DiscountId = model.DiscountId
            });

            foreach (var pd in productDiscounts)
            {
                await _productDiscountRepository.AddAsync(pd);
            }
        }

        public async Task<ProductDiscount> GetProductDiscountAsync(int productId, int discountId)
        {
            return await _productDiscountRepository.GetAsync(pd => pd.ProductId == productId && pd.DiscountId == discountId);
        }
    }
}
