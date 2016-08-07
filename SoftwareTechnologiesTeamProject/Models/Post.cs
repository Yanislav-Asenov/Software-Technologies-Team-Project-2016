using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SoftwareTechnologiesTeamProject.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Post
    {
        public Post()
        {
            this.Date = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [AllowHtml]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        public string Body { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "datetime2")]
        public DateTime? Date { get; set; }

        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]


        [Display(Name = "Author Name")]
        public ApplicationUser Author { get; set; }

    }
}