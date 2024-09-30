using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Application.Services
{
    public class DiscountService : GenericService<Discount>, IDiscountService
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

        public DiscountService(IDiscountRepository discountRepository, IOrderDetailsRepository orderDetailsRepository, IOrderRepository orderRepository,
                IProductRepository productRepository, IUserDiscountRepository userDiscountRepository, IProductDiscountRepository productDiscountRepository,
                IUsedUserDiscountRepository usedUserDiscountRepository, IUsedProductDiscountRepository usedProductDiscountRepository)
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
            #region Before Apply Discount

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

            #endregion

            #region Calculate Discount

            decimal discountAmount = 0;

            // 1. تخفیف به کل سفارش برای همه کاربران و همه محصولات
            if (discount.IsGeneralForProducts && discount.IsGeneralForUsers)
            {
                // تخفیف برای کل مبلغ سفارش اعمال می‌شود
                discountAmount = order.Sum * (discount.DiscountPercentage / 100m);
            }
            // 2. تخفیف برای همه محصولات ولی کاربران خاص
            else if (discount.IsGeneralForProducts && !discount.IsGeneralForUsers)
            {
                // بررسی اینکه کاربر خاص مجاز به استفاده از تخفیف باشد
                if (discount.UserDiscounts.Any(ud => ud.UserId == userId))
                {
                    discountAmount = order.Sum * (discount.DiscountPercentage / 100m);
                }
                else
                {
                    return "شما مجاز به استفاده از این کد تخفیف نمی باشید";
                }
            }
            // 3. تخفیف برای همه کاربران ولی محصولات خاص
            else if (!discount.IsGeneralForProducts && discount.IsGeneralForUsers)
            {
                // بررسی اینکه تخفیف برای محصولات خاصی اعمال شود
                bool hasValidProducts = false; // برای پیگیری اینکه آیا محصول معتبر وجود دارد یا نه

                foreach (var orderItem in order.OrderDetails)
                {
                    if (discount.ProductDiscounts.Any(pd => pd.ProductId == orderItem.ProductId))
                    {
                        hasValidProducts = true; // اگر محصول معتبر پیدا شد، مقدار true می‌شود
                        discountAmount += orderItem.OriginalPrice * orderItem.Quantity * (discount.DiscountPercentage / 100m);
                    }
                }

                // بررسی اینکه آیا هیچ محصول معتبری پیدا نشد
                if (!hasValidProducts)
                {
                    return "محصولات سبد خرید شما شامل این کد تخفیف نمی شوند";
                }
            }
            // 4. تخفیف برای کاربران خاص و محصولات خاص
            else if (!discount.IsGeneralForProducts && !discount.IsGeneralForUsers)
            {
                // بررسی اینکه کاربر و محصول مجاز به استفاده از تخفیف باشند
                if (discount.UserDiscounts.Any(ud => ud.UserId == userId))
                {
                    foreach (var orderItem in order.OrderDetails)
                    {
                        if (discount.ProductDiscounts.Any(pd => pd.ProductId == orderItem.ProductId))
                        {
                            discountAmount += orderItem.Quantity * orderItem.OriginalPrice * (discount.DiscountPercentage / 100m);
                        }
                    }
                }
                else
                {
                    return "این کد تخفیف متعلق به شما نمی باشد";
                }
            }

            // محاسبه نهایی مبلغ سفارش با تخفیف
            //var sumWithDiscount = order.Sum - discountAmount;

            #endregion

            #region Apply Discount
            order.Sum = order.Sum - discountAmount;
            await _orderRepository.UpdateAsync(order);
            #endregion
                   
            #region After Apply Discount

            if (discount.UsableCount.HasValue)
            {
                discount.UsableCount -= 1;
            }
            discount.UsedCount += 1;

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