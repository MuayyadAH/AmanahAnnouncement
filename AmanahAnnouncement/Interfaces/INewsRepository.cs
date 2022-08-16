using AmanahAnnouncement.Models;

namespace AmanahAnnouncement.Interfaces
{
    public interface INewsRepository
    {
        Task<IEnumerable<News>> GetAll();
        Task<IEnumerable<News>> getPublishedNews();
        Task<News> GetByIdAsyncNoTracking(int id);
        Task<News> GetByIdAsync(int id);
        bool Add(News news);
        bool Update(News news);
        bool Delete(News news);
        bool Save();
        bool ChangePublishStatus(News news);
        bool ChangeArchiveStatus(News news);
        Task<IEnumerable<News>> GetLastNews();
    }
}
