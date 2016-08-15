namespace SoftwareTechnologiesTeamProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web.Mvc;
    using ViewModels;

    public class Post
    {
        public Post()
        {
            this.Date = DateTime.Now;
            this.Tags = new List<Tag>();
            this.VotedUsers = new List<ApplicationUser>();
            this.LastDateActive = DateTime.Now;
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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}",
               ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        public DateTime? LastDateActive { get; set; }

        public virtual List<ApplicationUser> VotedUsers { get; set; }

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

        public string GetTagNames()
        {
            string tagNames = "";

            if (Tags.Count != 0)
            {
                tagNames = "#" + string.Join("#", Tags.Select(t => t.Name));
            }

            return tagNames;
        }

        public void Update(ApplicationUser author, EditPostViewModel viewModel)
        {
            this.Author = author;
            this.AuthorId = author.Id;
            this.Title = viewModel.Title;
            this.Date = DateTime.Now;
            this.Body = viewModel.Body;
        }

        public bool HasVoted(string userId)
        {
            return this.VotedUsers.Select(u => u.Id).ToList().Contains(userId);
        }

        public string GetDate()
        {

            return $"{Date:dd/MMMM/yyyy}";
        }

        public string GetTime()
        {
            return $"{Date:H:mm}";
        }
    }
}