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
    using ViewModels;

    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts 
        public ActionResult Index(string searchString)
        {
            var posts = db.Posts
                .Include(p => p.Author)
                .OrderByDescending(p => p.Date)
                .ToList();

            var profiles = db.Profile.ToList();

            foreach (var post in posts)
            {
                post.Author.Profile = profiles.FirstOrDefault(p => p.UserId == post.AuthorId);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchedPosts = posts
                    .Where(p => p.Title.Contains(searchString) || p.Author.FullName.Contains(searchString) || p.Body.Contains(searchString))
                    .ToList();

                return View(searchedPosts);
            }

            return View(posts);
        }

        public ActionResult Tag(int? id)
        {
            var tag = db.Tags.Include(t => t.Posts.Select(p => p.Author)).FirstOrDefault(t => t.Id == id);

            if (tag == null)
            {
                return HttpNotFound();
            }

            var posts = tag.Posts;
            ViewBag.Header = tag.Name;

            return View(posts);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments.Select(c => c.Author))
                .Include(p => p.Tags)
                .First(p => p.Id == id);

            if (post == null)
            {
                this.AddNotification("Error occurred while trying to load post deitals.", NotificationType.ERROR);
                return RedirectToAction("Index", "Posts");
            }

            var viewModel = new PostDetailsViewModel
            {
                Post = post,
                CommentAuthorId = User.Identity.GetUserId(),
                PostId = post.Id,
                PostImage = db.Images.FirstOrDefault(i => i.ImagePath.Contains("PostId_" + post.Id))
            };


            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment([Bind(Include = "NewCommentContent,PostId,CommentAuthorId")] PostDetailsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var author = db.Users.Find(viewModel.CommentAuthorId);
                Comment newComment = new Comment
                {
                    PostId = viewModel.PostId,
                    Author = author,
                    Content = viewModel.NewCommentContent,
                    AuthorId = viewModel.CommentAuthorId
                };

                author.Comments.Add(newComment);
                db.Comments.Add(newComment);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = viewModel.PostId });
            }

            viewModel.Post = db.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments.Select(c => c.Author))
                .First(p => p.Id == viewModel.PostId);

            this.AddNotification("Error while adding the comment.", NotificationType.ERROR);
            return View("Details", viewModel);
        }

        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments.Find(id);

            int postId = comment.PostId;
            db.Comments.Remove(comment);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = postId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment(Comment comment)
        {
            comment.Author = db.Users.FirstOrDefault(u => u.Id == comment.AuthorId);
            comment.DateCreated = DateTime.Now;

            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = comment.PostId });
        }

        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Body,AuthorId,Tags")] CreatePostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Post post = new Post
                {
                    Author = db.Users.FirstOrDefault(u => u.Id == viewModel.AuthorId),
                    Title = viewModel.Title,
                    Body = viewModel.Body,
                    AuthorId = viewModel.AuthorId,
                };

                var existingTags = db.Tags.ToList();
                var inputTags = viewModel.GetTags();
                if (inputTags[0] != string.Empty)
                {
                    post.AddTags(inputTags, existingTags);
                }

                db.Posts.Add(post);
                db.SaveChanges();

                this.AddNotification("Post created successfully.", NotificationType.SUCCESS);
                return RedirectToAction("Details", new { id = post.Id });
            }

            this.AddNotification("Error while creating the post.", NotificationType.ERROR);
            return View();
        }

        // GET: Edit
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                this.AddNotification("Sorry this post does not exist.", NotificationType.ERROR);
                return HttpNotFound();
            }

            if (User.Identity.GetUserId() != post.AuthorId && !User.IsInRole("Administrator"))
            {
                this.AddNotification("You are not admin nor the post owner.", NotificationType.INFO);
                return RedirectToAction("Index");
            }

            EditPostViewModel viewModel = new EditPostViewModel(post, post.GetTagNames());

            return View(viewModel);
        }

        //POST Edit
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditPostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Post post = db.Posts.Include(p => p.Author).First(p => p.Id == viewModel.PostId);
                post.Tags.Clear();

                if (post.Author == null)
                    return HttpNotFound();

                var author = db.Users.FirstOrDefault(u => u.UserName == viewModel.AuthorUserName);
                if (author == null)
                {
                    this.AddNotification("Username does not exist.", NotificationType.ERROR);
                    return RedirectToAction("Edit", post.Id);
                }

                post.Update(author, viewModel);

                string[] tagNames = viewModel.GetTagNames();
                var allExistingTags = db.Tags.ToList();
                post.AddTags(tagNames, allExistingTags);

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();

                this.AddNotification("Post edited successfully.", NotificationType.SUCCESS);
                return RedirectToAction("Details", new { post.Id });
            }

            this.AddNotification("Could not edit the post. The input data is not valid.", NotificationType.ERROR);
            return View(viewModel);
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

            if (User.Identity.GetUserId() != post.AuthorId && !User.IsInRole("Administrator"))
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
            db.Posts.Remove(post);
            db.SaveChanges();

            this.AddNotification("Post deleted successfully.", NotificationType.SUCCESS);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult AddLike(int? id)
        {
            if (id == null)
            {
                this.AddNotification("Something went wrong please try again and check if the post exist.", NotificationType.ERROR);
                return View("Index");
            }

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var post = db.Posts.Find(id);

            post.VotedUsers.Add(user);

            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = post.Id });
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
