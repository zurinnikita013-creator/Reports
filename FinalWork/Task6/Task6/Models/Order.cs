using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Task6.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Дата доставки заказа сотрудником. Может быть неопределенной (null).
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [MaxLength(150)]
        public string CustomerName { get; set; }

        public int? UserId { get; set; }

        /// <summary>
        /// Трехзначный случайно сгенерированный код получения талона.
        /// </summary>
        [Required]
        [MaxLength(3)]
        public string PickupCode { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
