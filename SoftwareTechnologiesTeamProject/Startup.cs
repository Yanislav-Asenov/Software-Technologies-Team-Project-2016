using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SoftwareTechnologiesTeamProject.Startup))]
namespace SoftwareTechnologiesTeamProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
