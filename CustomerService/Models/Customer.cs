namespace CustomerService.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        //public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<CustomerNote> CustomerNotes { get; set; }
    }
}
