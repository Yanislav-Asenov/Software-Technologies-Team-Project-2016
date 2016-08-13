namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using System.ComponentModel.DataAnnotations;
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
    }
}