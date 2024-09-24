using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels;
using MyApp.Domain.ViewModels.AssignDiscount;
using MyApp.Domain.ViewModels.Discounts;
using MyApp.Domain.ViewModels.Products;

namespace MyApp.Mvc.Areas.Admin.Controllers
{
    public class DiscountController : AdminBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IDiscountAssignmentToProductService _discountAssignmentToProductService;
        private readonly IDiscountAssignmentToUserService _discountAssignmentToUserService;

        public DiscountController(IDiscountService discountService, IProductService productService, IUserService userService,
            IDiscountAssignmentToProductService discountAssignmentToProductService, IDiscountAssignmentToUserService discountAssignmentToUserService
            )
        {
            _discountService = discountService;
            _productService = productService;
            _userService = userService;
            _discountAssignmentToProductService = discountAssignmentToProductService;
            _discountAssignmentToUserService = discountAssignmentToUserService;
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
                IsActive = discount.IsActive,
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
            if (!ModelState.IsValid)
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

        // GET: DiscountController/AssignToProduct/5
        public async Task<ActionResult> AssignToProduct(int id)
        {
            var products = await _productService.GetAllAsync();
            var viewModel = new AssignDiscountToProductViewModel
            {
                DiscountId = id,
                Products = products.Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    Title = product.Title
                }).ToList()
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AssignToProduct(AssignDiscountToProductViewModel model)
        {
            bool hasDuplicateDiscounts = false;

            if (ModelState.IsValid)
            {
                foreach (var productId in model.ProductIds)
                {
                    var existingDiscount = await _discountAssignmentToProductService.GetProductDiscountAsync(productId, model.DiscountId);
                    if (existingDiscount == null)
                    {
                        await _discountAssignmentToProductService.AddAsync(new ProductDiscount
                        {
                            ProductId = productId,
                            DiscountId = model.DiscountId
                        });
                    }
                    else
                    {
                        hasDuplicateDiscounts = true;
                    }
                }

                if (!hasDuplicateDiscounts)
                {
                    TempData["AlertMessage"] = "تخفیف با موفقیت به محصولات اختصاص یافت";
                    return RedirectToAction("Index", "Discount");
                }
                else
                {
                    TempData["AlertMessage"] = $"تخفیف تکراری است";
                    return RedirectToAction("Index", "Discount");
                }

            }

            var products = await _productService.GetAllAsync();
            model.Products = products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title
            }).ToList();

            return View(model);
        }

        // GET: DiscountController/AssignToUser/5
        public async Task<ActionResult> AssignToUser(int id)
        {
            var users = await _userService.GetAllAsync();
            var viewModel = new AssignDiscountToUserViewModel
            {
                DiscountId = id,
                Users = users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssignToUser(AssignDiscountToUserViewModel model)
        {
            bool hasDuplicateDiscounts = false;

            if (ModelState.IsValid)
            {
                foreach (var userId in model.UserIds)
                {
                    var existingDiscount = await _discountAssignmentToUserService.GetUserDiscountAsync(userId, model.DiscountId);
                    if (existingDiscount == null)
                    {
                        await _discountAssignmentToUserService.AddAsync(new UserDiscount
                        {
                            UserId = userId,
                            DiscountId = model.DiscountId
                        });
                    }
                    else
                    {
                        hasDuplicateDiscounts = true;
                    }
                }

                if (!hasDuplicateDiscounts)
                {
                    TempData["AlertMessage"] = "تخفیف با موفقیت به کاربران اختصاص یافت";
                    return RedirectToAction("Index", "Discount");
                }
                else
                {
                    TempData["AlertMessage"] = $"تخفیف تکراری است";
                    return RedirectToAction("Index", "Discount");
                }
            }

            var users = await _userService.GetAllAsync();
            model.Users = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            }).ToList();

            return View(model);
        }


    }
}
