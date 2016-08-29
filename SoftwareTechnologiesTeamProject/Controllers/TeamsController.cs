namespace SoftwareTechnologiesTeamProject.Controllers
{
    using Models;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using ViewModels;

    public class TeamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Teams
        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.League);
            return View(teams.ToList());
        }

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
        public ActionResult Edit([Bind(Include = "Id,Name,Victories,Draws,Losses,GoalsFor,GoalsAgainst,Points,City,Coach,Stadium,StadiumCapacity,StadiumWidth,StadiumHeight,LogoLink,LeagueId")] Team team)
        {
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
        public ActionResult Delete(int? id)
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
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
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
