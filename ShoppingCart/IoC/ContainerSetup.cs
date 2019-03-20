using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ShoppingCart.Queries;

namespace ShoppingCart.IoC
{
    public static class ContainerSetup
    {
        public static void Setup(IServiceCollection services, IConfigurationRoot configuration)
        {
            AddUow(services, configuration);
            AddQueryProcessors(services);
            ConfigureAutoMapper(services);
            ConfigureAuth(services);
        }

        private static void ConfigureAuth(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddScoped<ITokenBuilder, TokenBuilder>();

            //services.AddScoped<ISecurityContext, SecurityContext>();
        }

        private static void AddQueryProcessors(IServiceCollection services)
        {
            var exampleProcessorType = typeof(ProductsQueryProcessor);

            var types = (from t in exampleProcessorType.GetTypeInfo().Assembly.GetTypes()
                         where t.Namespace == exampleProcessorType.Namespace
                               && t.GetTypeInfo().IsClass
                               && t.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null
                         select t).ToArray();

            foreach (var type in types)
            {
                var interfaceQ = type.GetTypeInfo().GetInterfaces().First();
                services.AddScoped(interfaceQ, type);
            }
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            //var mapperConfig = AutoMapperConfigurator.Configure();
            //var mapper = mapperConfig.CreateMapper();
            //services.AddSingleton(x => mapper);
            //services.AddTransient<IAutoMapper, AutoMapperAdapter>();
        }

        private static void AddUow(IServiceCollection services, IConfigurationRoot configuration)
        {
            var connectionString = configuration["Data:main"];

            services.AddEntityFrameworkSqlServer();

            // services.AddDbContext<MainDbContext>(options =>
            //     options.UseSqlServer(connectionString));

            // services.AddScoped<IUnitOfWork>(ctx => new EFUnitOfWork(ctx.GetRequiredService<MainDbContext>()));

            // services.AddScoped<IActionTransactionHelper, ActionTransactionHelper>();

            // services.AddScoped<UnitOfWorkFilterAttribute>();
        }
    }
}
