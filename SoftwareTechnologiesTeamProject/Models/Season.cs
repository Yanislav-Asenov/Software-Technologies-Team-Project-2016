using System.Collections.Generic;

namespace SoftwareTechnologiesTeamProject.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Season
    {
        [Key]
        public int Id { get; set; }

        public List<League> Leagues { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }
    }
}