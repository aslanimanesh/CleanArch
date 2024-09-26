using MyApp.Domain.Models;

namespace MyApp.Application.Interfaces
{
    public interface IOrderService : IGenericService<Order>
    {
        Task<Order> HasPendingOrder(int userId);
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        Task<decimal> GetOrderTotalAsync(int orderId);
    }
}
