using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasTacos.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        public bool MarketingOptIn { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        public int LoyaltyPoints { get; set; }

        // Navigation properties
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<SurveyResponse> SurveyResponses { get; set; }
    }
}