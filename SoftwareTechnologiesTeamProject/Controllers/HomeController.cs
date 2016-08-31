namespace SoftwareTechnologiesTeamProject.Controllers
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using ViewModels;



    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var viewModel = new HomePageViewModel();

            var posts = db.Posts.Include(p => p.Author).Include(p => p.Comments).ToList();

            //Adding recent posts to view model
            var recentPosts = posts.OrderByDescending(p => p.Date).Take(5).ToList();
            viewModel.RecentPosts = recentPosts;

            //Adding last commented posts in view model
            var comments = db.Comments.OrderByDescending(c => c.DateCreated).ToList();
            var lastCommentedPosts = new List<Post>();
            foreach (var comment in comments)
            {
                var post = posts.Find(p => p.Id == comment.PostId);

                if (!lastCommentedPosts.Contains(post))
                {
                    lastCommentedPosts.Add(post);
                }

                if (lastCommentedPosts.Count > 2)
                {
                    break;
                }
            }

            viewModel.LastCommentedPosts = lastCommentedPosts;

            var featuredPosts =
                posts.Where(p => p.Date <= DateTime.Now)
                    .OrderByDescending(p => p.Date)
                    .ThenByDescending(p => p.VotedUsers.Count + p.Comments.Count)
                    .Take(5)
                    .ToList();

            viewModel.FeaturedPosts = featuredPosts;

            viewModel.PopularTags = db.Tags.OrderByDescending(t => t.Posts.Count).Take(10).ToList();

            var images = db.Images.ToList();

            var homePageImage = images
                .Where(i => i.ImagePath.Contains("homepage"))
                .OrderByDescending(i => i.UploadedDate)
                .FirstOrDefault();

            if (homePageImage != null)
            {
                viewModel.HomePageImagePath = homePageImage.ImagePath;
            }

            foreach (var post in viewModel.FeaturedPosts)
            {
                post.Image = images.OrderByDescending(i => i.UploadedDate).FirstOrDefault(i => i.ImagePath.Contains("PostId_" + post.Id));
            }

            return View(viewModel);
        }
    }
}