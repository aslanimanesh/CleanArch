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
        public DiscoutService(IDiscountRepository discountRepository, IOrderDetailsRepository orderDetailsRepository,
        IOrderRepository orderRepository, IProductRepository productRepository,
        IUserDiscountRepository userDiscountRepository, IProductDiscountRepository productDiscountRepository
        , IUsedUserDiscountRepository usedUserDiscountRepository, IUsedProductDiscountRepository usedProductDiscountRepository) : base(discountRepository)
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

            #region Discount Validation

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

            #endregion

            #region Order Validation

            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null)
            {
                return "سفارشی یافت نشد.";
            }

            #endregion


            return "Ok";


            #endregion

            #endregion
        }

        public Task<bool> IsExistDiscountCode(string discountCode)
        {
            throw new NotImplementedException();
        }
    }
}