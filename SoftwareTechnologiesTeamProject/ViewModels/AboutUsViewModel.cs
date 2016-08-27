namespace SoftwareTechnologiesTeamProject.ViewModels
{
    using Models;
    using System.Collections.Generic;

    public class AboutUsViewModel
    {
        public List<TeamMember> Developers { get; set; }

        public List<TeamMember> Designers { get; set; }
    }
}