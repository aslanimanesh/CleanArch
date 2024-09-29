using Microsoft.AspNet.SignalR.Client.Http;
using MyApp.Application.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace MyApp.Application.Services
{
    public class PaymentService : IPaymentService
    {
        #region Fields

        private const string MerchantId = "cdb9caf7-008b-439c-b6e0-3f4bd44b2476";
        private const string PaymentUrl = "https://payment.zarinpal.com/pg/v4/payment/request.json";
        private const string VerifyUrl = "https://api.zarinpal.com/pg/v4/payment/verify.json";

        #endregion

        #region Public CreatePaymentAsync

        public async Task<(bool success, string redirectUrl, string errorMessage)> CreatePaymentAsync(int orderId, decimal amount, string callbackUrl)
        {
            int amountToRial = (int)(amount * 10);
            var paymentRequest = new
            {
                merchant_id = MerchantId,
                amount = amountToRial,
                callback_url = $"{callbackUrl}&amount={amount}",
                description = $"پرداخت برای سفارش شماره {orderId}",
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
                try
                {
                    var response = await client.PostAsync(PaymentUrl, data);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResult = JsonConvert.DeserializeObject<dynamic>(result);
                        string authority = jsonResult.data.authority;
                        string paymentUrl = "https://www.zarinpal.com/pg/StartPay/" + authority;
                        return (true, paymentUrl, null);
                    }
                    else
                    {
                        var errorResponse = JsonConvert.DeserializeObject<dynamic>(result);
                        return (false, null, errorResponse.errors.message);
                    }
                }
                catch (HttpRequestException ex)
                {
                    return (false, null, $"خطا در اتصال به سرویس پرداخت: {ex.Message}");
                }
            }
        }

        #region MyRegion

        #endregion
        #endregion

        #region VerifyPaymentAsync
        public async Task<(bool success, string message)> VerifyPaymentAsync(string authority, decimal amount)
        {
            var verifyRequest = new
            {
                merchant_id = MerchantId,
                authority = authority,
                amount = (int)(amount * 10)
            };

            var json = JsonConvert.SerializeObject(verifyRequest);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(VerifyUrl, httpContent);
                    var responseString = await response.Content.ReadAsStringAsync();

                    var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

                    if (responseObject.data.code == 100)
                    {
                        return (true, "پرداخت با موفقیت انجام شد.");
                    }
                    else if (responseObject.data.code == 101)
                    {
                        return (true, "این تراکنش قبلاً تأیید شده است.");
                    }
                    else
                    {
                        return (false, $"خطا در تأیید تراکنش: {responseObject.message}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    return (false, $"خطا در اتصال به سرویس تأیید: {ex.Message}");
                }
            }
        }

        #endregion

    }
}
