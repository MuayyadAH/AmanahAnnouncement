using Microsoft.EntityFrameworkCore;
using AmanahAnnouncement.Models;
using AmanahAnnouncement.Interfaces;
using AmanahAnnouncement.Data;
using System.Globalization;

namespace Amanahannouncments.Repository
{
    public class NewsRepository : INewsRepository
    {

        private readonly ApplicationDbContext db;
        public NewsRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public bool Add(News news)
        {
            db.Add(news);
            return Save();
        }

        public bool Delete(News news)
        {
            db.Remove(news);
            return Save();
        }


        public async Task<IEnumerable<News>> GetAll()
        {
            return await db.News.ToListAsync();
        }

        public async Task<IEnumerable<News>> getPublishedNews()
        {
            IEnumerable<News> publishedNews = db.News.Where(x => x.isPublished == true && x.isArchived == false).ToList();
            return publishedNews;
        }

        public async Task<News> GetByIdAsync(int id)
        {
            return await db.News.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<News> GetByIdAsyncNoTracking(int id)
        {
            return await db.News.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<News>> GetLastNews()
        {
            return await db.News.OrderByDescending(x => x.Id).Take(4).ToListAsync();
        }

        public bool Save()
        {
            var saved = db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(News news)
        {
            db.Update(news);
            return Save();
        }

        public bool ChangePublishStatus(News news)
        {
            if (news.isPublished == true)
            {
                news.isPublished = false;
            }
            else
            {
                news.isPublished = true;
            }
            db.Update(news);
            return Save();
        }

        public bool ChangeArchiveStatus(News news)
        {
            if (news.isArchived == true)
            {
                news.isArchived = false;
            }
            else
            {
                news.isArchived = true;
            }
            db.Update(news);
            return Save();
        }
    }
}
