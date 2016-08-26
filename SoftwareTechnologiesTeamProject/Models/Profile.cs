using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SoftwareTechnologiesTeamProject.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }


        public int Age { get; set; }


        [Display(Name ="City")]
        [StringLength(50)]
        public string City { get; set; }



        [Display(Name = "FullName")]
        [StringLength(50)]
        public string FullName { get; set; }

        [Display(Name = "Interests")]
        [StringLength(200)]
        public string Interests { get; set; }

        [StringLength(500)]
        [Display(Name = "More Info")]
        public string MoreInfo { get; set; }

        [StringLength(200)]
        public string ProfilePic { get; set; }
    }
}