using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmanahAnnouncement.Models
{
    public class FileModel
    {
        [Key]
        public int FileId { get; set; }
        public string FilePath { get; set; }
        public int? AnnId { get; set; }
        public int? NewsId { get; set; }
    }
}
