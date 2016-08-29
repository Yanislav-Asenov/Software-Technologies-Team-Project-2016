using System;
using System.Linq;
using SoftwareTechnologiesTeamProject.Models;
using System.Web;
using System.Web.Mvc;
using SoftwareTechnologiesTeamProject.Extensions;
using SoftwareTechnologiesTeamProject.ViewModels;

namespace SoftwareTechnologiesTeamProject.Controllers
{
    public class ImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private readonly string[] allowedFileExtensions = new[] {"png", "jpg", "jpeg", "gif"};

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
                if (file == null)
                {
                    this.AddNotification("Choose image!", NotificationType.ERROR);
                    return RedirectToAction("Index", "Home");
                }

                var fileExtension = file.FileName.Split('.').Last();
                if (!allowedFileExtensions.Contains(fileExtension.ToLower()))
                {
                    this.AddNotification("Not allowed file extension", NotificationType.ERROR);
                    return RedirectToAction("Index", "Home");
                }

                Image img = new Image();

                string homePageImgName = "homepage" + file.FileName;

                    
                file.SaveAs(HttpContext.Server.MapPath("~/Content/Images/HomePage/")
                                                            + homePageImgName);
                img.ImagePath = homePageImgName;
                    
                img.UploadedDate = DateTime.Now;

                db.Images.Add(img);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
              
            }
            return View("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPostImage(HttpPostedFileBase file)
        {
            
            if (ModelState.IsValid)
            {
                string check = (HttpContext.Request.FilePath.ToString().Split('/').Last());
                int postid = int.Parse(check);
                Image img = new Image();

                if (file == null)
                {
                    return RedirectToAction("Details", "Posts", new { id = postid });
                }

                var fileExtension = file.FileName.Split('.').Last();
                if (!allowedFileExtensions.Contains(fileExtension.ToLower()))
                {
                    this.AddNotification("Not allowed file extension", NotificationType.ERROR);
                    return RedirectToAction("Index", "Home");
                }

                string postImgName = "PostId_" + check + file.FileName;

                file.SaveAs(HttpContext.Server.MapPath("~/Content/Images/PostImages/")
                                                            + postImgName);
                img.ImagePath = postImgName;
                    
                img.UploadedDate = DateTime.Now;

                db.Images.Add(img);
                db.SaveChanges();

                return RedirectToAction("Details", "Posts", new { id = postid });
                
                
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
