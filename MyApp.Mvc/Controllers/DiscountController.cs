using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Discounts;

namespace MyApp.Mvc.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        // GET: DiscountController
        public async Task<ActionResult> Index()
        {
            var discounts = await _discountService.GetAllAsync();
            var discountViewModels = discounts.Select(discount => new DiscountViewModel
            {
                Id = discount.Id,
                DiscountPercentage = discount.DiscountPercentage,
                DiscountCode = discount.DiscountCode,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive = discount.IsActive,

            }).ToList();

            return View(discountViewModels);
        }

        // GET: DiscountController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var discount = await _discountService.GetByIdAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            var viewModel = new DiscountViewModel
            {
                Id = discount.Id,
                DiscountPercentage = discount.DiscountPercentage,
                DiscountCode = discount.DiscountCode,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive= discount.IsActive,
            };

            return View(viewModel);
        }

        // GET: DiscountController/Create
        public IActionResult Create()
        {
            var viewModel = new CreateDiscountViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDiscountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var discount = new Discount
                {
                    DiscountPercentage = viewModel.DiscountPercentage,
                    DiscountCode = viewModel.DiscountCode,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    IsActive = viewModel.IsActive,

                };

                await _discountService.AddAsync(discount);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: DiscountController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var discount = await _discountService.GetByIdAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            var viewModel = new EditDiscountViewModel
            {
                Id = discount.Id,
                DiscountPercentage = discount.DiscountPercentage,
                DiscountCode = discount.DiscountCode,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive = discount.IsActive,
                
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditDiscountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var discount = new Discount
                {
                    Id = viewModel.Id,
                    DiscountPercentage = viewModel.DiscountPercentage,
                    DiscountCode = viewModel.DiscountCode,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    IsActive = viewModel.IsActive,
                };

                await _discountService.UpdateAsync(discount);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: DiscountController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var discount = await _discountService.GetByIdAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }

        // POST: DiscountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _discountService.DeleteAsync(id); 

            return RedirectToAction(nameof(Index));
        }
    }
}
