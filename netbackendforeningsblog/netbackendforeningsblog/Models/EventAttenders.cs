using System.ComponentModel.DataAnnotations.Schema;

namespace netbackendforeningsblog.Models
{
    [NotMapped]
    public class EventAttenders
    {
        public Event Event { get; set; }
        public List<User> Attenders { get; set; }
    }
}
