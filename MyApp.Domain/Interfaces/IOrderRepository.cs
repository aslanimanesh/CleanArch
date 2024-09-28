using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {

        #region HasPendingOrder
        Task<Order> HasPendingOrder(int userId);
        #endregion

        #region GetOrderWithDetailsAsync
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        #endregion

        #region GetOrderTotalPriceAsync
        Task<decimal> GetOrderTotalPriceAsync(int orderId);
        #endregion

        #region UpdatePaymentStatusAsync
        Task UpdatePaymentStatusAsync(int orderId);
        #endregion

    }
}
