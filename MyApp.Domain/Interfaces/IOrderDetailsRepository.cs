using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetail>
    {
        #region Public Methods

        Task<OrderDetail> ExistProductInOrderDetail(int orderId, int productId);
        Task<List<OrderDetail>> GetAllOrderDetailByOrderId(int orderId);

        #endregion
    }
}
