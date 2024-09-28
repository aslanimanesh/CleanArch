using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;

public class PaymentController : Controller
{
    #region Fields
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    #endregion

    #region Constructor
    public PaymentController(IOrderService orderService, IPaymentService paymentService)
    {
        _orderService = orderService;
        _paymentService = paymentService;
    }
    #endregion

    #region Public Methods

    #region Payment

    [HttpPost]
    public async Task<IActionResult> Pay(int orderId, decimal amount)
    {
        var callbackUrl = Url.Action("Verify", "Payment", new { orderId = orderId }, Request.Scheme);
        var (success, redirectUrl, errorMessage) = await _paymentService.CreatePaymentAsync(orderId, amount, callbackUrl);

        if (success)
        {
            return Redirect(redirectUrl);
        }
        else
        {
            ViewBag.Error = $"خطا در ایجاد پرداخت: {errorMessage}";
            return View("Error");
        }
    }
    #endregion

    #region VerifyPayment

    [HttpGet]
    public async Task<IActionResult> Verify(int orderId, string authority, decimal amount)
    {
        var (success, message) = await _paymentService.VerifyPaymentAsync(authority, amount);

        if (success)
        {
            await _orderService.UpdatePaymentStatusAsync(orderId);
        }

        ViewBag.Message = message;
        return View("Index");
    }
    #endregion

    #endregion

}
