using AmanahAnnouncement.Interfaces;
using AmanahAnnouncement.Models;
using AmanahAnnouncement.ViewModel.NewsViewModel;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace AmanahAnnouncement.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository newsRepo;
        private readonly IFileModelRepository fileRepo;
        private IHttpContextAccessor _httpContextAccessor { get; }

        public NewsController(INewsRepository newsRepository, IHttpContextAccessor httpContextAccessor,
            IFileModelRepository fileModelRepository)
        {
            newsRepo = newsRepository;
            _httpContextAccessor = httpContextAccessor;
            fileRepo = fileModelRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<News> news = await newsRepo.getPublishedNews();
            return View(news);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateNewsVM news)
        {
            // Check if any user is logged in
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");

            if (ModelState.IsValid)
            {

                // SANITIZE USER INPUT
                var sanitizer = new HtmlSanitizer();
                var sanitizedTitle = sanitizer.Sanitize(news.Title, "http://www.example.com");
                var sanitizedContent = sanitizer.Sanitize(news.Content, "http://www.example.com");


                var newNews = new News()
                {
                    Title = sanitizedTitle,
                    Content = sanitizedContent,
                    CreationDate = DateTime.Now,
                    Attachement = null,
                    isPublished = false,
                    isArchived = false,
                    NewsUserId = _httpContextAccessor.HttpContext.User.GetUserId()
                };
                newsRepo.Add(newNews); // Adding the announcement for ID assignment

                List<string> listOfFiles = new List<string>();
                string uniqueFileName = null;
                if (news.File != null && news.File.Count > 0)
                {

                    foreach (IFormFile file in news.File)
                    {

                        string uploadsFolder = Path.Combine(@"wwwroot/Uploads", "News");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        file.CopyTo(new FileStream(filePath, FileMode.Create));

                        var newFile = new FileModel()
                        {
                            FilePath = filePath,
                            NewsId = newNews.Id,
                        };
                        listOfFiles.Add(uniqueFileName);

                        fileRepo.Add(newFile);
                    }
                }
                if (listOfFiles.Count() > 0)
                {
                    newNews.Attachement = String.Join(", ", listOfFiles.ToList());
                }
                else
                    newNews.Attachement = null;
                newsRepo.Update(newNews);
                //newsRepo.Add(newAnn);
                TempData["SuccessMessage"] = "تم اضافة الخبر بنجاح";
                TempData["NewsId"] = newNews.Id;
                return RedirectToAction("Index", "News");
            }
            return View(news);
        }

        public async Task<IActionResult> View(int id)
        {
            dynamic ViewNewsAndLastNews = new ExpandoObject();
            ViewNewsAndLastNews.ViewNews = await newsRepo.GetByIdAsync(id);
            if (ViewNewsAndLastNews.ViewNews == null) return RedirectToAction("Error", "Home");
            ViewNewsAndLastNews.LastNews = await newsRepo.GetLastNews();

            if ((ViewNewsAndLastNews.ViewNews.isPublished == false))
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Error", "Home");

            // News news = await newsRepo.GetByIdAsync(id);
            return View(ViewNewsAndLastNews);
        }

        public async Task<IActionResult> Edit(int id)
        {
            // Check if any user is logged in
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");

            var news = await newsRepo.GetByIdAsync(id);
            return View(news);

        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(News news)
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");
            if (!ModelState.IsValid) return View(news);

            News editedNews = await newsRepo.GetByIdAsync(news.Id);

            editedNews.Title = news.Title;
            editedNews.Content = news.Content;
            editedNews.ModifyDate = DateTime.Now;
            editedNews.isPublished = false;
            editedNews.isArchived = false;

            newsRepo.Update(editedNews);
            return RedirectToAction("Index", "News");
        }

        public async Task<IActionResult> Delete(int id)
        {
            // Check if any user is logged in
            if (await newsRepo.GetByIdAsync(id) == null) return RedirectToAction("Error", "Home");
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");

            return View(await newsRepo.GetByIdAsync(id));
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            // Check if any user is logged in
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");

            var news = await newsRepo.GetByIdAsync(id);
            var file = await fileRepo.GetByIdAsync(id);
            if (news != null)
            {
                try
                {
                    newsRepo.Delete(news); // Delete announcement from database.
                    if (file != null)
                        fileRepo.DeleteFiles(file); // Delete attachment file from database.
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return RedirectToAction("Error", "Home");
        }

        [Route("DeleteFileSeperate/{id}/{attachment}")]
        [HttpGet]
        public async Task<IActionResult> DeleteFileSeperate(string attachment, int id)
        {
            // Check if any user is logged in
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");

            News news = await newsRepo.GetByIdAsync(id);
            if (news != null)
            {
                List<string> attachmentsList = news.Attachement.Split(", ").ToList();
                attachmentsList.Remove(attachment);
                if (attachmentsList.Count == 0)
                {
                    news.Attachement = null;
                }
                else
                {
                    news.Attachement = String.Join(", ", attachmentsList.ToArray());
                }
                news.ModifyDate = DateTime.Now;

                fileRepo.DeleteFile(attachment);

                newsRepo.Update(news);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Edit", "News", new { @id = id });
        }
    }
}
