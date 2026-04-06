using System.ComponentModel.DataAnnotations;

namespace POSSampleOWN.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool DeleteFlag { get; set; } = false;

        public ICollection<Product>? Products { get; set; }
    }
}
