namespace SoftwareTechnologiesTeamProject.Controllers
{
    using Models;
    using System.Linq;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var recentPosts = db.Posts.OrderByDescending(x => x.Date).Take(5).ToList();
            ViewBag.RecentPosts = recentPosts;

            return View(); 
        }

    }
}