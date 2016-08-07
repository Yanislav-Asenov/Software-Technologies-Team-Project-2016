

using SoftwareTechnologiesTeamProject.Extensions;
using System;

namespace SoftwareTechnologiesTeamProject.Controllers
{
    using Microsoft.AspNet.Identity;
    using Models;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.Include(p => p.Author).ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            post.Author = db.Users.FirstOrDefault(u => u.Id == post.AuthorId);

            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                db.Posts.Add(post);
                db.SaveChanges();
                this.AddNotification("Post created successfully.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            this.AddNotification("Error while creating the post.", NotificationType.ERROR);
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                this.AddNotification("Sorry this post does not exist.", NotificationType.ERROR);
                return HttpNotFound();
            }

            if (User.Identity.GetUserId() != post.AuthorId && !User.IsInRole("Administrators"))
            {
                this.AddNotification("You are not admin nor the post owner.", NotificationType.INFO);
                return RedirectToAction("Index");
            }

            ViewBag.Authors = db.Users.ToList();


            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Date,AuthorId")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                if (post.Date == null)
                {
                    post.Date = DateTime.Now;
                }
                db.SaveChanges();

                this.AddNotification("Post edited successfully.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }


            this.AddNotification("Could not edit the post. The input data is not valid.", NotificationType.ERROR);
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.GetUserId() != post.AuthorId && !User.IsInRole("Administrators"))
            {
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);

            if (User.Identity.GetUserId() != post.AuthorId && !User.IsInRole("Administrators"))
            {
                this.AddNotification("You are not admin nor the post owner.", NotificationType.INFO);
                return RedirectToAction("Index");
            }

            db.Posts.Remove(post);
            db.SaveChanges();

            this.AddNotification("Post deleted successfully.", NotificationType.SUCCESS);

            return RedirectToAction("Index");
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
