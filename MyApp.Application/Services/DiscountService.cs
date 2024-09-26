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
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserDiscountRepository _userDiscountRepository;
        private readonly IProductDiscountRepository _productDiscountRepository;
        private readonly IUsableUserDiscount _usableUserDiscount;

        public DiscoutService(IDiscountRepository discountRepository, IOrderDetailsRepository orderDetailsRepository,
            IOrderRepository orderRepository, IProductRepository productRepository,
            IUserDiscountRepository userDiscountRepository,IProductDiscountRepository productDiscountRepository
            ,IUsableUserDiscount usableUserDiscount) : base(discountRepository) 
        {
            _discountRepository = discountRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userDiscountRepository = userDiscountRepository;
            _productDiscountRepository = productDiscountRepository;
            _usableUserDiscount = usableUserDiscount;
        }

        public async Task<string> ApplyDiscountToOrderAsync(string discountCode, int orderId, int userId)
        {
            Discount discount = await _discountRepository.GetByDiscountCodeAsync(discountCode);

            if (discount == null)
            {
                return "کد تخفیف معتبر نیست.";
            }

            if (!discount.IsActive)
            {
                return "این تخفیف غیر فعال است.";
            }

            if (discount.UsableCount != null && discount.UsableCount <= 1)
            {
                return "تعداد استفاده‌های مجاز از این کد تخفیف به پایان رسیده است.";
            }

            if (discount.StartDate != null && discount.StartDate > DateTime.Now)
            {
                return "تاریخ شروع تخفیف هنوز نرسیده است.";
            }

            if (discount.EndDate != null && discount.EndDate < DateTime.Now)
            {
                return "تاریخ این تخفیف گذشته است.";
            }

            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null)
            {
                return "سفارشی یافت نشد.";
            }

            UserDiscount userDiscount = await _userDiscountRepository.GetUserDiscountAsync(userId , discount.Id);
            if (userDiscount == null)
            {
                return "این کد تخفیف متعلق به شما نمی باشد.";
            }

          
            UsableUserDiscount usableuserDiscount = await _usableUserDiscount.GetUsableUserDiscountAsync(userId, discount.Id);
            if (usableuserDiscount != null)
            {
                return "شما قبلاً از این کد تخفیف استفاده کرده‌اید.";
            }

            var orderProducts = order.OrderDetails.Select(od => od.ProductId).ToList();
            var productDiscounts = await _productDiscountRepository.GetDiscountsForProductsAsync(orderProducts, discount.Id);

            if (!productDiscounts.Any())
            {
                return "این کد تخفیف برای هیچ یک از محصولات سفارش شما معتبر نیست.";
            }

            decimal newTotalPrice = 0;

            foreach (var orderDetail in order.OrderDetails)
            {
                if (productDiscounts.Any(pd => pd.ProductId == orderDetail.ProductId))
                {
                    orderDetail.Price *= (1 - discount.DiscountPercentage / 100);
                }
                newTotalPrice += orderDetail.Price * orderDetail.Count;
            }
            order.Sum = newTotalPrice;


            await _orderRepository.UpdateAsync(order);

            discount.UsableCount -= 1;
            await _discountRepository.UpdateAsync(discount);

            await _usableUserDiscount.AddAsync(new UsableUserDiscount() { UserId = userId ,DiscountId=discount.Id});

            return "کد تخفیف با موفقیت اعمال شد.";
        }
    }

}

