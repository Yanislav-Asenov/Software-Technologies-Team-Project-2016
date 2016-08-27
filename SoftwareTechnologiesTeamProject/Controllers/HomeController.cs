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
            var comments = db.Comments.OrderByDescending(c => c.DateCreated).Take(10).ToList();
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
                posts.Where(p => p.Date <= DateTime.Now && p.Date >= DateTime.Now.AddDays(-7))
                    .OrderByDescending(p => p.Date)
                    .ThenByDescending(p => p.VotedUsers.Count)
                    .ToList();

            viewModel.FeaturedPosts = featuredPosts;

            viewModel.PopularTags = db.Tags.OrderByDescending(t => t.Posts.Count).Take(10).ToList();

            if (db.Images.Any())
            {
                viewModel.HomePageImagePath = db.Images
                .Where(i => i.ImagePath.Contains("homepage"))
                .OrderByDescending(i => i.UploadedDate)
                .First()
                .ImagePath;
            }
            return View(viewModel);
        }

    }
}