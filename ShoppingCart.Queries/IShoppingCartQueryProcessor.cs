using System.Linq;
using System.Threading.Tasks;

using ShoppingCart.Data.Model;
using ShoppingCart.Model.CartProduct;

namespace ShoppingCart.Queries
{
    /// <summary>
    /// Processes queries on the shopping cart.
    /// </summary>
    public interface IShoppingCartQueryProcessor
    {
        /// <summary>
        /// Gets all the product with counts info from the shopping cart.
        /// </summary>
        /// <returns>List of cart product info</returns>
        IQueryable<CartProduct> GetAllCartProducts();

        /// <summary>
        /// Gets the specified cart product info with count for given product id.
        /// </summary>
        /// <param name="productId">Id of the product</param>
        /// <returns>Cart product info</returns>
        CartProduct GetCartProduct(int productId);

        /// <summary>
        /// Adds the products to the cart.
        /// </summary>
        Task<CartProduct> AddToCart(CreateCartProductModel model);

        /// <summary>
        /// Removes the products for the given count with id from the cart.
        /// </summary>
        /// <param name="productId">Id of the product</param>
        /// <param name="count">Count of the removed products</param>
        Task RemoveFromCart(int productId, int count);

        /// <summary>
        /// Removes all the products for the given id from the cart.
        /// </summary>
        /// <param name="productId">Id of the product</param>
        Task RemoveAllFromCart(int productId);
    }
}
