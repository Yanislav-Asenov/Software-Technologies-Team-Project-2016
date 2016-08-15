namespace SoftwareTechnologiesTeamProject.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        public Tag()
        {
            this.Posts = new List<Post>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual List<Post> Posts { get; set; }

    }
}