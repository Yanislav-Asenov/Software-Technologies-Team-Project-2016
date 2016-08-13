namespace SoftwareTechnologiesTeamProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
    public class Post
    {
        public Post()
        {
            this.Date = DateTime.Now;
            this.Comments = new List<Comment>();
            this.Tags = new List<Tag>();
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


        [Display(Name = "Author")]
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Tag> Tags { get; set; }
    }
}