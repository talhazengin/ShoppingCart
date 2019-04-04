using System.Linq;
using System.Threading.Tasks;

using ShoppingCart.Core.Exceptions;
using ShoppingCart.Data.Access.DAL;
using ShoppingCart.Data.Model;
using ShoppingCart.Model.CartProduct;

namespace ShoppingCart.Queries
{
    public class ShoppingCartQueryProcessor : IShoppingCartQueryProcessor
    {
        private readonly IProductStockQueryProcessor _productStockQueryProcessor;
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartQueryProcessor(IUnitOfWork unitOfWork, IProductStockQueryProcessor productStockQueryProcessor)
        {
            _unitOfWork = unitOfWork;
            _productStockQueryProcessor = productStockQueryProcessor;
        }

        public IQueryable<CartProduct> GetAllCartProducts()
        {
            IQueryable<CartProduct> query = _unitOfWork.Query<CartProduct>()
                .Where(x => !x.IsDeleted);

            return query;
        }

        public CartProduct GetCartProduct(int productId)
        {
            CartProduct cartProduct = GetAllCartProducts().FirstOrDefault(cartProductInfo => cartProductInfo.ProductId == productId);

            if (cartProduct == null)
            {
                throw new NotFoundException("Product is not found in the shopping cart");
            }

            return cartProduct;
        }

        public async Task<CartProduct> AddToCart(CreateCartProductModel model)
        {
            Product product = _productStockQueryProcessor.GetProduct(model.ProductId);

            if (product.StockCount < model.Count)
            {
                throw new BadRequestException("There is no product until the desired amount.");
            }

            CartProduct cartProduct = GetAllCartProducts().FirstOrDefault(cartProductInfo => cartProductInfo.ProductId == model.ProductId);

            if (cartProduct != null)
            {
                cartProduct.Count += model.Count;

                _unitOfWork.Update(cartProduct);
            }
            else
            {
                cartProduct = new CartProduct { ProductId = model.ProductId, Count = model.Count };

                _unitOfWork.Add(cartProduct);
            }

            await _unitOfWork.CommitAsync();

            return cartProduct;
        }

        public async Task RemoveFromCart(int productId, int count)
        {
            CartProduct cartProduct = GetCartProduct(productId);

            if (count > cartProduct.Count)
            {
                throw new BadRequestException("Count for remove from the shopping cart can't be bigger than actual count.");
            }

            cartProduct.Count -= count;

            _unitOfWork.Update(cartProduct);

            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveAllFromCart(int productId)
        {
            CartProduct cartProduct = GetCartProduct(productId);

            _unitOfWork.Remove(cartProduct);

            await _unitOfWork.CommitAsync();
        }
    }
}
