using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasTacos.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime OrderTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public string OrderStatus { get; set; } = "Pending";

        [Required]
        public string OrderType { get; set; }

        public int? TimeSlotId { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual TimeSlot TimeSlot { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual SurveyResponse SurveyResponse { get; set; }
    }
}