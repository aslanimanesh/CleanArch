using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using MyApp.Domain.ViewModels.Products;
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

        #region Actions

        #region List
        public async Task<IActionResult> Index()
        {
            int? userId = null;

            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out var id))
            {
                userId = id; // ??? ????? ???? ???? userId ?? ????? ???????
            }

            var products = await _productService.GetDiscountedProductsByUserStatusAsync(userId);

            var productViewModels = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                ImageName = p.ImageName,
                DiscountPercentage = p.DiscountPercentage,
                DiscountedPrice = p.DiscountedPrice,
            }).ToList();

            return View(productViewModels);
        }
        #endregion

        #endregion
    }
}
