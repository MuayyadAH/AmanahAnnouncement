using AmanahAnnouncement.Models;

namespace AmanahAnnouncement.ViewModel
{
    public class ViewAnnVM
    {
        public IEnumerable<Announcment> announcments { get; set; }
        public IEnumerable<FileModel> Files { get; set; }
    }
}
