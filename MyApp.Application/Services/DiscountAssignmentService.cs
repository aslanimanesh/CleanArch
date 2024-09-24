using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.AssignDiscount;

namespace MyApp.Application.Services
{
    public class DiscountAssignmentService : GenericService<ProductDiscount>, IDiscountAssignmentService
    {
        private readonly IProductDiscountRepository _productDiscountRepository;

        public DiscountAssignmentService(IProductDiscountRepository productDiscountRepository) : base(productDiscountRepository)
        {
            _productDiscountRepository = productDiscountRepository;
        }

        public async Task AssignDiscountToProductsAsync(AssignDiscountViewModel model)
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

        public async Task RemoveDiscountFromProductAsync(int productId, int discountId)
        {
            var productDiscounts = await _productDiscountRepository.GetDiscountsByProductIdAsync(productId);
            var discountToRemove = productDiscounts.FirstOrDefault(pd => pd.DiscountId == discountId);

            if (discountToRemove != null)
            {
                await _productDiscountRepository.DeleteAsync(discountToRemove);
            }
        }
    }
}
