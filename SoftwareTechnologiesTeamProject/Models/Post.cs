namespace SoftwareTechnologiesTeamProject.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
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



        [Display(Name = "Author full name")]
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }

    }
}