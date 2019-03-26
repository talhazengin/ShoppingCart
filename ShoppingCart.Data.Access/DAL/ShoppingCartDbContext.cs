using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using ShoppingCart.Data.Access.Maps;

namespace ShoppingCart.Data.Access.DAL
{
    public class ShoppingCartDbContext : DbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IEnumerable<IMap> mappings = MappingsHelper.GetMappings();

            foreach (IMap mapping in mappings)
            {
                mapping.Visit(modelBuilder);
            }
        }
    }
}
