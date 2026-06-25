namespace Task6.Models
{
    public static class ShoppingCart
    {
        public static List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public static void AddProduct(Product product)
        {
            var existingItem = Items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                Items.Add(new OrderItem { Product = product, Quantity = 1 });
            }
        }

        public static bool HasItems => Items.Count > 0;
    }
}
