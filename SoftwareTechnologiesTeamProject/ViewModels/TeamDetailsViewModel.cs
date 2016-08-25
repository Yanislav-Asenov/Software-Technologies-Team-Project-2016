using System.Collections.Generic;

namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using Models;
    using System.Linq;

    public class TeamDetailsViewModel
    {
        public Team Team { get; set; }

        public List<Match> Matches { get; set; }

        public List<Match> HomeGames { get; set; }

        public List<Match> AwayGames { get; set; }

        public string LeagueName { get; set; }


        public List<Match> GetPlayedMatches()
        {
            return this.Matches.Where(m => m.IsResultUpdated).ToList();
        }

        public List<Match> GetHomePlayedMatches()
        {
            return this.Matches.Where(m => m.HomeTeam.Name == this.Team.Name && m.IsResultUpdated).ToList();
        }

        public List<Match> GetAwayPlayedMatches()
        {
            return this.Matches.Where(m => m.AwayTeam.Name == this.Team.Name && m.IsResultUpdated).ToList();
        }

        public int GetTotalWinsCount()
        {
            return this.GetPlayedMatches().Count(m => m.GetWinnerName() == this.Team.Name);
        }

        public int GetHomeWinsCount()
        {
            return this.GetHomePlayedMatches().Count(m => m.GetWinnerName() == this.Team.Name);
        }

        public int GetAwayWinsCount()
        {
            return this.GetAwayPlayedMatches().Count(m => m.GetWinnerName() != this.Team.Name);
        }

        public Dictionary<string, int> MostCommonResults()
        {
            Dictionary<string, int> mostCommonResult = new Dictionary<string, int>();

            foreach (var match in this.Matches)
            {
                if (!mostCommonResult.ContainsKey(match.Result))
                {
                    mostCommonResult.Add(match.Result, 1);
                }

                mostCommonResult[match.Result]++;
            }

            return mostCommonResult;
        }
    }
}