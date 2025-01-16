namespace CustomerService.Models
{
    public class CustomerNote
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerNotes { get; set; }
        //public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Customer Customer { get; set; }
    }
}
