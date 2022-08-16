using AmanahAnnouncement.Models;

namespace AmanahAnnouncement.Interfaces
{
    public interface IAnnouncementRepository
    {
        Task<IEnumerable<Announcment>> GetAll();
        Task<IEnumerable<Announcment>> getPublishedAnns();
        Task<Announcment> GetByIdAsyncNoTracking(int id);
        Task<Announcment> GetByIdAsync(int id);
        bool ChangePublishStatus(Announcment ann);
        bool ChangeArchiveStatus(Announcment ann);
        bool Add(Announcment ann);
        bool Update(Announcment ann);
        bool Delete(Announcment ann);
        bool Delete(FileModel files);
        bool Save();
        bool Add(FileModel newFile);
    }
}
