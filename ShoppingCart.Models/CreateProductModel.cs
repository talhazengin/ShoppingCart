using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Model
{
    public class CreateProductModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
