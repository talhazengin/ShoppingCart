using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Data.Access.Maps
{
    public interface IMap
    {
        void Visit(ModelBuilder builder);
    }
}