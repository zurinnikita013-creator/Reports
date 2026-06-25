using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Task6.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(150)]
        public string FIO { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; }
    }
}
