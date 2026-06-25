using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Task6.Models
{
    /// <summary>
    /// Представляет состав конкретного заказа.
    /// </summary>
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceAtOrder { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
