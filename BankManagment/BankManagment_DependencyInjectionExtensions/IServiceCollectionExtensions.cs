using BankManagment_Infrastructure.Repository;
using BankManagment_Infrastructure.UnitOfWork;
using BankManagment_Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BankManagment_DependencyInjectionExtensions
{

    public static class IServiceCollectionExtensions
    {
        //public static void RegisterServicesAndRepositories(this IServiceCollection services, Assembly assembly)
        //{
        //    services.Scan(selector => selector
        //        .FromAssemblies(assembly)
        //        .AddClasses(classes => classes
        //            .InNamespaces("BankManagment_Services", "BankManagment_Infrastructure"))
        //        .AsImplementedInterfaces()
        //        .WithScopedLifetime());
        //}
        public static void RegisterServicesAndRepositories(this IServiceCollection services)
        {
            services.Scan(selector => selector
                .FromAssemblies(
                  typeof(IAccountTypeService).Assembly,
                  typeof(IUnitOfWork).Assembly
                ).AddClasses()
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }
    }

}
