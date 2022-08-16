using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmanahAnnouncement.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public string? Attachement { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        [ForeignKey("Users")]
        public string? NewsUserId { get; set; }
        public Users? Users { get; set; }
        public bool? isPublished { get; set; }
        public bool? isArchived { get; set; }
    }
}