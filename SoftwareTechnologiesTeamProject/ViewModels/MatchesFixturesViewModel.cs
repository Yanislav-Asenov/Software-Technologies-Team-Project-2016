namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class MatchesFixturesViewModel
    {
        public Dictionary<string, List<Match>> Matches { get; set; }

        public List<League> Leagues { get; set; }

        public DateTime DateForMatches { get; set; } = DateTime.Now;

        public string MonthName
        {
            get
            {
                return new DateTime(this.DateForMatches.Year,
                    this.DateForMatches.Month,
                    this.DateForMatches.Day)
                    .ToString("MMMM", CultureInfo.InvariantCulture);
            }
        }

        public int LastDayInMonth => DateTime.DaysInMonth(this.DateForMatches.Year, this.DateForMatches.Month);

        public int GetMissingDays(string dayOfWeek)
        {
            int missingDays = -1;
            string[] daysOfWeek = new[] { "Monday", "TuesDay", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            for (int i = 0; i < daysOfWeek.Length; i++)
            {
                string currentDayOfWeek = daysOfWeek[i];
                if (currentDayOfWeek == dayOfWeek)
                {
                    missingDays = i;
                    break;
                }
            }

            return missingDays;
        }

        public string GetShortDate()
        {
            return $"{this.DateForMatches:dd-MM-yyyy}";
        }

        public string GetLongDate()
        {
            string longDate = this.DateForMatches.ToString("dddd dd, MMMM, yyyy", CultureInfo.InvariantCulture);
            return longDate;
        }

        public void SetLeaguesUpcomingMatches(List<League> leagues, List<Match> matches)
        {
            var leaguesUpcomingMatches = new Dictionary<string, List<Match>>();
            var date = this.DateForMatches.Date;

            foreach (var league in leagues)
            {
                var currentDateMatches = league.Matches.Where(m => m.DateTime.Date == date).ToList();

                if (currentDateMatches.Count != 0)
                {
                    if (!leaguesUpcomingMatches.ContainsKey(league.Name))
                    {
                        leaguesUpcomingMatches.Add(league.Name, new List<Match>());
                    }

                    leaguesUpcomingMatches[league.Name] = currentDateMatches;
                }
            }

            this.Matches = leaguesUpcomingMatches;
        }
    }
}