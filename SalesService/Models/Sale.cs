using System.ComponentModel.DataAnnotations;

namespace SalesService.Models
{
    public class Sale
    {
        public int Id { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Status { get; set; } // Example: "Pending", "Completed", "Cancelled"
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
