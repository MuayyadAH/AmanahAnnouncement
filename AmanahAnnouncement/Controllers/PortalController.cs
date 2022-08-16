using AmanahAnnouncement.Data;
using AmanahAnnouncement.Interfaces;
using AmanahAnnouncement.Models;
using AmanahAnnouncement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace AmanahAnnouncement.Controllers
{
    public class PortalController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly INewsRepository newsRepo;
        private readonly IAnnouncementRepository annRepo;
        private readonly IPortalRepository portalRepo;
        private readonly SignInManager<Users> _signInManager;

        public PortalController(UserManager<Users> userManager, SignInManager<Users> signInManager, ApplicationDbContext context,
            INewsRepository newsRepo, IAnnouncementRepository annRepo, IPortalRepository portalRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            this.newsRepo = newsRepo;
            this.annRepo = annRepo;
            this.portalRepo = portalRepo;
        }

        [Authorize(Roles = "MediaHead, MediaEmployee")]
        public async Task<IActionResult> Index()
        {   
            dynamic NewsAnns = new ExpandoObject();
            NewsAnns.News = await portalRepo.getUnPublishedNews();
            NewsAnns.Anns = await portalRepo.getUnPublishedAnns();
            return View(NewsAnns);
        }

        [Route("PublishService/{id}/{control}")]
        [HttpGet]
        public async Task<IActionResult> PublishService(int id, string control)
        {
            if (User.Identity.IsAuthenticated)
                if (!User.IsInRole("MediaHead"))
                    return RedirectToAction("Error", "Home");

            if (control == "News")
            {
                var news = await newsRepo.GetByIdAsyncNoTracking(id);
                newsRepo.ChangePublishStatus(news);
            }
            else if (control == "Announcment")
            {
                var ann = await annRepo.GetByIdAsyncNoTracking(id);
                annRepo.ChangePublishStatus(ann);
                return RedirectToAction("Index", "Portal");
            }

            return RedirectToAction("Index", "Portal");
        }
        public IActionResult Login()
        {
            var response = new LoginVM();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM EnteredInfo)
        {
            if (!ModelState.IsValid) return View(EnteredInfo);
            if (EnteredInfo.Username.IndexOf('@') > -1)
            {
                //Validate email format
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                       @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                          @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(EnteredInfo.Username))
                {
                    ModelState.AddModelError("Username", "Email is not valid");
                }
            }
            else
            {
                //validate Username format
                string emailRegex = @"^[a-zA-Z0-9]*$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(EnteredInfo.Username))
                {
                    ModelState.AddModelError("Email", "Username is not valid");
                }
            }

            var userName = EnteredInfo.Username;
            if (userName.IndexOf('@') > -1)
            {
                var user = await _userManager.FindByEmailAsync(EnteredInfo.Username);
                if (user != null)
                {
                    userName = user.UserName;
                }
            }
            var result = await _signInManager.PasswordSignInAsync(userName, EnteredInfo.Password, false, lockoutOnFailure: false);
            if (result.Succeeded) return RedirectToAction("Index", "Announcment");

            ViewData["LoginFlag"] = "اسم المستخدم/كلمة المرور غير صحيحة!";
            return View(EnteredInfo);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Error", "Home");
            }
            await _signInManager.SignOutAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
