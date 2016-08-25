namespace SoftwareTechnologiesTeamProject.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [RegularExpression("^[0-9]{1,}$", ErrorMessage = "Input must be a number.")]
        public int GamesPlayed { get; set; }

        [RegularExpression("^[0-9]{1,}$", ErrorMessage = "Input must be a number.")]
        public int Victories { get; set; }

        [RegularExpression("^[0-9]{1,}$", ErrorMessage = "Input must be a number.")]
        public int Draws { get; set; }

        [RegularExpression("^[0-9]{1,}$", ErrorMessage = "Input must be a number.")]
        public int Losses { get; set; }

        [RegularExpression("^[0-9]{1,}$", ErrorMessage = "Input must be a number.")]
        public int GoalsFor { get; set; }

        [RegularExpression("^[0-9]{1,}$", ErrorMessage = "Input must be a number.")]
        public int GoalsAgainst { get; set; }

        [RegularExpression("^[0-9]{1,}$", ErrorMessage = "Input must be a number.")]
        public int Points { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string Coach { get; set; }

        [Required]
        [StringLength(100)]
        public string Stadium { get; set; }

        [Required]
        [DisplayName("Stadium capacity")]
        public int StadiumCapacity { get; set; }

        [Required]
        [DisplayName("Stadium width")]
        public int StadiumWidth { get; set; }

        [Required]
        [DisplayName("Stadium height")]
        public int StadiumHeight { get; set; }

        [Required]
        [StringLength(300)]
        public string LogoLink { get; set; }

        [Required]
        public int? LeagueId { get; set; }

        [ForeignKey("LeagueId")]
        public League League { get; set; }

        public virtual List<Match> Matches { get; set; }

        [NotMapped]
        public Match NextMatch { get; set; }

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
            this.City = team.City;
            this.Coach = team.Coach;
            this.Stadium = team.Stadium;
            this.StadiumCapacity = team.StadiumCapacity;
            this.StadiumWidth = team.StadiumWidth;
            this.StadiumHeight = team.StadiumHeight;
        }

        public string GetGoalDifference()
        {
            int goalDiff = this.GoalsFor - this.GoalsAgainst;

            return goalDiff >= 0 ? "+" + goalDiff : "" + goalDiff;
        }

        public string GetStadiumSize()
        {
            return $"{this.StadiumWidth} x {this.StadiumHeight} m";
        }
    }
}