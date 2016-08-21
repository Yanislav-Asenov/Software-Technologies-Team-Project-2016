namespace SoftwareTechnologiesTeamProject.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Team
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int GamesPlayed { get; set; }

        public int Victories { get; set; }

        public int Draws { get; set; }

        public int Losses { get; set; }

        public int GoalsFor { get; set; }

        public int GoalsAgainst { get; set; }

        public int GoalDifference { get; set; }

        public int Points { get; set; }

        public string LogoLink { get; set; }

        [Required]
        public int? LeagueId { get; set; }

        [ForeignKey("LeagueId")]
        public League League { get; set; }

        public virtual List<Match> Matches { get; set; }

        public Team()
        {
            GamesPlayed = 0;
            GoalsFor = 0;
            GoalsAgainst = 0;
            Losses = 0;
            Victories = 0;
            Draws = 0;
            Points = 0;
        }

        public void Update(Team team)
        {
            this.Name = team.Name;
            this.GamesPlayed = team.GamesPlayed;
            this.GoalsFor = team.GoalsFor;
            this.Victories = team.Victories;
            this.Draws = team.Draws;
            this.Losses = team.Losses;
            this.GoalsFor = team.GoalsFor;
            this.GoalsAgainst = team.GoalsAgainst;
            this.Points = team.Points;
        }
    }
}