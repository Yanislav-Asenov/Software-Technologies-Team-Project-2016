using System;

namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public class EditPostViewModel
    {
        public int PostId { get; set; }

        [Required]
        [AllowHtml]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        public string Body { get; set; }

        public string Tags { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "datetime2")]
        public DateTime? Date { get; set; }

        [Required]
        [DisplayName("Author username")]
        [EmailAddress]
        public string AuthorUserName { get; set; }


    }
}