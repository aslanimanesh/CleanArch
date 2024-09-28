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

            // Check if the user already has a pending order
            Order order = await _orderService.HasPendingOrder(CurrentUserID);
            if (order == null)
            {
                // If no pending order, create a new order
                order = new Order()
                {
                    UserId = CurrentUserID,
                    CreateDate = DateTime.Now,
                    IsFinaly = false,
                    Sum = 0,
                };

                // Retrieve the product to be added to the order
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                // Add the new order to the database
                await _orderService.AddAsync(order);
                // Add the product as an order detail
                await _orderDetailsService.AddAsync(new OrderDetail()
                {
                    OrderId = order.Id,
                    Count = 1,
                    Price = product.Price,
                    ProductId = id
                });

                // Update the total sum of the order
                await UpdateSumOrder(order.Id);
            }
            else
            {
                // If there is a pending order, check if the product already exists in the order
                var details = await _orderDetailsService.ExistProductInOrderDetail(order.Id, id);
                if (details == null)
                {
                    // If product doesn't exist, add it to the order details
                    var product = await _productService.GetByIdAsync(id);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    await _orderDetailsService.AddAsync(new OrderDetail()
                    {
                        OrderId = order.Id,
                        Count = 1,
                        Price = product.Price,
                        ProductId = id
                    });
                }
                else
                {
                    // If product exists, increment the count
                    details.Count += 1;
                    await _orderDetailsService.UpdateAsync(details);
                }
            }

            // Update the total sum of the order
            await UpdateSumOrder(order.Id);

            return RedirectToAction("ShowOrder", "Order"); // Redirect to the order display
        }
        #endregion

        #region ShowOrder
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
                        Count = item.Count,
                        ImageName = product.ImageName,
                        OrderDetailId = item.Id,
                        Price = item.Price,
                        Sum = item.Count * item.Price, // Calculate total for this item
                        Title = product.Title
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

        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            // Retrieve the order detail to be deleted
            var orderDetail = await _orderDetailsService.GetByIdAsync(id);
            await _orderDetailsService.DeleteAsync(id); // Delete the order detail
            await UpdateSumOrder(orderDetail.OrderId); // Update the order sum

            return RedirectToAction("ShowOrder", "Order"); // Redirect to show the order
        }
        #endregion

        #region Command
        public async Task<IActionResult> Command(int id, string command)
        {
            // Retrieve the order detail
            var orderDetail = await _orderDetailsService.GetByIdAsync(id);

            // Process the command for updating the order detail
            switch (command)
            {
                case "up": // Increment the count
                    {
                        orderDetail.Count += 1;
                        await _orderDetailsService.UpdateAsync(orderDetail);
                        break;
                    }
                case "down": // Decrement the count
                    {
                        orderDetail.Count -= 1;
                        if (orderDetail.Count == 0)
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
