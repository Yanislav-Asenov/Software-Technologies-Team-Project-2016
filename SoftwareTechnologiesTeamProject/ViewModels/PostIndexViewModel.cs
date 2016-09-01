namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using SoftwareTechnologiesTeamProject.Models;
    using System.Collections.Generic;

    public class PostIndexViewModel
    {
        public List<Post> Posts { get; set; }

        public List<Tag> PopularTags { get; set; }
    }
}