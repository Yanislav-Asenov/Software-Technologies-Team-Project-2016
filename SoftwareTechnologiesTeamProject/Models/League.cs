using System;
using System.Collections.Generic;

namespace SoftwareTechnologiesTeamProject.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class League
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("League name")]
        public string Name { get; set; }

        public virtual List<Team> Teams { get; set; } = new List<Team>();

        public virtual List<Match> Matches { get; set; } = new List<Match>();

        [Required]
        [DisplayName("Season start date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayName("Season end date")]
        public DateTime EndDate { get; set; }

        [Required]
        public string Country { get; set; }

        public string GetSeason()
        {
            return $"{StartDate:yyyy} - {EndDate:yyyy}";
        }

    }
}