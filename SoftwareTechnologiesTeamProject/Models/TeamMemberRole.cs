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

    public class TeamMemberRole
    {
        public static readonly TeamMemberRole Developer = new TeamMemberRole { Id = 1, Lable = "Developer" };
        public static readonly TeamMemberRole Designer = new TeamMemberRole { Id = 2, Lable = "Designer" };

        [Key]
        public int Id { get; set; }

        [Required]
        public string Lable { get; set; }
    }
}