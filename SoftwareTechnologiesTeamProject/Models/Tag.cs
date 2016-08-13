using System.Collections.Generic;

namespace SoftwareTechnologiesTeamProject.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        public Tag()
        {

        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual List<Post> Posts { get; set; }

    }
}