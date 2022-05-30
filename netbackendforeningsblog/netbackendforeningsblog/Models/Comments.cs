using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netbackendforeningsblog.Models
{
    public class Comments
    {
        [Key]
        public int Id { get; set; }
        public string? Comment { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Event")]
        public int EventId { get; set; }

    }
}
