namespace UserService.Models
{
    public class Role
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // Rolün adı (örn. "Admin", "User")
        public ICollection<User> Users { get; set; } // Role ait kullanıcılar
        
    }
}
