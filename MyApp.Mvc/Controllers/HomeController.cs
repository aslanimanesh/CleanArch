using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using MyApp.Domain.ViewModels.Products;

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
            var products = await _productService.GetAllAsync();

            var productViewModels = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                ImageName = p.ImageName,
            }).ToList();

            return View(productViewModels);
        }
        #endregion

        #endregion
    }
}
