using System.ComponentModel.DataAnnotations;

namespace POSSampleOWN.Models
{
    public class Tbl_Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(250)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool DeleteFlag { get; set; } = false;

        public ICollection<Tbl_Product>? Products { get; set; }
    }
}
