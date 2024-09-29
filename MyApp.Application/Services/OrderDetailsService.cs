using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Application.Services
{
    public class OrderDetailsService : GenericService<OrderDetail>, IOrderDetailsService
    {
        #region Fields

        private readonly IOrderDetailsRepository _orderDetailsRepository;

        #endregion

        #region Constructor

        public OrderDetailsService(IOrderDetailsRepository orderDetailsRepository) : base(orderDetailsRepository)
        {
            _orderDetailsRepository = orderDetailsRepository;
        }

        #endregion

        #region Public Methods

        #region ExistProductInOrderDetail

        public async Task<OrderDetail> ExistProductInOrderDetail(int orderId, int productId)
        {
            return await _orderDetailsRepository.ExistProductInOrderDetail(orderId, productId);
        }

        #endregion

        #region GetAllOrderDetailByOrderId

        public async Task<List<OrderDetail>> GetAllOrderDetailByOrderId(int orderId)
        {
            return await _orderDetailsRepository.GetAllOrderDetailByOrderId(orderId);
        }

        #endregion

        #endregion
    }
}
