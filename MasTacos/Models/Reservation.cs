using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasTacos.Models
{
    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime ReservationTime { get; set; }

        [Required]
        public int PartySize { get; set; }

        [Required]
        public string Status { get; set; } = "Confirmed";

        public string? SpecialRequests { get; set; }

        public int? TimeSlotId { get; set; }

        // Navigation properties
        public virtual Customer? Customer { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }
    }
}