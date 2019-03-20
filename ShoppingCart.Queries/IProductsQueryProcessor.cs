using System.Linq;
using System.Threading.Tasks;

using ShoppingCart.Model;
using ShoppingCart.Data.Model;

namespace ShoppingCart.Queries
{
    public interface IProductsQueryProcessor
    {
        IQueryable<Product> Get();

        Product Get(int id);

        Task<Product> Create(CreateProductModel model);

        Task<Product> Update(int id, UpdateProductModel model);

        Task Delete(int id);
    }
}