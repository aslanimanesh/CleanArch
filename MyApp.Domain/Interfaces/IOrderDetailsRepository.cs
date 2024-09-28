using MyApp.Domain.Models;

namespace MyApp.Domain.Interfaces
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetail>
    {

        #region ExistProductInOrderDetail
        Task<OrderDetail> ExistProductInOrderDetail(int orderId, int productId);
        #endregion

        #region GetAllOrderDetailByOrderId
        Task<List<OrderDetail>> GetAllOrderDetailByOrderId(int orderId);
        #endregion

    }
}
