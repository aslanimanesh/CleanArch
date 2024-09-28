namespace MyApp.Application.Interfaces
{
    public interface IPaymentService
    {
        #region CreatePaymentAsync
        Task<(bool success, string redirectUrl, string errorMessage)> CreatePaymentAsync(int orderId, decimal amount, string callbackUrl);

        #endregion

        #region VerifyPaymentAsync
        Task<(bool success, string message)> VerifyPaymentAsync(string authority, decimal amount);

        #endregion

    }
}
