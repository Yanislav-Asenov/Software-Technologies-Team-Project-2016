using System.IO;
using System.Web.UI.WebControls;

namespace SoftwareTechnologiesTeamProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web.Mvc;
    using ViewModels;

    public class TeamMember
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public TeamMemberRole Role { get; set; }

        [Required]
        [StringLength(1000)]
        public string Info { get; set; }

        [Required]
        public Image Image { get; set; }
    }
}