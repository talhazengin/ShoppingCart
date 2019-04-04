using System.Linq;
using System.Threading.Tasks;

using ShoppingCart.Core.Exceptions;
using ShoppingCart.Data.Access.DAL;
using ShoppingCart.Data.Model;
using ShoppingCart.Model.Product;

namespace ShoppingCart.Queries
{
    public class ProductStockQueryProcessor : IProductStockQueryProcessor
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductStockQueryProcessor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Product> GetAllProducts()
        {
            IQueryable<Product> query = _unitOfWork.Query<Product>()
                .Where(x => !x.IsDeleted);

            return query;
        }

        public Product GetProduct(int id)
        {
            Product product = GetAllProducts().FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Product is not found");
            }

            return product;
        }

        public async Task CreateProduct(CreateProductModel model)
        {
            // Assuming there should not be a same name product in the system.
            // So check for any product saved with same name before.
            bool isAnyProductWithSameName = GetAllProducts().Any(product => product.Name == model.Name);

            if (isAnyProductWithSameName)
            {
                throw new BadRequestException("A product with same name created before!");
            }

            var item = new Product
            {
                Name = model.Name,
                Description = model.Description,
                StockCount = model.StockCount
            };

            _unitOfWork.Add(item);

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateProduct(int id, UpdateProductModel model)
        {
            Product product = GetAllProducts().FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Product is not found");
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.StockCount = model.StockCount;

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteProduct(int id)
        {
            Product product = GetAllProducts().FirstOrDefault(u => u.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Product is not found");
            }

            if (product.IsDeleted)
            {
                return;
            }

            product.IsDeleted = true;

            await _unitOfWork.CommitAsync();
        }
    }
}
