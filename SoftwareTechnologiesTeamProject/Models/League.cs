using System;
using System.Collections.Generic;

namespace SoftwareTechnologiesTeamProject.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class League
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Team> Teams { get; set; } = new List<Team>();

        public virtual List<Match> Matches { get; set; } = new List<Match>();

        [DataType(DataType.Date)]
        [Column(TypeName = "datetime2")]
        public DateTime StartDate { get; set; }
    }
}