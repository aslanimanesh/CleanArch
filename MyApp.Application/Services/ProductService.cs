using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Discounts;
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

        #endregion

        #region Public Methods

        #region GetDiscountedProducts

        // متدی که تخفیف‌ها را بدون توجه به وضعیت لاگین کاربر برمی‌گرداند
        public async Task<IEnumerable<ProductViewModel>> GetDiscountedProductsAsync()
        {
            var products = await GetAllAsync();
            var discounts = await _discountRepository.GetAllActiveDiscountsWithoutCodeAsync();

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

        #endregion

        #region GetDiscountedProductsByUserStatus

        public async Task<IEnumerable<ProductViewModel>> GetDiscountedProductsByUserStatusAsync(int? userId)
        {
            var activeDiscounts = await _discountRepository.GetAllActiveDiscountsWithoutCodeAsync();

            // دسته‌بندی تخفیف‌ها
            var generalDiscounts = activeDiscounts
                .Where(d => d.IsGeneralForProducts && d.IsGeneralForUsers)
                .ToList();

            var specificUserDiscounts = activeDiscounts
                .Where(d => !d.IsGeneralForUsers && d.IsGeneralForProducts)
                .ToList();

            var specificProductDiscounts = activeDiscounts
                .Where(d => !d.IsGeneralForProducts && d.IsGeneralForUsers)
                .ToList();

            var specificUserProductDiscounts = activeDiscounts
                .Where(d => !d.IsGeneralForProducts && !d.IsGeneralForUsers)
                .ToList();

            var products = await GetAllAsync();
            var productViewModels = new List<ProductViewModel>();

            foreach (var product in products)
            {
                var productViewModel = new ProductViewModel
                {
                    Id = product.Id,
                    Title = product.Title,
                    Description = product.Description,
                    Price = product.Price,
                    ImageName = product.ImageName,
                    OriginalPrice = product.Price,
                    DiscountedPrice = product.Price, // ابتدا قیمت جدید را برابر با قیمت قبلی قرار می‌دهیم
                    DiscountPercentage = 0 // درصد تخفیف را ابتدا صفر قرار می‌دهیم
                };

                // اعمال تخفیف‌های عمومی
                foreach (var discount in generalDiscounts)
                {
                    var discountAmount = product.Price * (discount.DiscountPercentage / 100m);
                    productViewModel.DiscountedPrice -= discountAmount;
                    productViewModel.DiscountPercentage = discount.DiscountPercentage;
                }

                // اعمال تخفیف‌های مخصوص کاربران
                if (userId.HasValue)
                {
                    foreach (var discount in specificUserDiscounts)
                    {
                        if (discount.UserDiscounts.Any(ud => ud.UserId == userId.Value))
                        {
                            var discountAmount = product.Price * (discount.DiscountPercentage / 100m);
                            productViewModel.DiscountedPrice -= discountAmount;
                            productViewModel.DiscountPercentage = Math.Max(productViewModel.DiscountPercentage ?? 0, discount.DiscountPercentage);
                        }
                    }

                    // اعمال تخفیف‌های مخصوص محصولات
                    foreach (var discount in specificProductDiscounts)
                    {
                        if (discount.ProductDiscounts.Any(pd => pd.ProductId == product.Id))
                        {
                            var discountAmount = product.Price * (discount.DiscountPercentage / 100m);
                            productViewModel.DiscountedPrice -= discountAmount;
                            productViewModel.DiscountPercentage = Math.Max(productViewModel.DiscountPercentage ?? 0, discount.DiscountPercentage);
                        }
                    }

                    // اعمال تخفیف‌های مخصوص کاربران خاص و محصولات خاص
                    foreach (var discount in specificUserProductDiscounts)
                    {
                        if (discount.UserDiscounts.Any(ud => ud.UserId == userId.Value) &&
                            discount.ProductDiscounts.Any(pd => pd.ProductId == product.Id))
                        {
                            var discountAmount = product.Price * (discount.DiscountPercentage / 100m);
                            productViewModel.DiscountedPrice -= discountAmount;
                            productViewModel.DiscountPercentage = Math.Max(productViewModel.DiscountPercentage ?? 0, discount.DiscountPercentage);
                        }
                    }
                }

                productViewModels.Add(productViewModel);
            }

            return productViewModels;
        }

        #endregion

        #region CalculateDiscountedPrice

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

        #endregion

        #region GetDiscountPercentage

        private int GetDiscountPercentage(IEnumerable<Discount> discounts)
        {
            var applicableDiscount = discounts.OrderByDescending(d => d.DiscountPercentage)  // اول درصد تخفیف
                                              .ThenByDescending(d => d.CreatedDate)           // سپس تاریخ ایجاد
                                              .FirstOrDefault();
            return applicableDiscount?.DiscountPercentage ?? 0;
        }

        #endregion

        #endregion

    }

}
