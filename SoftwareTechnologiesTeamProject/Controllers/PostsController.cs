namespace SoftwareTechnologiesTeamProject.Controllers
{
    using Extensions;
    using Microsoft.AspNet.Identity;
    using Models;
    using System;
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

            Post post = db.Posts.Include(p => p.Comments).First(x => x.Id == id);

            if (post == null)
            {
                return HttpNotFound();
            }

            foreach (var comment in post.Comments)
            {
                comment.Author = db.Users.Find(comment.AuthorId);

            }

            return View(post);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(Comment comment)
        {
            var post = db.Posts.Find(comment.PostId);

            try
            {
                comment.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                db.Comments.Add(comment);

                db.SaveChanges();

                return RedirectToAction("Details", new { id = post.Id });
            }
            catch
            {
                this.AddNotification("Comment field must containt atleast 1 character.", NotificationType.ERROR);
                return RedirectToAction("Details", new { id = post.Id });
            }

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
            Post post = db.Posts.Include(p => p.Author).FirstOrDefault(x => x.Id == id);
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

            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                var authorId = db.Users.FirstOrDefault(x => x.UserName == post.Author.UserName)?.Id;

                //If author name does not exists send error message
                if (authorId == null)
                {
                    this.AddNotification("Username does not exist.", NotificationType.ERROR);
                    return RedirectToAction("Edit", post.Id);
                }

                post.AuthorId = authorId;
                post.Author = db.Users.FirstOrDefault(user => user.Id == post.AuthorId);

                if (post.Date == null)
                {
                    post.Date = DateTime.Now;
                }

                db.Entry(post).State = EntityState.Modified;

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
