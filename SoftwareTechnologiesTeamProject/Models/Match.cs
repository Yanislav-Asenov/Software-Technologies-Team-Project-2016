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

        public string HomeCoefficient { get; set; }

        public string DrawCoefficient { get; set; }

        public string AwayCoefficient { get; set; }

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

        public int GetVotesInPercents(string votesType)
        {
            double percents = -1;

            if (votesType == "draw")
            {
                percents = ((double)this.DrawVotesCount / this.TotalVotesCount) * 100;
            }
            else if (votesType == "home")
            {
                percents = ((double)this.HomeVotesCount / this.TotalVotesCount) * 100;
            }
            else if (votesType == "away")
            {
                percents = ((double)this.AwayVotesCount / this.TotalVotesCount) * 100;
            }

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
            return $"{DateTime:dd-MM-yyyy}";
        }

        public string GetTime()
        {
            return $"{DateTime:HH:mm}";
        }

        public Team GetOpponentName(string teamName)
        {
            if (teamName == this.HomeTeam.Name)
            {
                return this.AwayTeam;
            }
            else
            {
                return this.HomeTeam;
            }
        }

        public void UpdateTeams(string winner)
        {
            this.HomeTeam.GoalsFor += this.HomeTeamGoals;
            this.HomeTeam.GoalsAgainst += this.AwayTeamGoals;

            this.AwayTeam.GoalsFor += this.AwayTeamGoals;
            this.AwayTeam.GoalsAgainst += this.HomeTeamGoals;

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