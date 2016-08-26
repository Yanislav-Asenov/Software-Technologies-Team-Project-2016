namespace SoftwareTechnologiesTeamProject.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Comment
    {
        public Comment()
        {
            this.DateCreated = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public string AuthorId { get; set; }

        [Required]
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        public int PostId { get; set; }

        public string GetDate()
        {
            return $"{DateCreated:dd/MMMM/yyyy}";
        }

        public string GetTime()
        {
            return $"{DateCreated:H:mm}";
        }

        public string GetShortContent()
        {
            if (this.Content.Length > 100)
            {
                return this.Content.Substring(0, 100) + "...";
            }

            return this.Content;
        }
    }
}