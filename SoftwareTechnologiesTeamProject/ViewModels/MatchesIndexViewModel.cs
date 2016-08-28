namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using Models;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class MatchesIndexViewModel
    {
        public List<Match> Matches { get; set; }

        public List<League> Leagues { get; set; }

        public Dictionary<string, List<Match>> GetUpcomingMatches()
        {
            var upcomingMatches = new Dictionary<string, List<Match>>();

            foreach (var match in this.Matches.Where(m => m.IsResultUpdated == false).OrderBy(m => m.DateTime))
            {
                string key = match.DateTime.ToString("dddd dd MMMM yyyy", CultureInfo.InvariantCulture);

                if (!upcomingMatches.ContainsKey(key))
                {
                    upcomingMatches.Add(key, new List<Match>());
                    upcomingMatches[key].Add(match);
                }
                else
                {
                    upcomingMatches[key].Add(match);
                }
            }

            return upcomingMatches;
        }
    }
}