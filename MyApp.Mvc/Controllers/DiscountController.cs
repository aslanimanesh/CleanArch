using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using System.Security.Claims;

namespace MyApp.Mvc.Controllers
{
    public class DiscountController : Controller
    {
        #region Fields
        private readonly IDiscountService _discountService;
        #endregion

        #region Constructor
        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }
        #endregion

        #region UseDiscount
        [Authorize] // Requires user to be authenticated
        [HttpPost]
        public async Task<IActionResult> UseDiscount(string discountCode, int orderId)
        {
            // Retrieve the current user's ID
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Apply the discount to the order
            var resultMessage = await _discountService.ApplyDiscountToOrderAsync(discountCode, orderId, userId);

            // Show the result message to the user
            TempData["DiscountResult"] = resultMessage;
            return RedirectToAction("ShowOrder", "Order"); // Redirect to show the order
        }
        #endregion
    }
}
