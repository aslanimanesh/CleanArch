using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Application.Services
{
    public class OrderService : GenericService<Order>, IOrderService
    {
        #region Fields

        private readonly IOrderRepository _orderRepository;

        #endregion

        #region Constructor

        public OrderService(IOrderRepository orderRepository) : base(orderRepository)
        {
            _orderRepository = orderRepository;
        }

        #endregion

        #region Public Methods

        #region GetOrderTotalAsync

        public async Task<decimal> GetOrderTotalAsync(int orderId)
        {
            return await _orderRepository.GetOrderTotalPriceAsync(orderId);
        }

        #endregion

        #region GetOrderWithDetailsAsync

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            return await _orderRepository.GetOrderWithDetailsAsync(orderId);
        }

        #endregion

        #region HasPendingOrder

        public async Task<Order> HasPendingOrder(int userId)
        {
            return await _orderRepository.HasPendingOrder(userId);
        }

        #endregion

        #region UpdatePaymentStatusAsync

        public async Task UpdatePaymentStatusAsync(int orderId)
        {
            await _orderRepository.UpdatePaymentStatusAsync(orderId);
        }

        #endregion

        #endregion
    }
}
