using System.ComponentModel.DataAnnotations;

namespace AmanahAnnouncement.ViewModel
{
    public class CreateAnnVM
    {
        [Required(ErrorMessage = "عنوان الاعلان إلزامي")]
        [Display(Name = "عنوان الاعلان")]
        public string Title { get; set; }
        public string? Content { get; set; }
        public List<IFormFile>? File { get; set; }
    }
}
