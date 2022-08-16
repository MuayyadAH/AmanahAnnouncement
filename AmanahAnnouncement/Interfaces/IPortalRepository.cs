using AmanahAnnouncement.Models;

namespace AmanahAnnouncement.Interfaces
{
    public interface IPortalRepository
    {
        Task<IEnumerable<Announcment>> getUnPublishedAnns();
        Task<IEnumerable<News>> getUnPublishedNews();
        Task<IEnumerable<News>> getArchivedNews();
        Task<IEnumerable<Announcment>> getArchivedNAnns();
    }
}
