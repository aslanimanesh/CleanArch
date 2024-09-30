using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using System.Security.Claims;

namespace MyApp.Mvc.Controllers
{
    public class HomeController : Controller
    {

        #region Fields

        private readonly IProductService _productService;

        #endregion

        #region Constructor

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region Public Methods

        #region List

        public async Task<IActionResult> Index()
        {
            int? userId = null;

            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out var id))
            {
                userId = id;
            }

            var products = await _productService.GetDiscountedProductsByUserStatusAsync(userId);

            return View(products);
        }

        #endregion

        #endregion

    }
}
