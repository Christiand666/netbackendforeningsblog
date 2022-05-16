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
        //2022-05-20T17:00
        [DisplayFormat(DataFormatString = "{yyyy-MM-ddThh:mm}")]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        [DisplayFormat(DataFormatString = "{yyyy-MM-ddThh:mm}")]
        public DateTime StartDateTime { get; set; }
        [DisplayFormat(DataFormatString = "{yyyy-MM-ddThh:mm}")]
        public DateTime EndDateTime { get; set; }

    }
}
