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
        public int RoleId { get; set; } // Role için Foreign Key
        public Role Role { get; set; } // Navigation Property
        public List<string> Roles { get; set; }  // Birden fazla rol olmalı
    }
}
