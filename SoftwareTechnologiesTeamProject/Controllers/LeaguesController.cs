using SoftwareTechnologiesTeamProject.Models;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;

namespace SoftwareTechnologiesTeamProject.Controllers
{
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
        public ActionResult Create([Bind(Include = "Id,Name,StartDate,EndDate,Country")] League league)
        {
            if (ModelState.IsValid)
            {
                db.Leagues.Add(league);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(league);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(League league)
        {
            if (ModelState.IsValid)
            {
                league.Matches = db.Matches.Where(m => m.LeagueId == league.Id).ToList();
                league.Teams = db.Teams.Where(t => t.LeagueId == league.Id).ToList();
                db.Entry(league).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Standings", new { id = league.Id });
            }

            return RedirectToAction("Edit", new { id = league.Id });
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
