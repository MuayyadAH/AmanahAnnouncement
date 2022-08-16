using AmanahAnnouncement.Models;

namespace AmanahAnnouncement.Interfaces
{
    public interface IFileModelRepository
    {
        Task<IEnumerable<FileModel>> GetAll();
        Task<FileModel> GetByIdAsyncNoTracking(int id);
        Task<FileModel> GetByIdAsync(int id);
        bool Add(FileModel files);
        bool Update(FileModel files);
        bool DeleteFiles(FileModel files);
        bool DeleteFile(string fileName);
        bool Save();
    }
}
