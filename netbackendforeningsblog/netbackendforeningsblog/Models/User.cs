using System.ComponentModel.DataAnnotations;

namespace netbackendforeningsblog.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; } 
        public UserRole UserRole { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

    }

    public enum UserRole
    {
        Admin = 0,
        User = 1 
    }
}
