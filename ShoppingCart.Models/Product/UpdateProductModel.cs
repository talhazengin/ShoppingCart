using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Model.Product
{
    public class UpdateProductModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int StockCount { get; set; }
    }
}
