using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasTacos.Models
{
    [Table("TimeSlots")]
    public class TimeSlot
    {
        [Key]
        public int SlotId { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public string DayOfWeek { get; set; }

        public int? AvgCustomerVolume { get; set; }

        public bool PeakHours { get; set; }
    }
}