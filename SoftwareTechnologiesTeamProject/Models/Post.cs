namespace SoftwareTechnologiesTeamProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
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


        [Display(Name = "Author")]
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Tag> Tags { get; set; }

        public void AddTags(string[] tagNames, List<Tag> existingTags)
        {
            foreach (var tagName in tagNames)
            {
                if (existingTags.FirstOrDefault(t => t.Name == tagName) == null)
                {
                    Tags.Add(new Tag
                    {
                        Name = tagName,
                        Posts = new List<Post>() { this }
                    });
                }
                else
                {
                    Tag tag = existingTags.First(t => t.Name == tagName);
                    this.Tags.Add(tag);
                    tag.Posts.Add(this);
                }
            }
        }

    }
}