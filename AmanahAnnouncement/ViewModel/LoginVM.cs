using System.ComponentModel.DataAnnotations;

namespace AmanahAnnouncement.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "اسم المستخدم او الايميل مطلوب")]
        public string Username { get; set; }
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
