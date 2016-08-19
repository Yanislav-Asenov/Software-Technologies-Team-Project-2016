using System.Collections.Generic;

namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using Models;

    public class StandingsViewModel
    {
        public string LeagueName { get; set; }

        public List<Team> Teams { get; set; }

        public int? LeagueId { get; set; }

        public Team NewTeam { get; set; }
    }
}