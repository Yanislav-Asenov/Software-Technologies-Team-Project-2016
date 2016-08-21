using System.Collections.Generic;

namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using Models;

    public class MatchDetailsViewModel
    {
        public Match Match { get; set; }

        public List<Match> HomeTeamHistory { get; set; }

        public List<Match> AwayTeamHistory { get; set; }
    }
}