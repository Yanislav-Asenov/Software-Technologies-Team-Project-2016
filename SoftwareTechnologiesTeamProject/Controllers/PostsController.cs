namespace SoftwareTechnologiesTeamProject.Controllers
{
    using Extensions;
    using Microsoft.AspNet.Identity;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using ViewModels;

    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts 
        public ActionResult Index()
        {
            return View(db.Posts.Include(p => p.Author).ToList());
        }

        public ActionResult Tag(int? id)
        {
            var tag = db.Tags.Include(t => t.Posts.Select(p => p.Author)).FirstOrDefault(t => t.Id == id);

            if (tag == null)
            {
                return HttpNotFound();
            }

            var posts = new List<Post>();
            foreach (var post in tag.Posts)
            {
                posts.Add(post);
            }
            ViewBag.Title = tag.Name;

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
                PostId = post.Id
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

            this.AddNotification("Error while creating the post.", NotificationType.ERROR);
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

        public ActionResult EditComment(Comment comment)
        {
            comment.Author = db.Users.FirstOrDefault(u => u.Id == comment.AuthorId);

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

                var tags = db.Tags.ToList();
                var inputTags = viewModel.Tags.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                foreach (var tagName in inputTags)
                {
                    if (tags.FirstOrDefault(t => t.Name == tagName) == null)
                    {
                        post.Tags.Add(new Tag
                        {
                            Name = tagName
                        });
                    }
                    else
                    {
                        Tag tag = tags.Find(x => x.Name == tagName);
                        post.Tags.Add(tag);
                        tag.Posts.Add(post);
                    }
                }

                db.Posts.Add(post);
                db.SaveChanges();

                this.AddNotification("Post created successfully.", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            this.AddNotification("Error while creating the post.", NotificationType.ERROR);
            return View();
        }

        // GET: Posts/Edit/5
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
                post.Author = db.Users.FirstOrDefault(x => x.UserName == post.Author.UserName);

                if (post.Author == null)
                {
                    this.AddNotification("Username does not exist.", NotificationType.ERROR);
                    return RedirectToAction("Edit", post.Id);
                }

                post.AuthorId = post.Author.Id;
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
