using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasTacos.Models
{
    [Table("MenuItems")]
    public class MenuItem
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Category { get; set; }

        public bool IsActive { get; set; } = true;

        public int PopularityScore { get; set; }

        public byte[]? ImageData { get; set; }

        public string? ImageMimeType { get; set; }
    }
}