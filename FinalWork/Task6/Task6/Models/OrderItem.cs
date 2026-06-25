using System.Collections.Generic;
using System.Linq;
using Task6.Models;

namespace Task6.Models
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalItemPrice => Product != null ? Product.Price * Quantity : 0;
    }
}
