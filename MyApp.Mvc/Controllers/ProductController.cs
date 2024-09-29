using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;

namespace MyApp.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {

            // بررسی وضعیت لاگین کاربر
            int? userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = int.Parse(User.Identity.Name); // در صورتی که UserId در Name ذخیره شده باشد
            }

            // گرفتن محصولات تخفیف‌دار متناسب با وضعیت کاربر (لاگین کرده یا نکرده)
            var products = await _productService.GetDiscountedProductsByUserStatusAsync(userId);

            // ارسال لیست محصولات به ویو
            return View(products);
        }

          

    }
}
