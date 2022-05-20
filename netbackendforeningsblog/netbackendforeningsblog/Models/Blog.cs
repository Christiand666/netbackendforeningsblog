using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netbackendforeningsblog.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        //public User User { get; set; } 
        public string Author { get; set; } = string.Empty;
    }
}
