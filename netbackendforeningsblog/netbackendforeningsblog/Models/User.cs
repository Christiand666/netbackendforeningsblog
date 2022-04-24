using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace netbackendforeningsblog.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Email { get; set; } 
        public string? Password { get; set; } 
        public string? FullName { get; set; } 
        public Role Role { get; set; }
        

        [JsonIgnore]
        public string? PasswordHash { get; set; }
    }
    public enum Role
    {
        Admin,
        User
    }


}
