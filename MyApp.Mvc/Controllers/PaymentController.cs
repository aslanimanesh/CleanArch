using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using Newtonsoft.Json;
using System.Text;

public class PaymentController : Controller
{
    
    private const string MerchantId = "cdb9caf7-008b-439c-b6e0-3f4bd44b2476";
    private const string PaymentUrl = "https://payment.zarinpal.com/pg/v4/payment/request.json";

    private readonly IOrderService _orderService;

    public PaymentController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Pay(int OrderId,int UserId,decimal Amount)
    {
        int amountToRial = (int)(Amount * 10);

        var paymentRequest = new
        {
            merchant_id = "cdb9caf7-008b-439c-b6e0-3f4bd44b2476",
            amount = amountToRial,
            callback_url = $"https://localhost:7250/payment/verify?OrderId={OrderId}",
            description = "Payment for order #1234",
            metadata = new
            {
                email = "test@example.com",
                mobile = "09121111111"
            }
        };

        var json = JsonConvert.SerializeObject(paymentRequest);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        using (var client = new HttpClient())
        {
            var response = await client.PostAsync("https://payment.zarinpal.com/pg/v4/payment/request.json", data);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = JsonConvert.DeserializeObject<dynamic>(result);
                string authority = jsonResult.data.authority;
                string paymentUrl = "https://www.zarinpal.com/pg/StartPay/" + authority;
                return Redirect(paymentUrl);
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<dynamic>(result);
                ViewBag.Error = errorResponse.errors.message;
                return View("Error");
            }
        }
    }




    [HttpGet]
    public async Task<IActionResult> Verify(string authority, int orderId)
    {

        string verifyUrl = "https://api.zarinpal.com/pg/v4/payment/verify.json";

        var verifyRequest = new
        {
            merchant_id = MerchantId,
            authority = authority,
            amount = 12000
        };

        var json = JsonConvert.SerializeObject(verifyRequest);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.PostAsync(verifyUrl, httpContent);
            var responseString = await response.Content.ReadAsStringAsync();

            dynamic responseObject = JsonConvert.DeserializeObject(responseString);

            if (responseObject.data.code == 100)
            {
                await _orderService.UpdatePaymentStatusAsync(orderId);
                ViewBag.Message = "پرداخت با موفقیت انجام شد.";
            }
            else if (responseObject.data.code == 101)
            {
                ViewBag.Message = "این تراکنش قبلاً تأیید شده است.";
            }
            else
            {
                ViewBag.Message = "خطا در تأیید تراکنش: " + responseObject.message;
            }
        }

        return View("Index");
    }

}
