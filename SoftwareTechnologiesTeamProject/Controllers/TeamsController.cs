namespace SoftwareTechnologiesTeamProject.Controllers
{
    using Extensions;
    using Models;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using ViewModels;

    public class TeamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Team team = db.Teams.Find(id);

            var matches = db.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(m => m.HomeTeamId == id || m.AwayTeamId == id)
                .OrderByDescending(m => m.DateTime)
                .ToList();


            var teams = db.Teams
                .Where(t => t.LeagueId == team.LeagueId)
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalsFor - t.GoalsAgainst)
                .ThenByDescending(t => t.GoalsFor)
                .ToList();

            TeamDetailsViewModel viewModel = new TeamDetailsViewModel
            {
                Team = team,
                Matches = matches,
                League = db.Leagues.First(l => l.Id == team.LeagueId),
                Standings = teams
            };

            return View("Details", viewModel);
        }

        // GET: Teams/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(string id)
        {
            int leagueId = int.Parse(id);

            ViewBag.LeagueId = new SelectList(db.Leagues.Where(l => l.Id == leagueId).ToList(), "Id", "Name");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "Id,Name,Matches,Victories,Draws,Losses,GoalsFor,GoalsAgainst,Points,City,Coach,Stadium,StadiumCapacity,StadiumWidth,StadiumHeight,LogoLink,LeagueId")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Standings", "Leagues", new { id = team.LeagueId });
            }

            ViewBag.LeagueId = new SelectList(db.Leagues.Where(l => l.Id == team.LeagueId).ToList(), "Id", "Name", team.LeagueId);
            return View(team);
        }

        // GET: Teams/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeagueId = new SelectList(db.Leagues, "Id", "Name", team.LeagueId);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "Id,Name,Victories,Draws,Losses,GoalsFor,GoalsAgainst,Points,City,Coach,Stadium,StadiumCapacity,StadiumWidth,StadiumHeight,LogoLink,LeagueId")] Team team)
        {
            if (team.Draws < 0 || team.StadiumWidth < 0 || team.GoalsFor < 0 ||
                team.GoalsAgainst < 0 || team.Losses < 0 || team.Points < 0 ||
                team.StadiumCapacity < 0 || team.StadiumHeight < 0 || team.Victories < 0)
            {
                this.AddNotification("Team stats must be positive numbers.", NotificationType.ERROR);
                return RedirectToAction("Details", new { id = team.Id });
            }

            var league = db.Leagues.FirstOrDefault(l => l.Id == team.LeagueId);
            if (league == null)
            {
                this.AddNotification("League not found.", NotificationType.ERROR);
                return RedirectToAction("Details", new { id = team.Id });
            }

            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Standings", "Leagues", new { id = team.LeagueId });
            }
            ViewBag.LeagueId = new SelectList(db.Leagues, "Id", "Name", team.LeagueId);
            return View(team);
        }

        // GET: Teams/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Include(t => t.League).FirstOrDefault(t => t.Id == id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Include(t => t.Matches).FirstOrDefault(T => T.Id == id);

            if (team == null)
            {
                return HttpNotFound();
            }

            var matchesToDelete = db.Matches.Where(m => m.HomeTeamId == team.Id || m.AwayTeamId == team.Id).ToList();

            db.Matches.RemoveRange(matchesToDelete);

            var leagueId = team.LeagueId;
            db.Teams.Remove(team);
            db.SaveChanges();

            return RedirectToAction("Standings", "Leagues", new { id = leagueId });
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
