using Microsoft.EntityFrameworkCore;
using AmanahAnnouncement.Models;
using AmanahAnnouncement.Interfaces;
using AmanahAnnouncement.Data;
using System.Globalization;

namespace Amanahannouncments.Repository
{
    public class AnnouncementRepository : IAnnouncementRepository
    {

        private readonly ApplicationDbContext db;
        public AnnouncementRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public bool Add(Announcment ann)
        {
            db.Add(ann);
            return Save();
        }

        public bool Add(FileModel newFile)
        {
            db.Add(newFile);
            return Save();
        }

        public bool Delete(Announcment ann)
        {
            db.Remove(ann);
            return Save();
        }

        public bool Delete(FileModel files)
        {
            db.Remove(files);
            return Save();
        }

        public async Task<IEnumerable<Announcment>> GetAll()
        {
            return await db.Announcments.ToListAsync();
        }

        public async Task<IEnumerable<Announcment>> getPublishedAnns()
        {
            IEnumerable<Announcment> publishedAnns = db.Announcments.Where(x => x.isPublished == true && x.isArchived == false).ToList();
            return publishedAnns;
        }

        public async Task<Announcment> GetByIdAsync(int id)
        {
            return await db.Announcments.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Announcment> GetByIdAsyncNoTracking(int id)
        {
            return await db.Announcments.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Announcment ann)
        {
            db.Update(ann);
            return Save();
        }

        public bool ChangePublishStatus(Announcment ann)
        {
            if (ann.isPublished == true)
            {
                ann.isPublished = false;
            }
            else
            {
                ann.isPublished = true;
            }
            db.Update(ann);
            return Save();
        }

        public bool ChangeArchiveStatus(Announcment ann)
        {
            if (ann.isArchived == true)
            {
                ann.isArchived = false;
            }
            else
            {
                ann.isArchived = true;
            }
            db.Update(ann);
            return Save();
        }
    }
}
