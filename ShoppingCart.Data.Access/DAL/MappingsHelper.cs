using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ShoppingCart.Data.Access.Maps;

namespace ShoppingCart.Data.Access.DAL
{
    public static class MappingsHelper
    {
        public static IEnumerable<IMap> GetMappings()
        {
            IEnumerable<TypeInfo> assemblyTypes = typeof(ProductMap).GetTypeInfo().Assembly.DefinedTypes;

            IEnumerable<TypeInfo> mappings = assemblyTypes
                .Where(t => t.Namespace != null && t.Namespace.Contains(typeof(ProductMap).Namespace))
                .Where(t => typeof(IMap).GetTypeInfo().IsAssignableFrom(t));

            mappings = mappings.Where(x => !x.IsAbstract);
            return mappings.Select(m => (IMap)Activator.CreateInstance(m.AsType())).ToArray();
        }
    }
}