using MyApp.Domain.Models;

namespace MyApp.Application.Interfaces
{
    public interface IOrderDetailsService : IGenericService<OrderDetail>
    {
        #region ExistProductInOrderDetail
        Task<OrderDetail> ExistProductInOrderDetail(int orderId, int productId);
        #endregion

        #region GetAllOrderDetailByOrderId
        Task<List<OrderDetail>> GetAllOrderDetailByOrderId(int orderId);
        #endregion
    }
}
