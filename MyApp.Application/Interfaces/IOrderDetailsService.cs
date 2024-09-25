using MyApp.Domain.Models;

namespace MyApp.Application.Interfaces
{
    public interface IOrderDetailsService : IGenericService<OrderDetail>
    {
        Task<OrderDetail> ExistProductInOrderDetail(int orderId , int productId);
        Task<List<OrderDetail>> GetAllOrderDetailByOrderId(int orderId);
    }
}
