using AmanahAnnouncement.Data;
using AmanahAnnouncement.Interfaces;
using AmanahAnnouncement.Models;

namespace AmanahAnnouncement.Repository
{
    public class PortalRepository : IPortalRepository
    {
        private readonly ApplicationDbContext db;
        public PortalRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Announcment>> getArchivedNAnns()
        {
            IEnumerable<Announcment> ArchivedAnns = db.Announcments.Where(x => x.isArchived == true).ToList();
            return ArchivedAnns;
        }

        public async Task<IEnumerable<News>> getArchivedNews()
        {
            IEnumerable<News> ArchivedNews = db.News.Where(x => x.isArchived == true).ToList();
            return ArchivedNews;
        }

        public async Task<IEnumerable<Announcment>> getUnPublishedAnns()
        {
            IEnumerable<Announcment> unPublishedAnns = db.Announcments.Where(x => x.isPublished == false).ToList();
            return unPublishedAnns;
        }

        public async Task<IEnumerable<News>> getUnPublishedNews()
        {
            IEnumerable<News> unPublishedNews = db.News.Where(x => x.isPublished == false).ToList();
            return unPublishedNews;
        }

    }
}
