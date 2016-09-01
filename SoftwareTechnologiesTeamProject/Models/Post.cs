namespace SoftwareTechnologiesTeamProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using ViewModels;

    public class Post
    {
        public Post()
        {
            this.Date = DateTime.Now;
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
        public DateTime Date { get; set; }

        public DateTime LastDateActive { get; set; }

        public virtual List<ApplicationUser> VotedUsers { get; set; } = new List<ApplicationUser>();

        public string AuthorId { get; set; }

        [Display(Name = "Author")]
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }

        [NotMapped]
        public Image Image { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Tag> Tags { get; set; } = new List<Tag>();

        public string ShortBody()
        {
            if (this.Body.Length > 300)
            {
                return this.Body.Substring(0, 300);
            }
            else
            {
                return this.Body;
            }
        }

        public string FullBody()
        {
            if (this.Body.Length > 300)
            {
                return this.Body.Substring(300);
            }

            return string.Empty;
        }

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

        public bool HasUserVoted(string userId)
        {
            return this.VotedUsers.Select(u => u.Id).ToList().Contains(userId);
        }

        public string GetDate()
        {
            string date = this.Date.ToString("dd.MMMM.yyyy", CultureInfo.InvariantCulture);
            return date;
        }

        public string GetTime()
        {
            return $"{Date:H:mm}";
        }

        public string GetShortTitle()
        {
            if (this.Title.Length > 40)
            {
                return this.Title.Substring(0, 40) + "...";
            }

            return this.Title;
        }

        public Comment GetRecentComment()
        {
            return this.Comments.OrderByDescending(c => c.DateCreated).First();
        }

        public void SetImage(List<Image> images)
        {

            this.Image = images.Where(i => i.ImagePath.Contains("PostId_" + this.Id))
                    .OrderByDescending(i => i.UploadedDate)
                    .FirstOrDefault();
        }
    }
}
