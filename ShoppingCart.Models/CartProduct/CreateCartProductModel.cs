using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Model.CartProduct
{
    public class CreateCartProductModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
