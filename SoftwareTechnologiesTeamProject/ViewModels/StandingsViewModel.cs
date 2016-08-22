using System.Collections.Generic;

namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using Models;

    public class StandingsViewModel
    {
        public int? LeagueId { get; set; }

        public League League { get; set; }

        public List<Team> Teams { get; set; }

        public Team NewTeam { get; set; }
    }
}