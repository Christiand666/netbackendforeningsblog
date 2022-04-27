using System.ComponentModel.DataAnnotations;

namespace netbackendforeningsblog.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Place { get; set; } = string.Empty;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

    }
}
