using System.Linq;
using System.Threading.Tasks;

using ShoppingCart.Data.Model;
using ShoppingCart.Model.Product;

namespace ShoppingCart.Queries
{
    /// <summary>
    /// Processes queries on products.
    /// </summary>
    public interface IProductStockQueryProcessor
    {
        IQueryable<Product> GetAllProducts();

        Product GetProduct(int id);

        Task CreateProduct(CreateProductModel model);

        Task UpdateProduct(int id, UpdateProductModel model);

        Task DeleteProduct(int id);
    }
}