using System;
using System.ComponentModel.DataAnnotations;

namespace PROGPOE3.Models
{
    public class Claim
    {
        public int Id { get; set; }

        [Required]
        public string ClaimType { get; set; } = string.Empty;

        [Required]
        public decimal HoursWorked { get; set; }

        [Required]
        public decimal HourlyRate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string? Notes { get; set; }

        public string? SupportingDocument { get; set; }

        public DateTime DateSubmitted { get; set; }

        // Status FK
        [Required]
        public int ClaimStatusId { get; set; }
        public ClaimStatus? ClaimStatus { get; set; }

        // Lecturer FK
       
        public int? LecturerId { get; set; }
        public Lecturer? Lecturer { get; set; }
    }
}
