namespace SoftwareTechnologiesTeamProject.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public  List<Post> Posts { get; set; } = new List<Post>();

        public string Body { get; set; }            

    }
}
