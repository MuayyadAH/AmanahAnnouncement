using Microsoft.AspNetCore.Identity;

namespace AmanahAnnouncement.Models
{
    public class Users : IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string jobTitle { get; set; }
    }
}
