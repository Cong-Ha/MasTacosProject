using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasTacos.Models
{
    [Table("SurveyResponses")]
    public class SurveyResponse
    {
        [Key]
        public int ResponseId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public DateTime SubmissionDate { get; set; }

        public int? FoodRating { get; set; }

        public int? ServiceRating { get; set; }

        public int? AmbienceRating { get; set; }

        public string Feedback { get; set; }

        public bool FollowedUp { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual Order Order { get; set; }
    }
}