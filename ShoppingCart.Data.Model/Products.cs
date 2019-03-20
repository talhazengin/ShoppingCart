using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Model
{
    public class Product
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }
    }
}
