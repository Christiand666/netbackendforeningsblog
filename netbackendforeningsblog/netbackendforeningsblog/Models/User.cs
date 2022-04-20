using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace netbackendforeningsblog.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public Role Role { get; set; }
        

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
    public enum Role
    {
        Admin,
        User
    }


}
