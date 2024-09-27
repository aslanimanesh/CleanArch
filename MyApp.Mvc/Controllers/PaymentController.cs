using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyApp.Domain.Models;
using Newtonsoft.Json;

public class PaymentController : Controller
{
    private const string MerchantId = "cdb9caf7-008b-439c-b6e0-3f4bd44b2476"; // کد مرچنت
    private const string PaymentUrl = "https://payment.zarinpal.com/pg/v4/payment/request.json";

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Pay()
    {
        var paymentRequest = new
        {
            merchant_id = "cdb9caf7-008b-439c-b6e0-3f4bd44b2476", 
            amount = 20000, 
            callback_url = "https://localhost:7250/payment/verify", 
            description = "Payment for order #1234",  
            metadata = new
            {
                email = "test@example.com",
                mobile = "09123456789"
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
                // در صورت عدم موفقیت، پیام خطا را نمایش دهید
                var errorResponse = JsonConvert.DeserializeObject<dynamic>(result);
                ViewBag.Error = errorResponse.errors.message;
                return View("Error");
            }
        }
    }


  

[HttpGet]
public async Task<IActionResult> Verify(string Authority, int amount)
{
    string verifyUrl = "https://api.zarinpal.com/pg/v4/payment/verify.json";

    var verifyRequest = new
    {
        merchant_id = MerchantId,
        authority = Authority,
        amount = 20000
    };

    var json = JsonConvert.SerializeObject(verifyRequest);
    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

    using (var httpClient = new HttpClient())
    {
        var response = await httpClient.PostAsync(verifyUrl, httpContent);
        var responseString = await response.Content.ReadAsStringAsync();

        // تجزیه محتوای JSON دریافتی
        dynamic responseObject = JsonConvert.DeserializeObject(responseString);

        // چاپ پاسخ برای دیباگینگ
        Console.WriteLine(responseString);

        // بررسی کد موفقیت
        if (responseObject.data.code == 100) // اگر کد 100 در سطح ریشه باشد
        {
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
