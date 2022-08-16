using AmanahAnnouncement.Data;
using AmanahAnnouncement.Interfaces;
using AmanahAnnouncement.Models;
using Microsoft.EntityFrameworkCore;

namespace AmanahAnnouncement.Repository
{
    public class FileModelRepository : IFileModelRepository
    {

        private readonly ApplicationDbContext db;
        public FileModelRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public bool Add(FileModel newFiles)
        {
            db.Add(newFiles);
            return Save();
        }

        public bool DeleteFiles(FileModel files)
        {
            // Deleting files from WWWRoot, assigning list of files containing files path.
            IEnumerable<FileModel> listOfFiles = db.FileModel.Where(x => x.AnnId == files.AnnId);
            if (listOfFiles.Count() > 0)
            {
                listOfFiles = db.FileModel.Where(x => x.NewsId == files.NewsId);
                foreach (FileModel file in listOfFiles) // Iterate each file that contains same announcement ID
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(file.FilePath);
                }
                db.RemoveRange(listOfFiles);
            }
            return Save();
        }

        public bool DeleteFile(string fileName)
        {
            FileModel desiredFile = db.FileModel.Where(x => x.FilePath.Contains(fileName)).FirstOrDefault();
            if (desiredFile != null)
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(desiredFile.FilePath);
                db.RemoveRange(desiredFile);
            }
            return Save();
        }


        public async Task<IEnumerable<FileModel>> GetAll()
        {
            return await db.FileModel.ToListAsync();
        }

        public async Task<FileModel> GetByIdAsync(int id)
        {
            FileModel file = await db.FileModel.FirstOrDefaultAsync(i => i.AnnId == id);
            if (file == null)
                file = await db.FileModel.FirstOrDefaultAsync(i => i.NewsId == id);
            return file;
            //return await db.FileModel.FirstOrDefaultAsync(i => i.AnnId == id);
        }

        public async Task<FileModel> GetByIdAsyncNoTracking(int id)
        {
            return await db.FileModel.AsNoTracking().FirstOrDefaultAsync(i => i.AnnId == id);
        }

        public bool Save()
        {
            var saved = db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(FileModel files)
        {
            db.Update(files);
            return Save();
        }


    }

}
