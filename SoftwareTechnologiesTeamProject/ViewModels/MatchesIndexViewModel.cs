namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using Models;
    using System.Collections.Generic;

    public class MatchesIndexViewModel
    {
        public List<Match> Matches { get; set; }

        public ApplicationUser User { get; set; }
    }
}