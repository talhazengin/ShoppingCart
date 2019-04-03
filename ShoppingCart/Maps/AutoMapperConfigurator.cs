using System;
using System.Linq;
using System.Reflection;

using AutoMapper;

namespace ShoppingCart.Maps
{
    public static class AutoMapperConfigurator
    {
        private static readonly object Locker = new object();

        private static MapperConfiguration _configuration;

        public static MapperConfiguration Configure()
        {
            lock (Locker)
            {
                if (_configuration != null)
                {
                    return _configuration;
                }

                Type thisType = typeof(AutoMapperConfigurator);

                Type configInterfaceType = typeof(IAutoMapperTypeConfigurator);

                IAutoMapperTypeConfigurator[] configurators = thisType.GetTypeInfo().Assembly.GetTypes()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Namespace))
                    .Where(x => x.Namespace.Contains(thisType.Namespace))
                    .Where(x => x.GetTypeInfo().GetInterface(configInterfaceType.Name) != null)
                    .Select(x => (IAutoMapperTypeConfigurator)Activator.CreateInstance(x))
                    .ToArray();

                void AggregatedConfigurator(IMapperConfigurationExpression config)
                {
                    foreach (IAutoMapperTypeConfigurator configurator in configurators)
                    {
                        configurator.Configure(config);
                    }
                }

                _configuration = new MapperConfiguration(AggregatedConfigurator);

                return _configuration;
            }
        }
    }
}