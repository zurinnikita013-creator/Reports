using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Data;

namespace Task6.Models
{
    /// <summary>
    /// Представляет сущность товара в системе.
    /// </summary>
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
