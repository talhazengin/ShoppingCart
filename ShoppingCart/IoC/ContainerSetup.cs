using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using ShoppingCart.Data.Access.DAL;
using ShoppingCart.Filters;
using ShoppingCart.Helpers;
using ShoppingCart.Maps;
using ShoppingCart.Queries;

namespace ShoppingCart.IoC
{
    public static class ContainerSetup
    {
        public static void Setup(IServiceCollection services)
        {
            AddQueryProcessors(services);
            AddUow(services);
            ConfigureAutoMapper(services);
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            MapperConfiguration mapperConfig = AutoMapperConfigurator.Configure();
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(x => mapper);
            services.AddTransient<IAutoMapper, AutoMapperAdapter>();
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

            // Use in memory database for this demo project.
            services.AddDbContext<ShoppingCartDbContext>(builder => builder.UseInMemoryDatabase("InMemoryTestDb"));

            services.AddScoped<IUnitOfWork>(ctx => new EFUnitOfWork(ctx.GetRequiredService<ShoppingCartDbContext>()));

            services.AddScoped<IActionTransactionHelper, ActionTransactionHelper>();

            services.AddScoped<UnitOfWorkFilterAttribute>();
        }
    }
}
