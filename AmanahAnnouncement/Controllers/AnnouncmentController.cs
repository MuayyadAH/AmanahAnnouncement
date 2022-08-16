using AmanahAnnouncement.Interfaces;
using AmanahAnnouncement.Models;
using AmanahAnnouncement.ViewModel;
using AutoMapper;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AmanahAnnouncement.Controllers
{

    public class AnnouncmentController : Controller
    {
        public IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IFileModelRepository fileRepo;
        private readonly IAnnouncementRepository annRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AnnouncmentController(IAnnouncementRepository annRepo,
            IMapper mapper, IFileModelRepository fileRepo, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            this.fileRepo = fileRepo;
            this.annRepo = annRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Announcment> announcments = await annRepo.getPublishedAnns();
            return View(announcments);
        }

        public async Task<IActionResult> View(int id)
        {
            Announcment ann = await annRepo.GetByIdAsync(id);
            // redirect to error if there is no announcement
            if (ann == null) return RedirectToAction("Error", "Home");

            if ((ann.isPublished == false) || ann.isArchived == true)
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Error", "Home");

            return View(ann); // view announcement
        }

        public IActionResult Create()
        {
            // Check if any user is logged in
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateAnnVM ann)
        {
            // Check if any user is logged in
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");

            if (ModelState.IsValid)
            {

                // SANITIZE USER INPUT
                var sanitizer = new HtmlSanitizer();
                var sanitizedTitle = sanitizer.Sanitize(ann.Title, "http://www.example.com");
                var sanitizedContent = sanitizer.Sanitize(ann.Content, "http://www.example.com");


                var newAnn = new Announcment()
                {
                    Title = sanitizedTitle,
                    Content = sanitizedContent,
                    CreationDate = DateTime.Now,
                    Attachement = null,
                    isPublished = false,
                    isArchived = false,
                    AnnouncementUserId = _httpContextAccessor.HttpContext.User.GetUserId()
                };
                annRepo.Add(newAnn); // Adding the announcement for ID assignment

                List<string> listOfFiles = new List<string>();
                string uniqueFileName = null;
                if (ann.File != null && ann.File.Count > 0)
                {

                    foreach (IFormFile file in ann.File)
                    {

                        string uploadsFolder = Path.Combine(@"wwwroot/Uploads", "Announcement");
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
                            AnnId = newAnn.Id,
                        };
                        listOfFiles.Add(uniqueFileName);

                        annRepo.Add(newFile);
                    }
                }
                if (listOfFiles.Count() > 0)
                {
                    newAnn.Attachement = String.Join(", ", listOfFiles.ToList());
                }
                else
                    newAnn.Attachement = null;
                annRepo.Update(newAnn);
                //annRepo.Add(newAnn);
                TempData["SuccessMessage"] = "تم اضافة الاعلان بنجاح";
                TempData["AnnId"] = newAnn.Id;
                return RedirectToAction("Index", "Announcment");
            }
            return View(ann);
        }

        public async Task<IActionResult> Delete(int id)
        {
            // Check if any user is logged in
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");

            var ann = await annRepo.GetByIdAsync(id);
            if (ann != null)
            {
                return View(ann);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAnn(int id)
        {
            // Check if any user is logged in
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");

            var ann = await annRepo.GetByIdAsync(id);
            var file = await fileRepo.GetByIdAsync(id);
            if (ann != null)
            {
                ann.isPublished = false;
                ann.isArchived = true;
                annRepo.Update(ann);
                return RedirectToAction("Index");
            }
            //var file = await fileRepo.GetByIdAsync(id);
            //if (ann != null)
            //{
            //    annRepo.Delete(ann); // Delete announcement from database.
            //    if (file != null)
            //        fileRepo.DeleteFiles(file); // Delete attachment file from database.
            //    return RedirectToAction("Index");
            //}
            return RedirectToAction("Error", "Home");
        }

        public async Task<IActionResult> Edit(int id)
        {
            // Check if any user is logged in
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");

            var ann = await annRepo.GetByIdAsyncNoTracking(id);
            var annVM = new EditAnnVM
            {
                Title = ann.Title,
                Content = ann.Content,
                File = null // Configure later
            };
            TempData["SuccessMessage"] = "تم تعديل الإعلان بنجاح";
            TempData["AnnId"] = id;

            if (!ModelState.IsValid) return RedirectToAction("Error", "Home");

            return ann == null ? RedirectToAction("Error", "Home") : View(annVM);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditAnn(int id, EditAnnVM EditVM)
        {
            // Check if user is logged in
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Error", "Home");

            if (!ModelState.IsValid) return RedirectToAction("Error", "Home");

            var ann = await annRepo.GetByIdAsyncNoTracking(id);
            if (ann != null)
            {
                ann.Id = ann.Id;
                ann.Title = EditVM.Title;
                ann.Content = EditVM.Content;
                ann.Attachement = ann.Attachement;
                ann.ModifyDate = DateTime.Now;
                ann.isPublished = false;
                ann.isArchived = false;
                annRepo.Update(ann);
            }
            return RedirectToAction("Index", "Announcment");
        }


    }
}
