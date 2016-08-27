namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using Models;
    using System.Collections.Generic;

    public class HomePageViewModel
    {
        public List<Post> RecentPosts { get; set; }

        public List<Post> LastCommentedPosts { get; set; }

        public List<Post> FeaturedPosts { get; set; }

        public List<Tag> PopularTags { get; set; }

    }
}