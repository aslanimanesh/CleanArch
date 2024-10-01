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
        private readonly IOrderDetailsService _orderDetailsService;

        #endregion

        #region Constructor

        public DiscountService(IDiscountRepository discountRepository, IOrderDetailsRepository orderDetailsRepository, IOrderRepository orderRepository,
                IProductRepository productRepository, IUserDiscountRepository userDiscountRepository, IProductDiscountRepository productDiscountRepository,
                IUsedUserDiscountRepository usedUserDiscountRepository, IUsedProductDiscountRepository usedProductDiscountRepository,
                IOrderDetailsService orderDetailsService)
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
            _orderDetailsService = orderDetailsService;
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

            await ResetDiscountedPricesInOrderAsync(orderId);

            #region Calculate Discount

            //decimal discountAmount = 0;

            //// 1. Discount applies to the entire order for all users and all products
            //if (discount.IsGeneralForProducts && discount.IsGeneralForUsers)
            //{
            //    // The discount is applied to the total order amount
            //    discountAmount = order.Sum * (discount.DiscountPercentage / 100m);
            //}
            //// 2. Discount applies to all products but for specific users
            //else if (discount.IsGeneralForProducts && !discount.IsGeneralForUsers)
            //{
            //    // Check if the specific user is eligible for the discount
            //    if (discount.UserDiscounts.Any(ud => ud.UserId == userId))
            //    {
            //        discountAmount = order.Sum * (discount.DiscountPercentage / 100m);
            //    }
            //    else
            //    {
            //        return "You are not authorized to use this discount code.";
            //    }
            //}
            //// 3. Discount applies to all users but for specific products
            //else if (!discount.IsGeneralForProducts && discount.IsGeneralForUsers)
            //{
            //    // Check if the discount applies to specific products
            //    bool hasValidProducts = false; // To track if a valid product is found

            //    foreach (var orderItem in order.OrderDetails)
            //    {
            //        if (discount.ProductDiscounts.Any(pd => pd.ProductId == orderItem.ProductId))
            //        {
            //            hasValidProducts = true; // Set to true if a valid product is found
            //            discountAmount += orderItem.OriginalPrice * orderItem.Quantity * (discount.DiscountPercentage / 100m);
            //        }
            //    }

            //    // Check if no valid products were found
            //    if (!hasValidProducts)
            //    {
            //        return "The items in your cart are not eligible for this discount code.";
            //    }
            //}
            //// 4. Discount applies to specific users and specific products
            //else if (!discount.IsGeneralForProducts && !discount.IsGeneralForUsers)
            //{
            //    // Check if both the user and products are eligible for the discount
            //    if (discount.UserDiscounts.Any(ud => ud.UserId == userId))
            //    {
            //        foreach (var orderItem in order.OrderDetails)
            //        {
            //            if (discount.ProductDiscounts.Any(pd => pd.ProductId == orderItem.ProductId))
            //            {
            //                discountAmount += orderItem.Quantity * orderItem.OriginalPrice * (discount.DiscountPercentage / 100m);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        return "This discount code does not belong to you.";
            //    }
            //}

            #endregion

            #region Calculate Discount

            decimal discountAmount = 0;

            // Apply discount logic based on product and user eligibility
            foreach (var orderItem in order.OrderDetails)
            {
                decimal itemDiscountAmount = 0;

                // 1. Discount applies to the entire order for all users and all products
                if (discount.IsGeneralForProducts && discount.IsGeneralForUsers)
                {
                    itemDiscountAmount = orderItem.OriginalPrice * orderItem.Quantity * (discount.DiscountPercentage / 100m);
                }
                // 2. Discount applies to all products but for specific users
                else if (discount.IsGeneralForProducts && !discount.IsGeneralForUsers)
                {
                    if (discount.UserDiscounts.Any(ud => ud.UserId == userId))
                    {
                        itemDiscountAmount = orderItem.OriginalPrice * orderItem.Quantity * (discount.DiscountPercentage / 100m);
                    }
                    else
                    {
                        return "You are not authorized to use this discount code.";
                    }
                }
                // 3. Discount applies to all users but for specific products
                else if (!discount.IsGeneralForProducts && discount.IsGeneralForUsers)
                {
                    if (discount.ProductDiscounts.Any(pd => pd.ProductId == orderItem.ProductId))
                    {
                        itemDiscountAmount = orderItem.OriginalPrice * orderItem.Quantity * (discount.DiscountPercentage / 100m);
                    }
                }
                // 4. Discount applies to specific users and specific products
                else if (!discount.IsGeneralForProducts && !discount.IsGeneralForUsers)
                {
                    if (discount.UserDiscounts.Any(ud => ud.UserId == userId) && discount.ProductDiscounts.Any(pd => pd.ProductId == orderItem.ProductId))
                    {
                        itemDiscountAmount = orderItem.OriginalPrice * orderItem.Quantity * (discount.DiscountPercentage / 100m);
                    }
                }

                // Update order item details
                if (itemDiscountAmount > 0)
                {
                    discountAmount += itemDiscountAmount;

                    // Update final price and discount percentage for the order item
                    orderItem.FinalPrice = orderItem.OriginalPrice - (orderItem.OriginalPrice * (discount.DiscountPercentage / 100m));
                    orderItem.DiscountPercentage = discount.DiscountPercentage;

                    // Save changes to order item in the database
                    await _orderDetailsService.UpdateAsync(orderItem);
                }
            }

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

            if (discount.UsedCount == null)
            {
                discount.UsedCount = 1;
            }
            else
            {
                discount.UsedCount += 1;
            }
            

            await _discountRepository.UpdateAsync(discount);

            // Save Information User 
            var newUserDiscount = new UsedUserDiscount
            {
                UserId = userId,
                DiscountId = discount.Id,
                OrderId = orderId,
                UsedDate = DateTime.Now
            };
            await _usedUserDiscountRepository.AddAsync(newUserDiscount);

            return "Success";
            

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


        #region Private Method
        private async Task ResetDiscountedPricesInOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null) return; // Exit the method if the order is not found

            decimal newTotalSum = 0; // Variable to store the new total sum of the order

            // Check and update the final prices of products
            foreach (var orderDetail in order.OrderDetails)
            {
                if (orderDetail.DiscountPercentage > 0)
                {
                    // If the discount percentage is greater than 0, revert the final price to the original price
                    orderDetail.FinalPrice = orderDetail.OriginalPrice;
                    await _orderDetailsService.UpdateAsync(orderDetail); // Update the order details in the database
                }

                // Calculate the new total sum using the updated final price
                newTotalSum += Convert.ToDecimal(orderDetail.FinalPrice) * Convert.ToDecimal(orderDetail.Quantity);
            }

            // Update the order total sum
            order.Sum = newTotalSum;
            await _orderRepository.UpdateAsync(order); // Update the order in the database
        }

        #endregion


    }
}