using Microsoft.EntityFrameworkCore;

using ShoppingCart.Data.Model;

namespace ShoppingCart.Data.Access.Maps
{
    public class CartProductMap : IMap
    {
        public void Visit(ModelBuilder builder)
        {
            builder.Entity<CartProduct>()
                .ToTable("CartProduct")
                .HasKey(x => x.Id);
        }
    }
}
