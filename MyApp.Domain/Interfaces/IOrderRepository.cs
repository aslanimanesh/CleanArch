using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> HasPendingOrder(int userId);
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        Task<decimal> GetOrderTotalPriceAsync(int orderId);

    }
}
