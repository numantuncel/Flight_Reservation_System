using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }  // Kullanıcının adı
        public string LastName { get; set; }   // Kullanıcının soyadı
        public string? ProfilePicture { get; set; }  // Profil fotoğrafı (URL)
        public string PhoneNumber { get; set; }  // Kullanıcının telefon numarası
        public string PasswordHash { get; set; } // MD5 şifrelenmiş
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string RoleUrl { get; set; }
        public string Role { get; set; }

    }
}
