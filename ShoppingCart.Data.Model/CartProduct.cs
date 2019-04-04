namespace ShoppingCart.Data.Model
{
    public class CartProduct
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }

        public bool IsDeleted { get; set; }
    }
}
