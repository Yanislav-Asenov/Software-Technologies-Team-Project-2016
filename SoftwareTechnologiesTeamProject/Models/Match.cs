namespace SoftwareTechnologiesTeamProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class Match
    {
        public Match()
        {
            HomeTeamGoals = 0;
            AwayTeamGoals = 0;
            HomeVotesCount = 0;
            AwayVotesCount = 0;
            DrawVotesCount = 0;
            IsResultUpdated = false;
        }

        [Key]
        public int Id { get; set; }

        public string LeagueName { get; set; }

        public int? LeagueId { get; set; }

        [ForeignKey("LeagueId")]
        public League League { get; set; }

        public int? HomeTeamId { get; set; }

        [ForeignKey("HomeTeamId")]
        public Team HomeTeam { get; set; }

        public int? AwayTeamId { get; set; }

        [ForeignKey("AwayTeamId")]
        public Team AwayTeam { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}",
               ApplyFormatInEditMode = true)]
        public DateTime? DateTime { get; set; }

        public string Result => HomeTeamGoals + " - " + AwayTeamGoals;

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        public bool IsResultUpdated { get; set; }

        public virtual List<Vote> Votes { get; set; } = new List<Vote>();

        public int TotalVotesCount => HomeVotesCount + AwayVotesCount + DrawVotesCount;

        public int HomeVotesCount { get; set; }

        public int DrawVotesCount { get; set; }

        public int AwayVotesCount { get; set; }

        public double HomeCoefficient { get; set; }

        public double DrawCoefficient { get; set; }

        public double AwayCoefficient { get; set; }

        public string GetWinningSide()
        {
            if (HomeTeamGoals > AwayTeamGoals)
            {
                return "home";
            }
            else if (HomeTeamGoals < AwayTeamGoals)
            {
                return "away";
            }
            else
            {
                return "draw";
            }
        }

        public int GetVotesInPercents(int votes)
        {
            double percents = ((double)votes / this.TotalVotesCount) * 100;
            int result = (int)Math.Round(percents);

            return result > 0 ? result : 0;
        }

        public void IncreaseVoteCount(string voteType)
        {
            if (voteType == "home")
            {
                HomeVotesCount++;
            }
            else if (voteType == "draw")
            {
                DrawVotesCount++;
            }
            else if (voteType == "away")
            {
                AwayVotesCount++;
            }
        }

        public bool HasUserVoted(string userId)
        {
            if (Votes.FirstOrDefault(v => v.VoterId == userId) == null)
            {
                return false;
            }

            return true;
        }

        public string GetWinnerName()
        {
            if (HomeTeamGoals > AwayTeamGoals)
            {
                return this.HomeTeam.Name;
            }
            else if (HomeTeamGoals < AwayTeamGoals)
            {
                return this.AwayTeam.Name;
            }
            else
            {
                return "draw";
            }
        }

        public string GetDate()
        {
            return $"{DateTime:dd-MMM-yyyy}";
        }

        public string GetTime()
        {
            return $"{DateTime:HH:mm}";
        }

        public void UpdateTeams(string winner)
        {
            this.HomeTeam.GoalsFor += this.HomeTeamGoals;
            this.HomeTeam.GoalsAgainst += this.AwayTeamGoals;
            this.HomeTeam.GoalDifference = this.HomeTeamGoals - this.AwayTeamGoals;
            this.HomeTeam.GamesPlayed++;

            this.AwayTeam.GoalsFor += this.AwayTeamGoals;
            this.AwayTeam.GoalsAgainst += this.HomeTeamGoals;
            this.AwayTeam.GoalDifference = this.AwayTeamGoals - this.HomeTeamGoals;
            this.AwayTeam.GamesPlayed++;

            if (winner == "home")
            {
                this.HomeTeam.Victories++;
                this.HomeTeam.Points += 3;
                this.AwayTeam.Losses++;
            }
            else if (winner == "away")
            {
                this.AwayTeam.Victories++;
                this.AwayTeam.Points += 3;
                this.HomeTeam.Losses++;
            }
            else if (winner == "draw")
            {
                this.AwayTeam.Draws++;
                this.HomeTeam.Points++;
                this.HomeTeam.Draws++;
                this.AwayTeam.Points++;
            }
        }
    }
}