using SoftwareTechnologiesTeamProject.Models;
using System.Web;
using System.Web.Mvc;

namespace SoftwareTechnologiesTeamProject.Controllers
{
    public class ImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Image img = new Image();

                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Content/Images/")
                                                          + file.FileName);
                    img.ImagePath = file.FileName;
                }
                db.Images.Add(img);
                db.SaveChanges();
                return View("Index", img);
            }
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
