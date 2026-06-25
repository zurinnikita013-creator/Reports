using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task5.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Manufacturer { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
