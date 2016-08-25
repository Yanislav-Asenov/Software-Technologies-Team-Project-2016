namespace SoftwareTechnologiesTeamProject.ViewModels
{
    public class TeamResultStatsViewModel
    {
        public string Result { get; set; }

        public int MatchesPlayed { get; set; }

        public string GetPercents(int totalMatchesPlayed)
        {
            double result = this.MatchesPlayed / (double)totalMatchesPlayed;

            return $"{result:P2}";
        }
    }
}