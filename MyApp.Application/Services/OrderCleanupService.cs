using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyApp.Domain.Interfaces;

// This class is designed to clean up old orders and inherits from BackgroundService
public class OrderCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider; // Service provider to access other services
    private readonly ILogger<OrderCleanupService> _logger; // Logger to log information

    // Constructor that injects IServiceProvider and ILogger into the class
    public OrderCleanupService(IServiceProvider serviceProvider, ILogger<OrderCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    // The ExecuteAsync method where the core logic of the service is implemented
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) // While cancellation has not been requested
        {
            using (var scope = _serviceProvider.CreateScope()) // Create a scope to use Scoped Services
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>(); // Retrieve the order repository
                await CleanupOrdersAsync(orderRepository); // Call the method to clean up orders
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Wait for one hour before executing again
        }
    }

    // CleanupOrdersAsync method to check and delete old orders
    private async Task CleanupOrdersAsync(IOrderRepository orderRepository)
    {
        // Retrieve all orders from the repository
        var ordersToDelete = await orderRepository.GetAllAsync();

        // Check each order
        foreach (var order in ordersToDelete)
        {
            // If the order is not finalized and more than 48 hours have passed since its creation
            if (!order.IsFinaly && (DateTime.Now - order.CreateDate).TotalMinutes > 60) // Adjust for testing
            {
                // Log the deletion of the order
                _logger.LogInformation($"Deleting order with ID: {order.Id} - Status: {order.IsFinaly}");

                // Delete the order from the database using the Order object
                await orderRepository.DeleteAsync(order); // Changed to delete by object instead of ID
            }
        }
    }
}
