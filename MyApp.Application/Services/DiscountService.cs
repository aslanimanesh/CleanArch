using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Application.Services
{
    public class DiscoutService : GenericService<Discount>, IDiscountService
    {
        #region Fields
        private readonly IDiscountRepository _discountRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserDiscountRepository _userDiscountRepository;
        private readonly IProductDiscountRepository _productDiscountRepository;
        private readonly IUsedUserDiscountRepository _usedUserDiscountRepository;
        private readonly IUsedProductDiscountRepository _usedProductDiscountRepository;

        #endregion

        #region Constructor
        public DiscoutService(IDiscountRepository discountRepository,IOrderDetailsRepository orderDetailsRepository,IOrderRepository orderRepository,
                IProductRepository productRepository,IUserDiscountRepository userDiscountRepository,IProductDiscountRepository productDiscountRepository,
                IUsedUserDiscountRepository usedUserDiscountRepository,IUsedProductDiscountRepository usedProductDiscountRepository)
                : base(discountRepository)
        {
            _discountRepository = discountRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userDiscountRepository = userDiscountRepository;
            _productDiscountRepository = productDiscountRepository;
            _usedUserDiscountRepository = usedUserDiscountRepository;
            _usedProductDiscountRepository = usedProductDiscountRepository;
        }
        #endregion

        #region Public Methods

        #region ApplyDiscountToOrderAsync
        public async Task<string> ApplyDiscountToOrderAsync(string discountCode, int orderId, int userId)
        {
            #region Order Validation

            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null)
            {
                return "سفارشی یافت نشد.";
            }

            #endregion

            #region Discount Validation

            Discount discount = await _discountRepository.GetDiscountByDiscountCodeAsync(discountCode);

            if (discount == null)
            {
                return "کد تخفیف معتبر نیست.";
            }

            if (!discount.IsActive)
            {
                return "این تخفیف غیر فعال است.";
            }

            if (discount.UsableCount.HasValue && discount.UsableCount < 1)
            {
                return "تعداد استفاده‌های مجاز از این کد تخفیف به پایان رسیده است.";
            }

            if (discount.StartDate.HasValue && discount.StartDate > DateTime.Now)
            {
                return "تاریخ شروع تخفیف هنوز نرسیده است.";
            }

            if (discount.EndDate.HasValue && discount.EndDate < DateTime.Now)
            {
                return "تاریخ این تخفیف گذشته است.";
            }

            #endregion

            #region User Discount Usage Validation

            // چک می‌کنیم که آیا این کاربر قبلاً از این تخفیف استفاده کرده است یا خیر
            var usedUserDiscount = await _usedUserDiscountRepository.FindUsedUserDiscountByCodeAsync(userId, discountCode);
               

            if (usedUserDiscount != null)
            {
                return "این کد تخفیف قبلاً توسط این کاربر استفاده شده است.";
            }

            #endregion

            #region Product Discount Usage Validation

            // دریافت محصولات سفارش بر اساس orderId
            var productIds = order.OrderDetails.Select(oi => oi.ProductId).ToList();
            
            if (!productIds.Any())
            {
                return "محصولی در این سفارش یافت نشد.";
            }

            // بررسی اینکه آیا محصولاتی که تخفیف برای آنها اعمال می‌شود در سفارش وجود دارند یا خیر
            if (discount.ProductDiscounts.Any())
            {
                var applicableProductIds = discount.ProductDiscounts.Select(pd => pd.ProductId).ToList();
                var commonProducts = productIds.Intersect(applicableProductIds).ToList();

                if (commonProducts.Any())
                {
                    // بررسی اینکه آیا تخفیف قبلاً برای این محصولات استفاده شده است
                    foreach (var productId in commonProducts)
                    {
                        var usedProductDiscount = await _usedProductDiscountRepository.FindUsedProductDiscountByCodeAsync(productId, discountCode, orderId);

                        if (usedProductDiscount != null)
                        {
                            return $"تخفیف قبلاً برای محصول با شناسه {productId} استفاده شده است.";
                        }
                    }
                }
                else
                {
                    return "این تخفیف برای محصولات این سفارش معتبر نیست.";
                }
            }

            #endregion

            #region Applying Discount

            // ذخیره اطلاعات تخفیف استفاده‌شده برای محصولات مشترک
            if (discount.ProductDiscounts.Any())
            {
                var applicableProductIds = discount.ProductDiscounts.Select(pd => pd.ProductId).ToList();
                var commonProducts = productIds.Intersect(applicableProductIds).ToList();

                foreach (var productId in commonProducts)
                {
                    var usedProductDiscount = new UsedProductDiscount
                    {
                        ProductId = productId,
                        DiscountId = discount.Id,
                        OrderId = orderId,
                        UsedDate = DateTime.Now
                    };
                    await _usedProductDiscountRepository.AddAsync(usedProductDiscount);
                }
            }
            else // در صورتی که تخفیف عمومی باشد یا فقط برای کاربران باشد
            {
                foreach (var productId in productIds)
                {
                    var usedProductDiscount = new UsedProductDiscount
                    {
                        ProductId = productId,
                        DiscountId = discount.Id,
                        OrderId = orderId,
                        UsedDate = DateTime.Now
                    };
                    await _usedProductDiscountRepository.AddAsync(usedProductDiscount);
                }
            }

            // به‌روزرسانی تعداد دفعات استفاده شده از تخفیف و تعداد قابل استفاده
            discount.UsedCount += 1;
            discount.UsableCount -= 1;
            await _discountRepository.UpdateAsync(discount);

            // ذخیره اطلاعات تخفیف استفاده‌شده توسط کاربر
            var newUserDiscount = new UsedUserDiscount
            {
                UserId = userId,
                DiscountId = discount.Id,
                OrderId = orderId,
                UsedDate = DateTime.Now
            };
            await _usedUserDiscountRepository.AddAsync(newUserDiscount);

            return "تخفیف با موفقیت اعمال شد.";

            #endregion
        }

        #endregion

        #region Check DiscountCode duplicate 
        public async Task<bool> IsExistDiscountCode(string discountCode)
        {
            return await _discountRepository.IsExistDiscountCode(discountCode);
        }
        #endregion

        #endregion
    }
}