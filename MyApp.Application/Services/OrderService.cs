﻿using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interfaces;
using MyApp.Domain.Interfaces;
using MyApp.Domain.Models;

namespace MyApp.Application.Services
{
    public class OrderService :GenericService<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository) : base(orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<decimal> GetOrderTotalAsync(int orderId)
        {
            return await _orderRepository.GetOrderTotalPriceAsync(orderId);
        }

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            return await _orderRepository.GetOrderWithDetailsAsync(orderId);
        }

        public async Task<Order> HasPendingOrder(int userId)
        {
            return await _orderRepository.HasPendingOrder(userId);
        }

       
    }
}
