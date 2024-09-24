using Microsoft.Extensions.DependencyInjection;
using MyApp.Application.Interfaces;
using MyApp.Application.Services;
using MyApp.Domain.Interfaces;
using MyApp.Infa.Data.Repositories;

namespace MyApp.Infra.Ioc
{
    public static class DependencyContainer
    {
        public static void RegisterService(IServiceCollection services)
        {
            //Application Layer

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDiscountService, DiscoutService>();
            services.AddScoped<IDiscountAssignmentToProductService, DiscountAssignmentToProductService>();
            services.AddScoped<IDiscountAssignmentToUserService, DiscountAssignmentToUserService>();



            //Infra Data Layer

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<IProductDiscountRepository, ProductDiscountRepository>();
            services.AddScoped<IProductDiscountRepository, ProductDiscountRepository>();
            services.AddScoped<IUserDiscountRepository, UserDiscountRepository>();

        }
    }
}
