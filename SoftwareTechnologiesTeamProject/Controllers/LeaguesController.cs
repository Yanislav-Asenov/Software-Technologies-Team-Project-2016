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
                .ToList();

            var viewModel = new StandingsViewModel
            {
                League = db.Leagues.Find(id),
                Teams = teams
            };

            return View(viewModel);
        }

        // GET: Leagues/Details/5
        public ActionResult Details(int? id)
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

            var league = db.Leagues.Find(teamInfo.League.Id);

            if (league.Teams.FirstOrDefault(t => t.Name == teamInfo.NewTeam.Name) != null)
            {
                this.AddNotification("Team already exists.", NotificationType.ERROR);
                return RedirectToAction("Standings", new { id = league.Id, leagueName = league.Name });
            }

            var newTeam = teamInfo.NewTeam;
            newTeam.League = league;
            newTeam.LeagueId = teamInfo.League.Id;

            league.Teams.Add(newTeam);
            db.Teams.Add(newTeam);
            db.SaveChanges();

            return RedirectToAction("Standings", new { id = league.Id });
        }

        [Authorize]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteTeam(int? id)
        {
            if (!User.IsInRole("Administrator"))
            {
                this.AddNotification("You are not admin.", NotificationType.ERROR);
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return HttpNotFound();
            }

            var team = db.Teams.Find(id);
            var leagueId = team.LeagueId;

            db.Teams.Remove(team);
            db.SaveChanges();

            return RedirectToAction("Standings", new { id = leagueId });
        }


        public ActionResult EditTeam(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var team = db.Teams.Find(id);


            return View(team);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditTeam(Team team)
        {
            var leagueId = team.LeagueId;

            var newTeamInfo = db.Teams.Find(team.Id);
            newTeamInfo.Update(team);
            newTeamInfo.League = db.Leagues.Find(leagueId);

            db.Entry(newTeamInfo).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Standings", new { id = leagueId });
        }

        // GET: Leagues/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
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

        // POST: Leagues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Date")] League league)
        {
            if (ModelState.IsValid)
            {
                db.Entry(league).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(league);
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
