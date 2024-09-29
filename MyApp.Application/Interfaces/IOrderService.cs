using MyApp.Domain.Models;

namespace MyApp.Application.Interfaces
{
    public interface IOrderService : IGenericService<Order>
    {

        #region HasPendingOrder

        Task<Order> HasPendingOrder(int userId);

        #endregion

        #region GetOrderWithDetailsAsync

        Task<Order> GetOrderWithDetailsAsync(int orderId);

        #endregion

        #region GetOrderTotalAsync

        Task<decimal> GetOrderTotalAsync(int orderId);

        #endregion

        #region UpdatePaymentStatusAsync

        Task UpdatePaymentStatusAsync(int orderId);

        #endregion

    }
}
