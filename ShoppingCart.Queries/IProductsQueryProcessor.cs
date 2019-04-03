using System.Linq;
using System.Threading.Tasks;

using ShoppingCart.Data.Model;
using ShoppingCart.Model;

namespace ShoppingCart.Queries
{
    public interface IProductsQueryProcessor
    {
        IQueryable<Product> GetAll();

        Product Get(int id);

        Task<Product> Create(CreateProductModel model);

        Task<Product> Update(int id, UpdateProductModel model);

        Task Delete(int id);
    }
}