using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Application.Services
{
    public class OrderDetailsService : GenericService<OrderDetail>, IOrderDetailsService
    {
        private readonly IOrderDetailsRepository _orderDetailsRepository;

        public OrderDetailsService(IOrderDetailsRepository orderDetailsRepository) : base(orderDetailsRepository)
        {
            _orderDetailsRepository = orderDetailsRepository;
        }

        public async Task<OrderDetail> ExistProductInOrderDetail(int orderId, int productId)
        {
            return await _orderDetailsRepository.ExistProductInOrderDetail(orderId, productId);
        }

        public async Task<List<OrderDetail>> GetAllOrderDetailByOrderId(int orderId)
        {
            return await _orderDetailsRepository.GetAllOrderDetailByOrderId(orderId);
        }
    }
}
