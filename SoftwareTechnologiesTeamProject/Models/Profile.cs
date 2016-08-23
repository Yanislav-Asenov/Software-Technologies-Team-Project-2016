using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;


namespace SoftwareTechnologiesTeamProject.Models
{



    public class Profile
    {

        public Profile()
        {

        }


          [Key]
        public int Id { get; set; }

     
        
        public string UserId { get; set; }


        
        public ApplicationUser User { get; set; }
        
       
        public int Age { get; set; }

        [StringLength(50)]
        public string City { get; set; }

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