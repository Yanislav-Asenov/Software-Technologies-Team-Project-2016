namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    public class CreatePostViewModel
    {
        [Required]
        [AllowHtml]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        public string Body { get; set; }

        public string AuthorId { get; set; }

        [StringLength(100)]
        public string Tags { get; set; }

        public Image Image { get; set; }
        public string[] GetTags()
        {
            return Tags.Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
        }
    }
}