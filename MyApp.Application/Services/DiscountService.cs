using Microsoft.AspNetCore.Http;
using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;
using System.Security.Claims;

namespace MyApp.Application.Services
{
    public class DiscoutService : GenericService<Discount>, IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IUserDiscountRepository _userDiscountRepository;
        private readonly IProductDiscountRepository _productDiscountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DiscoutService(IDiscountRepository discountRepository, IOrderDetailsService orderDetailsService,
            IOrderService orderService , IProductService productService ,
            IUserDiscountRepository userDiscountRepository,IProductDiscountRepository productDiscountRepository,
            IHttpContextAccessor httpContextAccessor) : base(discountRepository) 
        {
            _discountRepository = discountRepository;
            _orderDetailsService = orderDetailsService;
            _orderService = orderService;
            _productService = productService;
            _userDiscountRepository = userDiscountRepository;
            _productDiscountRepository = productDiscountRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCurrentUserId()
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User;
            var user = userIdStr.FindFirst(ClaimTypes.NameIdentifier);
            
            throw new Exception("User not found or not authenticated.");
        }

        public async Task<string> ApplyDiscountToOrderAsync(string discountCode, int orderId, int? userId)
        {

            // Get UserId
            //int userId = userId;


            // قدم 1: بررسی کد تخفیف
            var discount = await _discountRepository.GetByDiscountCodeAsync(discountCode);
            if (discount == null)
            {
                return "کد تخفیف معتبر نیست.";
            }

            // قدم 2: بررسی تاریخ اعتبار کد تخفیف
            if (discount.StartDate != null && discount.StartDate > DateTime.Now)
            {
                //     return "تاریخ شروع تخفیف هنوز نرسیده است.";
            }

            if (discount.EndDate != null && discount.EndDate < DateTime.Now)
            {
                return "تاریخ این تخفیف گذشته است.";
            }

            // قدم 3: بررسی تعداد استفاده‌های مجاز
            if (discount.UsableCount != null && discount.UsableCount <= 1)
            {
                return "تعداد استفاده‌های مجاز از این کد تخفیف به پایان رسیده است.";
            }

            // قدم 4: بررسی فاکتور و محصولات
            //var order = await _orderService.GetOrderWithDetailsAsync(orderId);
            var order = await _orderService.GetOrderWithDetailsAsync(orderId);
            if (order == null)
            {
                return "سفارشی با این شماره یافت نشد.";
            }


            var orderProducts = order.OrderDetails.Select(od => od.ProductId).ToList();

            var productDiscounts = await _productDiscountRepository.GetDiscountsForProductsAsync(orderProducts, discount.Id);

            if (!productDiscounts.Any())
            {
                return "این کد تخفیف برای هیچ یک از محصولات سفارش شما معتبر نیست.";
            }

            // قدم 5: بررسی اینکه آیا کاربر قبلاً از این کد تخفیف استفاده کرده است
            if (userId.HasValue)
            {
                var userDiscount = await _userDiscountRepository.GetUserDiscountAsync(userId.Value, discount.Id);
                if (userDiscount != null)
                {
                    return "شما قبلاً از این کد تخفیف استفاده کرده‌اید.";
                }
            }

            // قدم 6: اعمال تخفیف بر روی محصولات سفارش
            foreach (var orderDetail in order.OrderDetails)
            {
                if (productDiscounts.Any(pd => pd.ProductId == orderDetail.ProductId))
                {
                    orderDetail.Price *= (1 - discount.DiscountPercentage / 100);
                }
            }

            // ذخیره فاکتور به‌روزرسانی شده
            await _orderService.UpdateAsync(order);

            // به‌روزرسانی تعداد استفاده از کد تخفیف
            discount.UsableCount -= 1;
            await _discountRepository.UpdateAsync(discount);

            return "کد تخفیف با موفقیت اعمال شد.";
        }
    }

}

