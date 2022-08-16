using System.ComponentModel.DataAnnotations;

namespace AmanahAnnouncement.ViewModel.NewsViewModel
{
    public class CreateNewsVM
    {
        [Required(ErrorMessage = "عنوان الخبر إلزامي")]
        [Display(Name = "عنوان الخبر")]
        public string Title { get; set; }
        public string? Content { get; set; }
        [Display(Name = "إرفاق ملفات")]
        public List<IFormFile>? File { get; set; }
    }
}
