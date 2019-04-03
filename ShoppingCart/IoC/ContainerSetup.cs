using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ShoppingCart.Data.Access.DAL;
using ShoppingCart.Filters;
using ShoppingCart.Helpers;
using ShoppingCart.Queries;

namespace ShoppingCart.IoC
{
    public static class ContainerSetup
    {
        public static void Setup(IServiceCollection services, IConfiguration configuration)
        {
            AddQueryProcessors(services);
            AddUow(services);
        }

        private static void AddQueryProcessors(IServiceCollection services)
        {
            Type exampleProcessorType = typeof(ProductsQueryProcessor);

            Type[] types = (from t in exampleProcessorType.GetTypeInfo().Assembly.GetTypes()
                         where t.Namespace == exampleProcessorType.Namespace
                               && t.GetTypeInfo().IsClass
                               && t.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null
                         select t).ToArray();

            foreach (Type type in types)
            {
                Type interfaceQ = type.GetTypeInfo().GetInterfaces().First();
                services.AddScoped(interfaceQ, type);
            }
        }

        private static void AddUow(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer();

            // Using in memory database for testing purpose.
            services.AddDbContext<ShoppingCartDbContext>(builder => builder.UseInMemoryDatabase("InMemoryTestDb"));

            services.AddScoped<IUnitOfWork>(ctx => new EFUnitOfWork(ctx.GetRequiredService<ShoppingCartDbContext>()));

            services.AddScoped<IActionTransactionHelper, ActionTransactionHelper>();

            services.AddScoped<UnitOfWorkFilterAttribute>();
        }
    }
}
