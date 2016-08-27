using System;
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
        public ActionResult AddHomePageImage(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                Image img = new Image();

                string homePageImgName = "homepage"+file.FileName;

                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Content/Images/HomePage/")
                                                          + homePageImgName);
                    img.ImagePath = homePageImgName;
                }
                img.UploadedDate = DateTime.Now;

                db.Images.Add(img);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View("Index");
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
