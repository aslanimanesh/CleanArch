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


            //Infra Data Layer

            services.AddScoped<IUserRepository, UserRepository>();

        }
    }
}
