using System.Data;

namespace UserService.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; } // Kullanıcının adı
        public string LastName { get; set; } // Kullanıcının soyadı
        public string Email { get; set; } // Kullanıcının e-posta adresi
        public string PasswordHash { get; set; } // Şifre (hashlenmiş şekilde saklanacak)
        public ICollection<UserRole> UserRoles { get; set; } // Many-to-Many ilişkisi


    }
}
