using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyApp.Application.Interfaces;
using MyApp.Domain.ViewModels.AssignDiscount;

namespace MyApp.Mvc.Controllers
{
    public class DiscountAssignmentController : Controller
    {
        private readonly IDiscountAssignmentService _discountAssignmentService;
        private readonly IProductService _productService;
        private readonly IDiscountService _discountService;

        public DiscountAssignmentController(IDiscountAssignmentService discountAssignmentService,IProductService productService,
            IDiscountService discountService)
        {
            _discountAssignmentService = discountAssignmentService;
            _productService = productService;
            _discountService = discountService;
        }

        // GET: DiscountAssignment/Create
        public async Task<IActionResult> Create()
        {
            var discounts = await _discountService.GetAllAsync(); 
            ViewBag.Discounts = discounts.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.DiscountCode 
            }).ToList();

      
            var products = await _productService.GetAllAsync();
            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            });

            var viewModel = new AssignDiscountViewModel();
            return View(viewModel);
           
            
        }

        // POST: DiscountAssignment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssignDiscountViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _discountAssignmentService.AssignDiscountToProductsAsync(model);
                return RedirectToAction("Index", "Product");
            }

            return View(model);
        }
    }
}

