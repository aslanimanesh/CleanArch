using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Products;

namespace MyApp.Application.Services
{
    public class ProductService : GenericService<Product>, IProductService
    {
        #region Fields
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        #endregion

        #region Constructor
        public ProductService(IProductRepository productRepository , IDiscountRepository discountRepository) : base(productRepository)
        {
            _productRepository = productRepository;
            _discountRepository = discountRepository;
        }
        // متدی که تخفیف‌ها را بدون توجه به وضعیت لاگین کاربر برمی‌گرداند
        public async Task<IEnumerable<ProductViewModel>> GetDiscountedProductsAsync()
        {
            var products = await GetAllAsync();
            var discounts = await _discountRepository.GetAllActiveDiscountsAsync();

            var productViewModels = products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                ImageName = product.ImageName,
                OriginalPrice = product.Price,
                DiscountedPrice = CalculateDiscountedPrice(product.Price, discounts),
                DiscountPercentage = GetDiscountPercentage(discounts)
            });

            return productViewModels;
        }

        // متدی که تخفیف‌ها را با توجه به وضعیت لاگین کاربر برمی‌گرداند
        public async Task<IEnumerable<ProductViewModel>> GetDiscountedProductsByUserStatusAsync(int? userId)
        {
            var products = await GetAllAsync();
            var discounts = await _discountRepository.GetAllActiveDiscountsAsync(); // دریافت تخفیف‌های فعال

            var productViewModels = products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                ImageName = product.ImageName,
                OriginalPrice = product.Price,
                // استفاده از لیست تخفیف‌ها برای محاسبه قیمت تخفیف‌دار
                DiscountedPrice = CalculateDiscountedPrice(product.Price, discounts),
                DiscountPercentage = GetDiscountPercentage(discounts)
            });

            return productViewModels;
        }


        private decimal CalculateDiscountedPrice(decimal originalPrice, IEnumerable<Discount> discounts)
        {
            var applicableDiscount = discounts.OrderByDescending(d => d.DiscountPercentage)  // اول درصد تخفیف
                                              .ThenByDescending(d => d.CreatedDate)           // سپس تاریخ ایجاد
                                              .FirstOrDefault();

            if (applicableDiscount != null)
            {
                return originalPrice * (1 - applicableDiscount.DiscountPercentage / 100m);
            }

            return originalPrice; // اگر تخفیفی نباشد، قیمت اصلی برمی‌گردد
        }


        private int GetDiscountPercentage(IEnumerable<Discount> discounts)
        {
            var applicableDiscount = discounts.OrderByDescending(d => d.DiscountPercentage)  // اول درصد تخفیف
                                              .ThenByDescending(d => d.CreatedDate)           // سپس تاریخ ایجاد
                                              .FirstOrDefault();
            return applicableDiscount?.DiscountPercentage ?? 0;
        }

        #endregion
    }

}
