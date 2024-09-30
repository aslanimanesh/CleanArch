using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Interfaces;
using MyApp.Domain.Models;
using MyApp.Domain.ViewModels.Orders;
using System.Security.Claims;

namespace MyApp.Mvc.Controllers
{
    public class OrderController : Controller
    {

        #region Fields
        private readonly IOrderService _orderService;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly IProductService _productService;
        
        #endregion

        #region Constructor
        public OrderController(IOrderService orderService, IOrderDetailsService orderDetailsService,
          IProductService productService)
        {
            _orderService = orderService;
            _orderDetailsService = orderDetailsService;
            _productService = productService;
        }
        #endregion

        #region Public Methods

        #region AddToCart

        [Authorize]
        public async Task<IActionResult> AddToCart(int id)
        {
            // Get User Id From Current User
            int CurrentUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Check for product discount
            var productViewModel = await _productService.GetProductForShowInBasket(CurrentUserID, id);

            // If the product is not found, return NotFound
            if (productViewModel == null)
            {
                return NotFound();
            }

            // Check if the user already has a pending order
            Order order = await _orderService.HasPendingOrder(CurrentUserID);
            if (order == null)
            {
                // If no pending order exists, create a new order
                order = new Order()
                {
                    UserId = CurrentUserID,
                    CreateDate = DateTime.Now,
                    IsFinaly = false,
                    Sum = 0,
                };

                // Add the new order to the database
                await _orderService.AddAsync(order);
            }

            // Check if the product already exists in the order details
            var details = await _orderDetailsService.ExistProductInOrderDetail(order.Id, id);
            if (details == null)
            {
                // If the product does not exist, add it to the order details
                await _orderDetailsService.AddAsync(new OrderDetail()
                {
                    OrderId = order.Id,
                    Quantity = 1,
                    OriginalPrice = productViewModel.Price,
                    ProductId = productViewModel.Id,
                    FinalPrice = productViewModel.DiscountedPrice, // Adding the discounted price
                    DiscountPercentage = productViewModel.DiscountPercentage // Adding the discount percentage
                });
            }
            else
            {
                // If the product exists, increment the quantity
                details.Quantity += 1;
                await _orderDetailsService.UpdateAsync(details);
            }

            // Update the total sum of the order
            await UpdateSumOrder(order.Id);

            return RedirectToAction("ShowOrder", "Order"); // Redirect to show the order
        }


        #endregion

        #region Show Basket Buy

        [Authorize] 
        public async Task<IActionResult> ShowOrder()
        {
            // Retrieve the current user Id
            int CurrentUserID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Check if the user has a pending order
            Order order = await _orderService.HasPendingOrder(CurrentUserID);

            // List to hold order details to display
            List<ShowOrderViewModel> _list = new List<ShowOrderViewModel>();
            if (order != null)
            {
                // Retrieve all details for the order
                var details = await _orderDetailsService.GetAllOrderDetailByOrderId(order.Id);
                foreach (var item in details)
                {
                    // Retrieve the product for each order detail
                    var product = await _productService.GetByIdAsync(item.ProductId);

                    // Add the order detail information to the view model list
                    _list.Add(new ShowOrderViewModel()
                    {
                        Count = item.Quantity,
                        ImageName = product.ImageName,
                        OrderDetailId = item.Id,
                        Title = product.Title,
                        OriginalPrice = item.OriginalPrice,
                        Price = item.OriginalPrice,
                        DiscountPercentage = Convert.ToInt32(item.DiscountPercentage),
                        DiscountedPrice = item.FinalPrice,

                    });
                }
                // Pass additional order information to the view
                ViewBag.OrderId = order.Id;
                ViewBag.Sum = order.Sum;
                ViewBag.UserId = order.UserId;
            }

            return View(_list); // Return the view with order details
        }

        #endregion

        #region DeleteProductFromBasket

        public async Task<IActionResult> Delete(int id)
        {
            // Retrieve the order detail to be deleted
            var orderDetail = await _orderDetailsService.GetByIdAsync(id);
            await _orderDetailsService.DeleteAsync(id); // Delete the order detail
            await UpdateSumOrder(orderDetail.OrderId); // Update the order sum

            return RedirectToAction("ShowOrder", "Order"); // Redirect to show the order
        }

        #endregion

        #region Command Increase And Decrease Quantity 

        public async Task<IActionResult> Command(int id, string command)
        {
            // Retrieve the order detail
            var orderDetail = await _orderDetailsService.GetByIdAsync(id);

            // Process the command for updating the order detail
            switch (command)
            {
                case "up": // Increment the count
                    {
                        orderDetail.Quantity += 1;
                        await _orderDetailsService.UpdateAsync(orderDetail);
                        break;
                    }
                case "down": // Decrement the count
                    {
                        orderDetail.Quantity -= 1;
                        if (orderDetail.Quantity == 0)
                        {
                            // If count reaches zero, delete the order detail
                            await _orderDetailsService.DeleteAsync(orderDetail.Id);
                        }
                        else
                        {
                            await _orderDetailsService.UpdateAsync(orderDetail);
                        }
                        break;
                    }
            }

            // Update the total sum of the order
            await UpdateSumOrder(orderDetail.OrderId);

            return RedirectToAction("ShowOrder", "Order"); // Redirect to show the order
        }
        #endregion

        #region UpdateSumOrder

        public async Task UpdateSumOrder(int orderId)
        {
            // Retrieve the order
            var order = await _orderService.GetByIdAsync(orderId);
            // Update the order sum with the total of all order details
            order.Sum = await _orderService.GetOrderTotalAsync(orderId);
            await _orderService.UpdateAsync(order); // Save changes to the order
        }

        #endregion

        #endregion

    }
}
