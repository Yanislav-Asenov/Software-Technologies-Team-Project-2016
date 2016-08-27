using SoftwareTechnologiesTeamProject.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using SoftwareTechnologiesTeamProject.ViewModels;
using System.Collections.Generic;

namespace SoftwareTechnologiesTeamProject.Controllers
{
    public class AboutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.Message = "Here you can find more information about us.";
            var teamMembers = new AboutUsViewModel();
            teamMembers.Developers = getTeamMembersByRole(TeamMemberRole.Developer);
            teamMembers.Designers = getTeamMembersByRole(TeamMemberRole.Designer);
            return View(teamMembers);
        }

        private List<TeamMember> getTeamMembersByRole(TeamMemberRole role)
        {
            return db.TeamMembers
                .Include(m => m.Image)
                .Where(m => m.Role.Id == role.Id)
                .ToList();
        }
    }
} 