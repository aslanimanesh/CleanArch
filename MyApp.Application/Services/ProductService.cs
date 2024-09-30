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

        #endregion

        #region Public Methods

        #region GetDiscountedProducts

        // متدی که تخفیف‌ها را بدون توجه به وضعیت لاگین کاربر برمی‌گرداند
        //public async Task<IEnumerable<ProductViewModel>> GetDiscountedProductsAsync()
        //{
        //    var products = await GetAllAsync();
        //    var discounts = await _discountRepository.GetAllActiveDiscountsWithoutCodeAsync();

        //    var productViewModels = products.Select(product => new ProductViewModel
        //    {
        //        Id = product.Id,
        //        Title = product.Title,
        //        ImageName = product.ImageName,
        //        OriginalPrice = product.Price,
        //        DiscountedPrice = CalculateDiscountedPrice(product.Price, discounts),
        //        DiscountPercentage = GetDiscountPercentage(discounts)
        //    });

        //    return productViewModels;
        //}

        #endregion

        #region GetDiscountedProductsByUserStatus

        public async Task<IEnumerable<ProductViewModel>> GetDiscountedProductsByUserStatusAsync(int? userId)
        {
            // دریافت تخفیف‌های فعال متناسب با کاربر
            var activeDiscounts = await _discountRepository.GetAllActiveDiscountsWithoutCodeForUserAsync(userId);


            // دریافت همه محصولات
            var products = await GetAllAsync();
            var productViewModels = new List<ProductViewModel>();

            foreach (Product product in products)
            {
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
                // اگر کاربر لاگین نکرده باشد
                if (!userId.HasValue)
                {               

                    // ابتدا تخفیف شامل همه کاربران و محصول خاص
                    var specificProductDiscount = activeDiscounts
                        .Where(d => d.IsGeneralForUsers && !d.IsGeneralForProducts &&
                                    d.ProductDiscounts != null && d.ProductDiscounts.Any(pd => pd.ProductId.Equals(product.Id)))
                        .OrderByDescending(d => d.DiscountPercentage)
                        .ThenByDescending(d => d.CreatedDate)
                        .FirstOrDefault();

                    // اگر تخفیف شامل همه کاربران و محصول خاص وجود دارد
                    if (specificProductDiscount != null)
                    {
                        var discountAmount = product.Price * (specificProductDiscount.DiscountPercentage / 100m);
                        productViewModel.DiscountedPrice -= discountAmount;
                        productViewModel.DiscountPercentage = specificProductDiscount.DiscountPercentage;
                    }
                    else
                    {
                        // اگر تخفیف محصول خاص وجود نداشت، بررسی تخفیف شامل همه کاربران و همه محصولات
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


                else
                {
                    // 1. تخفیف برای کاربر خاص و محصول خاص
                    var specificUserSpecificProductDiscount = activeDiscounts
                        .Where(d => !d.IsGeneralForUsers && !d.IsGeneralForProducts &&
                                    d.UserDiscounts.Any(ud => ud.UserId == userId.Value) &&
                                    d.ProductDiscounts.Any(pd => pd.ProductId == product.Id))
                        .OrderByDescending(d => d.DiscountPercentage)
                        .ThenByDescending(d => d.CreatedDate)
                        .FirstOrDefault();

                    if (specificUserSpecificProductDiscount != null)
                    {
                        // اگر تخفیف برای کاربر خاص و محصول خاص وجود داشت، تخفیف را اعمال کن
                        var discountAmount = product.Price * (specificUserSpecificProductDiscount.DiscountPercentage / 100m);
                        productViewModel.DiscountedPrice -= discountAmount;
                        productViewModel.DiscountPercentage = specificUserSpecificProductDiscount.DiscountPercentage;
                    }
                    else
                    {
                        // 2. تخفیف برای کاربر خاص و همه محصولات
                        var specificUserAllProductDiscount = activeDiscounts
                            .Where(d => !d.IsGeneralForUsers && d.IsGeneralForProducts &&
                                        d.UserDiscounts.Any(ud => ud.UserId == userId.Value))
                            .OrderByDescending(d => d.DiscountPercentage)
                            .ThenByDescending(d => d.CreatedDate)
                            .FirstOrDefault();

                        if (specificUserAllProductDiscount != null)
                        {
                            // اگر تخفیف برای کاربر خاص و همه محصولات وجود داشت، تخفیف را اعمال کن
                            var discountAmount = product.Price * (specificUserAllProductDiscount.DiscountPercentage / 100m);
                            productViewModel.DiscountedPrice -= discountAmount;
                            productViewModel.DiscountPercentage = specificUserAllProductDiscount.DiscountPercentage;
                        }
                        else
                        {
                            // 3. تخفیف برای محصول خاص و همه کاربران
                            var allUserSpecificProductDiscount = activeDiscounts
                                .Where(d => d.IsGeneralForUsers && !d.IsGeneralForProducts &&
                                            d.ProductDiscounts.Any(pd => pd.ProductId == product.Id))
                                .OrderByDescending(d => d.DiscountPercentage)
                                .ThenByDescending(d => d.CreatedDate)
                                .FirstOrDefault();

                            if (allUserSpecificProductDiscount != null)
                            {
                                // اگر تخفیف برای محصول خاص و همه کاربران وجود داشت، تخفیف را اعمال کن
                                var discountAmount = product.Price * (allUserSpecificProductDiscount.DiscountPercentage / 100m);
                                productViewModel.DiscountedPrice -= discountAmount;
                                productViewModel.DiscountPercentage = allUserSpecificProductDiscount.DiscountPercentage;
                            }
                            else
                            {
                                // 4. تخفیف برای همه کاربران و همه محصولات
                                var allUserAllProductDiscount = activeDiscounts
                                    .Where(d => d.IsGeneralForUsers && d.IsGeneralForProducts)
                                    .OrderByDescending(d => d.DiscountPercentage)
                                    .ThenByDescending(d => d.CreatedDate)
                                    .FirstOrDefault();

                                if (allUserAllProductDiscount != null)
                                {
                                    // اگر تخفیف برای همه کاربران و همه محصولات وجود داشت، تخفیف را اعمال کن
                                    var discountAmount = product.Price * (allUserAllProductDiscount.DiscountPercentage / 100m);
                                    productViewModel.DiscountedPrice -= discountAmount;
                                    productViewModel.DiscountPercentage = allUserAllProductDiscount.DiscountPercentage;
                                }
                            }
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
