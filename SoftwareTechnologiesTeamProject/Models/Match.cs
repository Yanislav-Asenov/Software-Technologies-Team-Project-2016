using System.Collections.Generic;

namespace SoftwareTechnologiesTeamProject.Models
{
    public class Match
    {
        public Match()
        {
            HomeTeamVotes = 0;
            AwayTeamVotes = 0;
            DrawVotes = 0;
            VotedUsers = new List<ApplicationUser>();
            Result = string.Empty;
        }

        public int Id { get; set; }

        public string Time { get; set; }

        public string Result { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public int HomeTeamScore { get; set; }

        public int AwayTeamScore { get; set; }

        public int HomeTeamVotes { get; set; }

        public int DrawVotes { get; set; }

        public int AwayTeamVotes { get; set; }

        public virtual List<ApplicationUser> VotedUsers { get; set; }

        public int GetTotalVotes()
        {
            return this.HomeTeamVotes + this.AwayTeamVotes + this.DrawVotes;
        }

        public bool HasVoted(ApplicationUser user)
        {
            return this.VotedUsers.Contains(user);
        }
    }
}