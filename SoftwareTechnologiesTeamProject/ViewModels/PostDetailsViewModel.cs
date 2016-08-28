namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using Models;
    using System.ComponentModel.DataAnnotations;

    public class PostDetailsViewModel
    {
        public Post Post { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 1)]
        public string NewCommentContent { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public string CommentAuthorId { get; set; }

        public string AddPostImagePaht { get; set; }

    }
}