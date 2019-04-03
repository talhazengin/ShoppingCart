using System.Linq;
using System.Threading.Tasks;

using ShoppingCart.Core;
using ShoppingCart.Data.Access.DAL;
using ShoppingCart.Data.Model;
using ShoppingCart.Model;

namespace ShoppingCart.Queries
{
    /// <summary>
    /// Processes queries on products.
    /// </summary>
    public class ProductsQueryProcessor : IProductsQueryProcessor
    {
        private readonly IUnitOfWork _uow;

        public ProductsQueryProcessor(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IQueryable<Product> Get()
        {
            IQueryable<Product> query = GetAllProductsQuery();
            return query;
        }

        public Product Get(int id)
        {
            Product user = GetAllProductsQuery().FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Product is not found");
            }

            return user;
        }

        public async Task<Product> Create(CreateProductModel model)
        {
            var item = new Product
            {
                Name = model.Name,
                Description = model.Description
            };

            _uow.Add(item);

            await _uow.CommitAsync();

            return item;
        }

        public async Task<Product> Update(int id, UpdateProductModel model)
        {
            Product product = GetAllProductsQuery().FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Product is not found");
            }

            product.Name = model.Name;
            product.Description = model.Description;

            await _uow.CommitAsync();

            return product;
        }

        public async Task Delete(int id)
        {
            Product user = GetAllProductsQuery().FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new NotFoundException("Product is not found");
            }

            if (user.IsDeleted)
            {
                return;
            }

            user.IsDeleted = true;

            await _uow.CommitAsync();
        }

        private IQueryable<Product> GetAllProductsQuery()
        {
            IQueryable<Product> query = _uow.Query<Product>()
                .Where(x => !x.IsDeleted);

            return query;
        }
    }
}
