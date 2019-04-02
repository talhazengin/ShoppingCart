using Microsoft.EntityFrameworkCore;

using ShoppingCart.Data.Model;

namespace ShoppingCart.Data.Access.Maps
{
    public class ProductMap : IMap
    {
        public void Visit(ModelBuilder builder)
        {
            builder.Entity<Product>()
                .ToTable("Product")
                .HasKey(x => x.Id);
        }
    }
}
