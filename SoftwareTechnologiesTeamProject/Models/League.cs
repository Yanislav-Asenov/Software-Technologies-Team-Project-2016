using System;
using System.Collections.Generic;

namespace SoftwareTechnologiesTeamProject.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "datetime2")]
        [DisplayName("Season start date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
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