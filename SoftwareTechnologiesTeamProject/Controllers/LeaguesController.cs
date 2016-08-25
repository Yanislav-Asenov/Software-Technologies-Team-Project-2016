using SoftwareTechnologiesTeamProject.Models;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;

namespace SoftwareTechnologiesTeamProject.Controllers
{
    using Extensions;
    using System.Linq;
    using ViewModels;

    public class LeaguesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Leagues
        public ActionResult Index()
        {
            return View(db.Leagues.ToList());
        }

        public ActionResult Standings(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var teams = db.Teams
                .Include(t => t.Matches)
                .Where(t => t.LeagueId == id)
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalsFor - t.GoalsAgainst)
                .ThenByDescending(t => t.GoalsFor)
                .ToList();

            var viewModel = new StandingsViewModel
            {
                LeagueId = id,
                League = db.Leagues.Find(id),
                Teams = teams
            };

            //Setting match history for each team
            var matches = db.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .ToList();

            foreach (var team in viewModel.Teams)
            {
                team.Matches = matches.Where(
                    m => (m.HomeTeam.Name == team.Name || m.AwayTeam.Name == team.Name) &&
                    m.IsResultUpdated)
                    .ToList();

                team.NextMatch = matches
                    .Where(m => !m.IsResultUpdated && (m.HomeTeam.Name == team.Name || m.AwayTeam.Name == team.Name))
                    .OrderBy(m => m.DateTime)
                    .FirstOrDefault();
            }

            return View(viewModel);
        }


        // GET: Leagues/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Leagues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Date")] League league)
        {
            if (ModelState.IsValid)
            {
                db.Leagues.Add(league);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(league);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult AddTeam([Bind(Include = "LeagueId,NewTeam")]StandingsViewModel teamInfo)
        {
            if (teamInfo.NewTeam.Name == null)
            {
                return HttpNotFound();
            }

            var league = db.Leagues.Find(teamInfo.LeagueId);

            if (league.Teams.FirstOrDefault(t => t.Name == teamInfo.NewTeam.Name) != null)
            {
                this.AddNotification("Team already exists.", NotificationType.ERROR);
                return RedirectToAction("Standings", new { id = league.Id, leagueName = league.Name });
            }

            var newTeam = teamInfo.NewTeam;
            newTeam.League = league;
            newTeam.LeagueId = league.Id;

            league.Teams.Add(newTeam);
            db.Teams.Add(newTeam);
            db.SaveChanges();

            return RedirectToAction("Standings", new { id = league.Id });
        }

        // GET: Leagues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            League league = db.Leagues.Find(id);
            if (league == null)
            {
                return HttpNotFound();
            }
            return View(league);
        }

        // POST: Leagues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            League league = db.Leagues.Find(id);
            db.Leagues.Remove(league);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
