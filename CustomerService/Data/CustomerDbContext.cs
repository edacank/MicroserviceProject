using Microsoft.EntityFrameworkCore;
using CustomerService.Models;
namespace CustomerService.Data
{
    public class CustomerDbContext : DbContext
    {


        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerNote> CustomerNotes { get; set; }


    }
}
