using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShoppingCart.Data.Model;
using ShoppingCart.Filters;
using ShoppingCart.Maps;
using ShoppingCart.Model.CartProduct;
using ShoppingCart.Model.Product;
using ShoppingCart.Queries;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Security and authorization features are not implemented for this demo project.
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartQueryProcessor _shoppingCartQueryProcessor;
        private readonly IProductStockQueryProcessor _productStockQueryProcessor;
        private readonly IAutoMapper _autoMapper;

        public ShoppingCartController(IShoppingCartQueryProcessor shoppingCartQueryProcessor, IProductStockQueryProcessor productStockQueryProcessor,  IAutoMapper autoMapper)
        {
            _shoppingCartQueryProcessor = shoppingCartQueryProcessor;
            _productStockQueryProcessor = productStockQueryProcessor;
            _autoMapper = autoMapper;

            CreateDummyProductsForThisDemoProject();
        }

        [HttpGet("stock")]
        public IEnumerable<ProductModel> GetAllStockProducts()
        {
            IQueryable<Product> products = _productStockQueryProcessor.GetAllProducts();

            IQueryable<ProductModel> productModels = _autoMapper.Map<Product, ProductModel>(products);

            return productModels;
        }

        [HttpGet]
        public IEnumerable<CartProductModel> GetAllCartProducts()
        {
            IQueryable<CartProduct> cartProducts = _shoppingCartQueryProcessor.GetAllCartProducts();

            IQueryable<CartProductModel> cartProductModels = _autoMapper.Map<CartProduct, CartProductModel>(cartProducts);

            return cartProductModels;
        }

        [HttpGet("{productId}")]
        public ActionResult<CartProductModel> GetCartProduct(int productId)
        {
            CartProduct cartProduct = _shoppingCartQueryProcessor.GetCartProduct(productId);

            return _autoMapper.Map<CartProductModel>(cartProduct);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<CartProductModel> AddToCart([FromBody] CreateCartProductModel requestModel)
        {
            CartProduct cartProduct = await _shoppingCartQueryProcessor.AddToCart(requestModel);

            return _autoMapper.Map<CartProductModel>(cartProduct);
        }

        [HttpDelete]
        public async Task RemoveAllFromCart(int productId)
        {
            await _shoppingCartQueryProcessor.RemoveAllFromCart(productId);
        }

        [HttpDelete("{productId}")]
        public async Task RemoveFromCart(int productId, int count)
        {
            await _shoppingCartQueryProcessor.RemoveFromCart(productId, count);
        }

        private void CreateDummyProductsForThisDemoProject()
        {
            for (int i = 1; i < 10; i++)
            {
                var productModel = new CreateProductModel
                                             {
                                                 Name = $"Prod{i}",
                                                 StockCount = i,
                                                 Description = $"Desc{i}"
                                             };

                _productStockQueryProcessor.CreateProduct(productModel);
            }
        }
    }
}
