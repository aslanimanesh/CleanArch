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

        // Constructor initializes the product and discount repositories
        public ProductService(IProductRepository productRepository, IDiscountRepository discountRepository)
            : base(productRepository)
        {
            _productRepository = productRepository;
            _discountRepository = discountRepository;
        }

        #endregion

        #region Public Methods

        #region GetDiscountedProductsByUserStatus

        public async Task<IEnumerable<ProductViewModel>> GetDiscountedProductsByUserStatusAsync(int? userId)
        {
            // Get all active discounts that do not require a discount code, filtered by user ID (if provided)
            var activeDiscounts = await _discountRepository.GetAllActiveDiscountsWithoutCodeForUserAsync(userId);

            // Get all products
            var products = await GetAllAsync();
            var productViewModels = new List<ProductViewModel>();

            foreach (Product product in products)
            {
                // Initialize a product view model for each product with default values
                var productViewModel = new ProductViewModel
                {
                    Id = product.Id,
                    Title = product.Title,
                    Description = product.Description,
                    Price = product.Price,
                    ImageName = product.ImageName,
                    OriginalPrice = product.Price,
                    DiscountedPrice = product.Price,
                    DiscountPercentage = 0
                };

                #region User Not Logged In (General Discounts)

                if (!userId.HasValue)
                {
                    // Get discount applicable to all users for a specific product
                    var specificProductDiscount = activeDiscounts
                        .Where(d => d.IsGeneralForUsers && !d.IsGeneralForProducts &&
                                    d.ProductDiscounts != null && d.ProductDiscounts.Any(pd => pd.ProductId == product.Id))
                        .OrderByDescending(d => d.DiscountPercentage)
                        .ThenByDescending(d => d.CreatedDate)
                        .FirstOrDefault();

                    // If specific product discount exists, apply it
                    if (specificProductDiscount != null)
                    {
                        var discountAmount = product.Price * (specificProductDiscount.DiscountPercentage / 100m);
                        productViewModel.DiscountedPrice -= discountAmount;
                        productViewModel.DiscountPercentage = specificProductDiscount.DiscountPercentage;
                    }
                    else
                    {
                        // Otherwise, apply general discount for all users and all products
                        var generalDiscount = activeDiscounts
                            .Where(d => d.IsGeneralForProducts && d.IsGeneralForUsers)
                            .OrderByDescending(d => d.DiscountPercentage)
                            .ThenByDescending(d => d.CreatedDate)
                            .FirstOrDefault();

                        if (generalDiscount != null)
                        {
                            var discountAmount = product.Price * (generalDiscount.DiscountPercentage / 100m);
                            productViewModel.DiscountedPrice -= discountAmount;
                            productViewModel.DiscountPercentage = generalDiscount.DiscountPercentage;
                        }
                    }
                }

                #endregion

                #region User Logged In (Personalized Discounts)

                else
                {
                    // 1. Apply discount for a specific user and a specific product
                    var specificUserSpecificProductDiscount = activeDiscounts
                        .Where(d => !d.IsGeneralForUsers && !d.IsGeneralForProducts &&
                                    d.UserDiscounts.Any(ud => ud.UserId == userId.Value) &&
                                    d.ProductDiscounts.Any(pd => pd.ProductId == product.Id))
                        .OrderByDescending(d => d.DiscountPercentage)
                        .ThenByDescending(d => d.CreatedDate)
                        .FirstOrDefault();

                    if (specificUserSpecificProductDiscount != null)
                    {
                        var discountAmount = product.Price * (specificUserSpecificProductDiscount.DiscountPercentage / 100m);
                        productViewModel.DiscountedPrice -= discountAmount;
                        productViewModel.DiscountPercentage = specificUserSpecificProductDiscount.DiscountPercentage;
                    }
                    else
                    {
                        // 2. Apply discount for a specific user and all products
                        var specificUserAllProductDiscount = activeDiscounts
                            .Where(d => !d.IsGeneralForUsers && d.IsGeneralForProducts &&
                                        d.UserDiscounts.Any(ud => ud.UserId == userId.Value))
                            .OrderByDescending(d => d.DiscountPercentage)
                            .ThenByDescending(d => d.CreatedDate)
                            .FirstOrDefault();

                        if (specificUserAllProductDiscount != null)
                        {
                            var discountAmount = product.Price * (specificUserAllProductDiscount.DiscountPercentage / 100m);
                            productViewModel.DiscountedPrice -= discountAmount;
                            productViewModel.DiscountPercentage = specificUserAllProductDiscount.DiscountPercentage;
                        }
                        else
                        {
                            // 3. Apply discount for all users and a specific product
                            var allUserSpecificProductDiscount = activeDiscounts
                                .Where(d => d.IsGeneralForUsers && !d.IsGeneralForProducts &&
                                            d.ProductDiscounts.Any(pd => pd.ProductId == product.Id))
                                .OrderByDescending(d => d.DiscountPercentage)
                                .ThenByDescending(d => d.CreatedDate)
                                .FirstOrDefault();

                            if (allUserSpecificProductDiscount != null)
                            {
                                var discountAmount = product.Price * (allUserSpecificProductDiscount.DiscountPercentage / 100m);
                                productViewModel.DiscountedPrice -= discountAmount;
                                productViewModel.DiscountPercentage = allUserSpecificProductDiscount.DiscountPercentage;
                            }
                            else
                            {
                                // 4. Apply general discount for all users and all products
                                var allUserAllProductDiscount = activeDiscounts
                                    .Where(d => d.IsGeneralForUsers && d.IsGeneralForProducts)
                                    .OrderByDescending(d => d.DiscountPercentage)
                                    .ThenByDescending(d => d.CreatedDate)
                                    .FirstOrDefault();

                                if (allUserAllProductDiscount != null)
                                {
                                    var discountAmount = product.Price * (allUserAllProductDiscount.DiscountPercentage / 100m);
                                    productViewModel.DiscountedPrice -= discountAmount;
                                    productViewModel.DiscountPercentage = allUserAllProductDiscount.DiscountPercentage;
                                }
                            }
                        }
                    }
                }

                #endregion

                productViewModels.Add(productViewModel);
            }

            return productViewModels;
        }


        #endregion


        #region MyRegion

        public async Task<ProductViewModel> GetProductForShowInBasket(int userId, int productId)
        {
            // دریافت محصول بر اساس شناسه
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return null;

            // دریافت تخفیف‌های فعال
            var activeDiscounts = await _discountRepository.GetAllActiveDiscountsWithoutCodeForUserAsync(userId);

            // ایجاد مدل نمایشی محصول
            var productViewModel = new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                ImageName = product.ImageName,
                OriginalPrice = product.Price,
                DiscountedPrice = product.Price,
                DiscountPercentage = 0
            };

            // تابع داخلی برای اعمال تخفیف
            void ApplyDiscount(decimal discountPercentage)
            {
                var discountAmount = product.Price * (discountPercentage / 100m);
                productViewModel.DiscountedPrice -= discountAmount;
                productViewModel.DiscountPercentage = Convert.ToInt32(discountPercentage);

            }

            // 1. تخفیف برای کاربر خاص و محصول خاص
            var specificUserSpecificProductDiscount = activeDiscounts
                .Where(d => !d.IsGeneralForUsers && !d.IsGeneralForProducts &&
                            d.UserDiscounts.Any(ud => ud.UserId == userId) &&
                            d.ProductDiscounts.Any(pd => pd.ProductId == productId))
                .OrderByDescending(d => d.DiscountPercentage)
                .ThenByDescending(d => d.CreatedDate)
                .FirstOrDefault();

            if (specificUserSpecificProductDiscount != null)
            {
                ApplyDiscount(specificUserSpecificProductDiscount.DiscountPercentage);
            }
            else
            {
                // 2. تخفیف برای کاربر خاص و همه محصولات
                var specificUserAllProductDiscount = activeDiscounts
                    .Where(d => !d.IsGeneralForUsers && d.IsGeneralForProducts &&
                                d.UserDiscounts.Any(ud => ud.UserId == userId))
                    .OrderByDescending(d => d.DiscountPercentage)
                    .ThenByDescending(d => d.CreatedDate)
                    .FirstOrDefault();

                if (specificUserAllProductDiscount != null)
                {
                    ApplyDiscount(specificUserAllProductDiscount.DiscountPercentage);
                }
                else
                {
                    // 3. تخفیف برای همه کاربران و محصول خاص
                    var allUserSpecificProductDiscount = activeDiscounts
                        .Where(d => d.IsGeneralForUsers && !d.IsGeneralForProducts &&
                                    d.ProductDiscounts.Any(pd => pd.ProductId == productId))
                        .OrderByDescending(d => d.DiscountPercentage)
                        .ThenByDescending(d => d.CreatedDate)
                        .FirstOrDefault();

                    if (allUserSpecificProductDiscount != null)
                    {
                        ApplyDiscount(allUserSpecificProductDiscount.DiscountPercentage);
                    }
                    else
                    {
                        // 4. تخفیف عمومی برای همه کاربران و همه محصولات
                        var allUserAllProductDiscount = activeDiscounts
                            .Where(d => d.IsGeneralForUsers && d.IsGeneralForProducts)
                            .OrderByDescending(d => d.DiscountPercentage)
                            .ThenByDescending(d => d.CreatedDate)
                            .FirstOrDefault();

                        if (allUserAllProductDiscount != null)
                        {
                            ApplyDiscount(allUserAllProductDiscount.DiscountPercentage);
                        }
                    }
                }
            }

            // بازگرداندن محصول با اطلاعات تخفیف‌ها (در صورت وجود)
            return productViewModel;
        }

        #endregion

        #endregion

    }
}
