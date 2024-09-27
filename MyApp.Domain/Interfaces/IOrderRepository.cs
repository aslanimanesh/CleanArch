using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        #region Public Methods

        Task<Order> HasPendingOrder(int userId);
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        Task<decimal> GetOrderTotalPriceAsync(int orderId);

        #endregion
    }
}
