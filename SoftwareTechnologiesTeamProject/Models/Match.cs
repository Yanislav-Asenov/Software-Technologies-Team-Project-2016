namespace SoftwareTechnologiesTeamProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Match
    {
        [Key]
        public int Id { get; set; }

        public int? HomeTeamId { get; set; }

        [ForeignKey("HomeTeamId")]
        public Team HomeTeam { get; set; }

        public int? AwayTeamId { get; set; }

        [ForeignKey("AwayTeamId")]
        public Team AwayTeam { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "datetime2")]
        public DateTime? DateTime { get; set; }

        public string Result { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public virtual List<Vote> Votes { get; set; } = new List<Vote>();

        public int TotalVotesCount { get; set; }

        public int HomeVotesCount { get; set; }

        public int DrawVotesCount { get; set; }

        public int AwayVotesCount { get; set; }

        public double HomeCoefficient { get; set; }

        public double DrawCoefficient { get; set; }

        public double AwayCoefficient { get; set; }

        public Team GetWinner()
        {
            if (HomeTeamGoals > AwayTeamGoals)
            {
                return HomeTeam;
            }
            else if (HomeTeamGoals < AwayTeamGoals)
            {
                return AwayTeam;
            }
            else
            {
                return null;
            }
        }


    }
}