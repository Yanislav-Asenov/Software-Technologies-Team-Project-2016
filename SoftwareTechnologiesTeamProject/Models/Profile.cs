using System.ComponentModel.DataAnnotations;

namespace SoftwareTechnologiesTeamProject.Models
{
    using System;

    public class Profile
    {
        public int Id { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "City")]
        [StringLength(50)]
        public string City { get; set; }

        [Display(Name = "Interests")]
        [StringLength(200)]
        public string Interests { get; set; }

        [StringLength(500)]
        [Display(Name = "More Info")]
        public string MoreInfo { get; set; }

        public Profile()
        {
            CreationDate = DateTime.Now;
        }
    }
}