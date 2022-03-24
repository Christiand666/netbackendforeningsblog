using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netbackendforeningsblog.Models
{
    public class SignedupUsers
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event Event { get; set; } = new Event();

    }
}
